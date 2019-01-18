<template>
  <div>
    <loading-icon size="2" v-if="isLoading"></loading-icon>
    <div v-else>
      <brand-tabs>
        <tabs v-if="domains.length"
          :unsavedChangesPresent="unsavedChangesPresent"
          :requestedTab ="requestedTab"
          @tabSelected="tabSelected">
          <tab-pane v-for="(domain, key) in domains" :icon ="getDomainIcon(domain)" :key="key" :label="domain.name" :selected="key===0">

          <!-- Domain verification -->
            <!-- Domain not verfied -->
            <div v-if="domain.status===domainStatus.notVerified">
              <brand-primary-btn :disabled="domain.verificationInProgress" @click="verifyDomain(domain.name, key)" >Verify Domain</brand-primary-btn>
              <!-- Domain verification not in progress -->
              <div v-if="!domain.verificationInProgress">
                <div class="product-configuration-warning">
                  <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>You need to verify your domain in order to start assigning this product to users. Please check your email for domain verification instructions. Don't have the verification Email?
                  You can <span @click="openTxtRecordsModal(domain.name)" class="in-text-action">resend verification email.</span>
                </div>
              </div>
              <!-- Domain verification in progress -->
              <div v-else class="product-configuration-warning">
                <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>The verification process for this domain is in progress.
                It may take up to 5 minutes to add your domain to the Office365 Portal.
                Please <span @click="refreshPage()" class="in-text-action">refresh</span> the page or come back later.
              </div>
            </div>

            <!-- Domain verified  -->
            <div v-if="domain.status === domainStatus.verified">
              <div class="no-users-message" v-if="domain.users.totalNumberOfRecords === 0 && !usersLoading">
                You currently don't have any users on this domain.
              </div>
              <div v-else>
                <div class="columns">
                  <div class="column">
                    <cloud-plus-textfield :hasIconRight="true" :icon="'fa-search'" v-model="search" :placeholder="'Search'" class="search-field"></cloud-plus-textfield>
                  </div>
                  <div class="column">
                    <brand-primary-btn @click="openSummaryModal()" :disabled="(!anyProductSelected && !anyRolesSelected) || !anyUsersSelected" class="is-pulled-right is-marginless">Save</brand-primary-btn>
                    <brand-reverse-primary-btn @click="startFederationProcess()" v-if="!domain.isFederated" class="is-pulled-right">Federate Domain</brand-reverse-primary-btn>
                  </div>
                </div>
                <div class="columns">
                  <div class="column is-7">
                    <loading-icon v-if="usersLoading" size="2"></loading-icon>
                    <domain-users
                      v-if="domain.users.results && !usersLoading"
                      @userSelected="setMultiUserLicenceUsers"
                      :search="search"
                      :domainIndex="key"
                      :domainName="domain.name">
                    </domain-users>
                  </div>
                  <div class="column is-5 roles-and-assignment">
                    <office365-product-and-roles-assignment
                      @roleSelected="setUserLicenceRoles"
                      @productSelected="setUserLicenceProductIdentifier"
                      class="product-and-role-assignment">
                    </office365-product-and-roles-assignment>
                  </div>
                </div>
              </div>
            </div>

          <!-- Domain configuration -->
            <!-- Domain not configured -->
            <div v-if="domain.status === domainStatus.notConfigured">
              <brand-primary-btn :disabled="domain.configurationInProgress" @click="configureDomain(domain.name, key)">Configure Domain</brand-primary-btn>
              <div v-if="!domain.configurationInProgress" class="product-configuration-warning">
                <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>Domain is not added to Office 365 Portal. To use this domain for Office 365 service,
                please configure the domain and go through the domain verification process.
              </div>
              <!-- Domain configuration in progress -->
              <div v-if="domain.configurationInProgress" class="product-configuration-warning">
                <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>The configuration process for this domain is in progress.
                It may take up to 5 minutes to add your domain to the Office365 Portal.
                Please <span @click="refreshPage()" class="in-text-action">refresh</span> the page or come back later.
              </div>
            </div>
          </tab-pane>
        </tabs>
      </brand-tabs>
      <resend-txt-records-modal :companyEmail = "company.email" v-if="showTxtRecordsModal" :showModal="showTxtRecordsModal" @closeModal="closeTxtRecordsModal"></resend-txt-records-modal>
      <edit-unfederated-users-modal
        @usersEdited ="editMultiUserSuccesfully"
        v-if="showUnfederatedUsersModal"
        :showModal="showUnfederatedUsersModal"
        @closeModal="closeUnfederatedUsersModal">
      </edit-unfederated-users-modal>
      <confirmation-modal
        v-if="showConfirmChangesModal" :showModal="showConfirmChangesModal"
        @cancel="stayOnTab"
        @confirm = "moveToTab"
        title = "Confirm Changes"
        cancelText="No"
        confirmText="Yes">You have unsaved changes for the current domain. Are you sure you want to move to another domain without saving them?
      </confirmation-modal>
      <confirmation-modal
        v-if="showConfirmFederateModal" :showModal="showConfirmFederateModal"
        :title="'Federate domain'"
        :showLoadingIcon="federatingDomain"
        :disableConfirmButton="federatingDomain"
        :disableCancelButton="federatingDomain"
        @cancel="closeFederateModal"
        @confirm = "federateDomain"
        confirmText="Federate">Are you sure you want to federate domain {{domains[requestedTab].name}}?
        <div v-show="federatingDomain" class="is-danger">
          <span class="has-text-danger">
            * This operation can take a little while. Please do not leave this page or close your browser.
          </span>
        </div>
      </confirmation-modal>
      <confirmation-modal
        v-if="showSummaryModal" :showModal="showSummaryModal"
        :title="'Summary'"
        @cancel="closeSummaryModal"
        @confirm = "saveMultiUserChanges"
        confirmText="Save"
        width="850px">
        <div>
          <office365-multi-user-edit-summary></office365-multi-user-edit-summary>
        </div>
      </confirmation-modal>
    </div>
  </div>
