<template>
  <div>
    <component-sticky-header title="User Details">
      <brand-secondary-btn v-if="!isLoading" @click="openDeleteUserModal" :disabled="updatingUser || !deleteEnabled()">Delete User</brand-secondary-btn>
      <brand-primary-btn v-if="!isLoading" @click="edit" :disabled="updatingUser">Save user profile</brand-primary-btn>
    </component-sticky-header>
    <div class="component-main">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <brand-tabs v-else>
          <tabs
            :unsavedChangesPresent="unsavedUserChangesPresent"
            :requestedTab ="requestedTab"
            @tabSelected="tabSelected">
            <tab-pane label="Personal">
              <div class="columns main-form">
                <div class="column column-padding-right">
                  <brand-section-title>Primary Contact Information</brand-section-title>
                  <user-personal-information></user-personal-information>

                  <brand-section-title>General Information</brand-section-title>
                  <user-general-information :disableEmail="true" :domains="companyDomains"></user-general-information>

                  <brand-section-title>Security Information</brand-section-title>
                  <brand-primary-btn @click="openChangeUserPasswordModal()">Change Password</brand-primary-btn>

                  <brand-section-title>Account Options</brand-section-title>
                  <user-account-options></user-account-options>
                </div>
                <div class="column column-padding-left">
                  <brand-section-title>User Profile Image</brand-section-title>
                  <user-profile-image :enableCropping="true"></user-profile-image>

                  <brand-section-title v-if="roleEditEnabled(user)">Roles</brand-section-title>
                  <user-roles v-if="roleEditEnabled(user)"></user-roles>
                </div>
              </div>
            </tab-pane>
            <tab-pane label="Services">
              <div class="columns">
                <div class="column is-4 user-info">
                  <user-info></user-info>
                </div>
                <div class="column is-8 services">
                  <services v-if="user.displayName" @serviceStatusChanged="checkStatus" @openModal="openProductAssignmentModal"></services>
                </div>
              </div>
            </tab-pane>
          </tabs>
        </brand-tabs>
      </div>
    </div>
    <change-password-modal v-if="showPasswordModal" :showModal="showPasswordModal" :userId="Number(id)" @closeModal="closeChangeUserPasswordModal"></change-password-modal>
    <product-assignment-modal v-if="showProductAssignmentModal" :showModal="showProductAssignmentModal" @closeModal="closeProductAssignmentModal"></product-assignment-modal>
    <confirmation-modal
      v-if="showDeleteConfirmationModal" :showModal="showDeleteConfirmationModal"
      @cancel="closeDeleteUserModal()"
      @confirm = "deleteUser()"
      confirmText="Delete">
        <p>Are you sure you want to delete this user?</p>
        <br/>
        <p><span class="has-text-weight-bold">NOTE:</span> All services assigned to the user will be deprovisioned. Service related data will be permanently lost.</p>
    </confirmation-modal>
    <confirmation-modal
      v-if="showConfirmChangesModal" :showModal="showConfirmChangesModal"
      @cancel="stayOnTab"
      @confirm="moveToTab"
      title = "Confirm Changes"
      cancelText="No"
      confirmText="Yes">You have unsaved changes for this user. Are you sure you want to move to the services tab without saving them?
    </confirmation-modal>
  </div>
</template>

<script>
import { Tabs, TabPane } from '@/components/shared/tabs';
import { mapActions, mapMutations, mapGetters } from 'vuex';
import { userServiceStatus } from '@/assets/constants/commonConstants';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import appConfig from 'appConfig';
import userService from '@/services/userService';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import accountTypesConstants from '@/assets/constants/accountTypes';
import UserPersonalInformation from '../forms/UserPersonalInformation';
import UserGeneralInformation from '../forms/UserGeneralInformation';
import UserAccountOptions from '../forms/UserAccountOptions';
import UserProfileImage from '../forms/UserProfileImage';
import UserRoles from '../forms/UserRoles';
import ChangePasswordModal from '../ChangePasswordModal';
import UserInfo from './UserInfo';
import Services from './Services';
import ProductAssignmentModal from './ProductAssignmentModal';

export default {
  props: ['id'],
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin, loadingMixin],
  components: {
    ComponentStickyHeader,
    Tabs,
    TabPane,
    UserPersonalInformation,
    UserGeneralInformation,
    UserAccountOptions,
    UserProfileImage,
    UserRoles,
    ChangePasswordModal,
    UserInfo,
    Services,
    ProductAssignmentModal,
    ConfirmationModal,
  },
  data() {
    return {
      updatingUser: false,
      serviceUpdateInProgress: false,
      showPasswordModal: false,
      showProductAssignmentModal: false,
      showDeleteConfirmationModal: false,
      showConfirmChangesModal: false,
      accountTypes: accountTypesConstants,
      requestedTab: -1,
      serviceStatus: {
        inProgress: userServiceStatus.InProgress,
      },
    };
  },
  created() {
    this.setUserCompany(this.companyId);
  },
  computed: {
    ...mapGetters({
      companyDomains: 'company/companyDomains',
      companyId: 'userAuth/companyId',
      companyUsers: 'company/companyUsers',
      company: 'company/generalInformation',
      user: 'user/basicUserInfo',
      userProfile: 'userAuth/userProfile',
      loggedUserProfileRole: 'userAuth/userProfileRole',
      unsavedUserChangesPresent: 'user/unsavedUserChangesPresent',
      userServicesTimeout: 'user/userServicesTimeout',
    }),
  },
  methods: {
    ...mapActions({
      updateUser: 'user/updateUser',
      getUser: 'user/getUser',
      getCompanyDomains: 'company/getCompanyDomains',
    }),
    ...mapMutations({
      setUserCompany: 'user/SET_USER_COMPANY',
      resetUserState: 'user/RESET_USER_STATE',
      resetUserServiceState: 'user/RESET_USER_SERVICE_STATE',
      resetUserLicenceState: 'office365/RESET_USER_LICENCE_STATE',
      resetCompanyState: 'company/RESET_COMPANY_STATE',
      resetUnsavedUserChanges: 'user/RESET_UNSAVED_USER_CHANGES',
    }),
    edit() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.updatingUser = true;
          this.updateUser().then(() => {
            this.successToasterMessage('Your user will be updated shortly. Redirecting to User List.');
            this.requestedTab = 0;
            this.resetUnsavedUserChanges();
          });
        } else {
          this.$bus.emit('changeFocus');
          this.$bus.emit('fieldFocus');
        }
      });
    },
    openDeleteUserModal() {
      this.showDeleteConfirmationModal = true;
    },
    closeDeleteUserModal() {
      this.showDeleteConfirmationModal = false;
    },
    deleteUser() {
      userService.deleteUser(this.user.id).then(() => {
        this.showDeleteConfirmationModal = false;
        this.updatingUser = true;
        this.successToasterMessage('Your user will be deleted shortly. Redirecting to User List.');
      });
    },
    openChangeUserPasswordModal() {
      this.showPasswordModal = true;
    },
    closeChangeUserPasswordModal() {
      this.showPasswordModal = false;
    },
    openProductAssignmentModal() {
      this.showProductAssignmentModal = true;
    },
    closeProductAssignmentModal() {
      this.showProductAssignmentModal = false;
    },
    roleEditEnabled(user) {
      if (Number(this.userProfile.id) === user.id) return false;

      switch (user.userRole) {
        case appConfig.masterRole:
          return this.loggedUserProfileRole.role === appConfig.masterRole;
        case appConfig.resellerRole:
          return this.loggedUserProfileRole.role === appConfig.masterRole
          || this.loggedUserProfileRole.role === appConfig.resellerRole;
        case appConfig.customerRole:
          return this.loggedUserProfileRole.role === appConfig.masterRole
          || this.loggedUserProfileRole.role === appConfig.resellerRole
          || this.loggedUserProfileRole.role === appConfig.customerRole;
        default:
          return true;
      }
    },
    successToasterMessage(message) {
      this.sucessToaster({
        text: message,
        icon: 'user',
        onComplete: () => {
          this.updatingUser = false;
          this.$router.push({ path: '/users' });
        },
      });
    },
    deleteEnabled() {
      if (!(Number(this.userProfile.id) === this.user.id)
      && this.roleEditEnabled(this.user)
      && !this.serviceUpdateInProgress) {
        return true;
      }
      return false;
    },
    checkStatus(status) {
      this.serviceUpdateInProgress = status === this.serviceStatus.inProgress;
    },
    tabSelected(index) {
      this.requestedTab = index;
      if (this.unsavedUserChangesPresent) {
        this.showConfirmChangesModal = true;
      }
    },
    moveToTab() {
      this.showConfirmChangesModal = false;
      this.resetUnsavedUserChanges();
      this.getUser(this.id);
    },
    stayOnTab() {
      this.showConfirmChangesModal = false;
    },
  },
  mounted() {
    this.getUser(this.id).then(() => {
      this.isLoading = false;
    });
    this.getCompanyDomains(this.companyId);
  },
  beforeRouteLeave(to, from, next) {
    this.$bus.off();
    clearTimeout(this.userServicesTimeout);
    this.resetUserState();
    this.resetUnsavedUserChanges();
    this.resetUserServiceState();
    this.resetUserLicenceState();
    this.resetCompanyState();
    next();
  },
};
</script>

<style scoped lang='scss'>
.upload-avatar-container {
  //In order for User Profile Image section to be aligned with Personal Information section
  height: 10.625rem;
}
.main-form {
  margin-top: -3rem;
}
.user-info {
  min-width: 350px;
}
</style>