</template>

<script>
import { mapMutations, mapGetters, mapActions } from 'vuex';
import { office365DomainStatus } from '@/assets/constants/commonConstants';
import PasswordGenerator from 'generate-password';
import office365UtilitiesService from '@/services/office365UtilitiesService';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import { Tabs, TabPane } from '@/components/shared/tabs';
import Office365ProductAndRolesAssignment from '@/components/services/office365/Office365ProductAndRolesAssignment';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import ResendTxtRecordsModal from './ResendTxtRecordsModal';
import EditUnfederatedUsersModal from './EditUnfederatedUsersModal';
import DomainUsers from './DomainUsers';
import Office365MultiUserEditSummary from './Office365MultiUserEditSummary';

export default {
  data() {
    return {
      showTxtRecordsModal: false,
      showConfirmChangesModal: false,
      showConfirmFederateModal: false,
      showUnfederatedUsersModal: false,
      showSummaryModal: false,
      federatingDomain: false,
      requestedTab: -1,
      search: '',
      domainStatus: {
        notConfigured: office365DomainStatus.NotConfigured,
        notVerified: office365DomainStatus.NotVerified,
        verified: office365DomainStatus.Verified,
      },
      domains: [],
      unfederatedUsers: [],
      usersLoading: false,
      pageSize: 10,
    };
  },
  mixins: [toasterMixin, loadingMixin],
  components: {
    Tabs,
    TabPane,
    Office365ProductAndRolesAssignment,
    ResendTxtRecordsModal,
    DomainUsers,
    ConfirmationModal,
    EditUnfederatedUsersModal,
    Office365MultiUserEditSummary,
  },
  computed: {
    unsavedChangesPresent() {
      return this.anyProductSelected || this.anyUsersSelected || this.anyRolesSelected;
    },
    anyProductSelected() {
      return this.multiUserLicenceEdit.cloudPlusProductIdentifier !== '';
    },
    anyUsersSelected() {
      return this.multiUserLicenceEdit.users.length > 0;
    },
    anyRolesSelected() {
      return this.multiUserLicenceEdit.userRoles.length > 0;
    },
    anyUnfederatedUsersSelected() {
      return this.unfederatedUsers.length > 0;
    },
    ...mapGetters({
      company: 'office365/company',
      companyId: 'userAuth/companyId',
      domainUsers: 'office365/domainUsers',
      multiUserLicenceEdit: 'office365/multiUserLicenceEdit',
      allSelectedUsers: 'office365/allSelectedLicenseUsers',
    }),
  },
  methods: {
    ...mapMutations({
      setResendTxtRecordsDomain: 'office365/SET_RESEND_TXT_RECORDS_DOMAIN',
      setSelectedDomain: 'office365/SET_SELECTED_DOMAIN',
      setUserLicenceProductIdentifier: 'office365/SET_MULTI_USER_LICENCE_PRODUCT_IDENTIFIER',
      setUserLicenceRoles: 'office365/SET_MULTI_USER_LICENCE_ROLES',
      setMultiUserLicenceUsers: 'office365/SET_MULTI_USER_LICENCE_USERS',
      resetMultiUserLicence: 'office365/RESET_MULTI_USER_LICENCE',
      updateUserPassword: 'office365/UPDATE_MULTI_USER_LICENSE_PASSWORD',
    }),
    ...mapActions({
      getCompanyOffice365Domains: 'office365/getCompanyOffice365Domains',
      addAdditionalDomain: 'office365/addAdditionalDomain',
      getDomainUsers: 'office365/getDomainUsers',
      editMultiUser: 'office365/editMultiUser',
      federateOffice365Domain: 'office365/federateDomain',
    }),
    // modals
    closeTxtRecordsModal() {
      this.showTxtRecordsModal = false;
    },
    openTxtRecordsModal(domain) {
      this.setResendTxtRecordsDomain(domain);
      this.showTxtRecordsModal = true;
    },
    closeFederateModal() {
      this.showConfirmFederateModal = false;
    },
    startFederationProcess() {
      this.showConfirmFederateModal = true;
    },
    closeUnfederatedUsersModal() {
      this.showUnfederatedUsersModal = false;
    },
    openSummaryModal() {
      this.showSummaryModal = true;
    },
    closeSummaryModal() {
      this.showSummaryModal = false;
    },
    moveToTab() {
      this.showConfirmChangesModal = false;
      this.resetMultiUserLicence();
    },
    stayOnTab() {
      this.showConfirmChangesModal = false;
    },
    // calls to services
    verifyDomain(domain, key) {
      office365UtilitiesService.verifyDomain(domain).then(() => {
        this.domains[key].verificationInProgress = true;
      });
    },
    configureDomain(domain, key) {
      this.setSelectedDomain(domain);
      this.addAdditionalDomain().then(() => {
        this.domains[key].configurationInProgress = true;
      });
    },
    federateDomain() {
      this.federatingDomain = true;
      this.federateOffice365Domain(this.domains[this.requestedTab].name).then(response => {
        if (response.data.result.isDomainFederated) {
          this.getCompanyOffice365Domains(this.companyId).then(() => {
            this.domains = this.company.domains;
          });
          this.sucessToaster({
            text: 'Your domain is now federated',
            icon: 'briefcase',
            duration: 5000,
          });
        } else {
          this.sucessToaster({
            text: 'There was an error federating your domain. Please try again later or contact support.',
            icon: 'briefcase',
            duration: 5000,
          });
        }
      }).finally(() => {
        this.closeFederateModal();
        this.federatingDomain = false;
      });
    },
    editMultiUserSuccesfully() {
      this.editMultiUser().then(() => {
        this.sucessToaster(this.getToasterOptions());
        this.resetMultiUserLicence();
        this.$router.push({ path: '/users' });
      });
    },
    getUsers(domainIndex) {
      if (this.domainUsers(domainIndex).results === undefined) {
        this.usersLoading = true;
        const selectedDomain = this.domains[domainIndex].name;
        this.getDomainUsers({
          domain: selectedDomain,
          index: domainIndex,
          pageNumber: 1,
          pageSize: this.pageSize,
          orderBy: 'DisplayName',
          order: 'asc',
          searchTerm: this.search,
        }).then(() => {
          this.usersLoading = false;
        });
      }
    },
    // UI interactions
    saveMultiUserChanges() {
      this.closeSummaryModal();
      if (this.domains[this.requestedTab].isFederated) {
        this.editMultiUserSuccesfully();
      } else {
        this.getSelectedUnfederatedUsers();
        if (this.anyUnfederatedUsersSelected) {
          this.showUnfederatedUsersModal = true;
        } else {
          this.editMultiUserSuccesfully();
        }
      }
    },
    tabSelected(index) {
      this.requestedTab = index;
      if (this.unsavedChangesPresent) {
        this.showConfirmChangesModal = true;
      } else if (this.domains[index].status === this.domainStatus.verified) {
        this.getUsers(index);
      }
    },
    getSelectedUnfederatedUsers() {
      this.unfederatedUsers = this.allSelectedUsers
        .filter(user => !user.isProvisioned)
        .map(user =>
          Object.assign({}, user, { password: this.generatePassword(user.userPrincipalName) }));
    },
    generatePassword(username) {
      const password = PasswordGenerator.generate({
        length: 8,
        numbers: true,
        uppercase: true,
        strict: true,
      });
      this.changeUserPassword(password, username);
      return password;
    },
    changeUserPassword(newPassword, userPrincipalName) {
      this.updateUserPassword({
        password: newPassword,
        username: userPrincipalName,
      });
    },
    getDomainIcon(domain) {
      return domain.status === this.domainStatus.verified ? 'fa fa-check' : 'fa fa-exclamation-triangle';
    },
    getToasterOptions() {
      return {
        text: 'Your changes will be applied shortly. You will be redirected to the user list where you can check for more details about the progress of your changes',
        icon: 'briefcase',
        duration: 10000,
      };
    },
    refreshPage() {
      window.location.reload();
    },
  },
  mounted() {
    this.isLoading = true;
    this.getCompanyOffice365Domains(this.companyId).then(() => {
      this.domains = this.company.domains;
      this.isLoading = false;
    });
  },
};
</script>

<style lang="scss" scoped>
  .verificiation-warning {
    font-size: $secondary-font-size;
    padding-top: 1rem;
  }

  .fa {
    margin-right: 0.75rem;
  }

  .product-and-role-assignment {
    border: 1px solid $border-color;
  }

  .in-text-action {
    cursor: pointer;
    text-decoration: underline;
  }

  .verification-success {
    padding-bottom: 2rem;
  }

  .search-field {
    width: 20rem;
  }
  .roles-and-assignment{
    padding: 1.7rem 1rem 0rem 2rem;
  }

  .no-users-message {
    text-align: center;
  }
</style>
