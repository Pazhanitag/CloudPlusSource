<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal">
      <p slot="header">Assign Products to User</p>
      <section class="product-assignment" slot="section">
        <product-overview v-if="product" :product="product" :showDescription="false"></product-overview>
        <office365-product-and-roles-assignment 
          :assignedRoles="assignedRoles"
          :assignedLicence="assignedLicence"
          @roleSelected="setUserLicenceRoles" 
          @productSelected = "setUserLicenceProductIdentifier"
          @passwordConfirmed = "approvePassword"
          @invalidPassword = "disableProcess"
          :assignedRolesLoaded = "rolesLoaded"
          :assignedLicenceLoaded = "licenceLoaded"
          :showCredentials="!isDomainFederated">
        </office365-product-and-roles-assignment>
      </section>
      <div slot="footer">
        <brand-primary-btn :disabled="noPendingChanges" @click="processUserLicenceEdit">Process</brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
  
</template>

<script>
import { areStringArraysEqual } from '@/helpers/utils';
import { mapGetters, mapActions, mapMutations } from 'vuex';
import { userServiceStatus } from '@/assets/constants/commonConstants';
import office365CustomersService from '@/services/office365CustomersService';
import toasterMixin from '@/mixins/toaster';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import ProductOverview from '@/components/catalogs/customer/products/ProductOverview';
import Office365ProductAndRolesAssignment from '@/components/services/office365/Office365ProductAndRolesAssignment';

export default {
  mixins: [toasterMixin],
  props: {
    showModal: {
      type: Boolean,
      default: false,
    },
  },
  components: {
    CloudPlusCardModal,
    ProductOverview,
    Office365ProductAndRolesAssignment,
  },
  data() {
    return {
      userServiceStatuses: {
        assigned: userServiceStatus.Assigned,
        available: userServiceStatus.Available,
      },
      assignedLicence: '',
      assignedRoles: [],
      rolesLoaded: true,
      licenceLoaded: true,
      isDomainFederated: true,
      passwordInvalid: true,
    };
  },
  computed: {
    ...mapGetters({
      product: 'product/selectedProduct',
      userLicenceAssignment: 'office365/userLicenceAssignment',
      userService: 'user/userServices',
      appliedUserLicenceChanges: 'office365/changeUserLicence',
    }),
    noPendingChanges() {
      if (this.userService.status === this.userServiceStatuses.assigned) {
        const isLicenceEdited = this.appliedUserLicenceChanges.removeCloudPlusProductIdentifier
        !== this.appliedUserLicenceChanges.assignCloudPlusProductIdentifier;

        const areRolesUnchanged =
        areStringArraysEqual(this.assignedRoles, this.appliedUserLicenceChanges.userRoles);

        return !isLicenceEdited && areRolesUnchanged;
      }
      if (!this.isDomainFederated
        && this.userService.status === this.userServiceStatuses.available) {
        const noProductOrRoleSelected = this.userLicenceAssignment.cloudPlusProductIdentifier === '' && this.userLicenceAssignment.userRoles.length === 0;
        return noProductOrRoleSelected || this.passwordInvalid;
      }
      return this.userLicenceAssignment.cloudPlusProductIdentifier === '' && this.userLicenceAssignment.userRoles.length === 0;
    },
  },
  methods: {
    ...mapActions({
      getProduct: 'product/getProduct',
      assignOffice365ToUser: 'office365/assignOffice365ToUser',
      getAssignedLicences: 'office365/getAssignedLicences',
      getAssignedRoles: 'office365/getAssignedRoles',
      changeLicence: 'office365/changeLicence',
    }),
    ...mapMutations({
      setUserLicenceProductIdentifier: 'office365/SET_USER_LICENCE_PRODUCT_IDENTIFIER',
      setUserLicenceRoles: 'office365/SET_USER_LICENCE_ROLES',
      setInitiallyAssignedProductIdentifier: 'office365/SET_INITIALLY_ASSIGNED_PRODUCT_IDENTIFIER',
      setUserLicensePassword: 'office365/SET_USER_LICENSE_PASSWORD',
      setUserServiceStatusToInProgress: 'user/SET_USER_SERVICE_STATUS_TO_IN_PROGRESS',
      resetUserLicenceState: 'office365/RESET_USER_LICENCE_STATE',
    }),
    closeModal() {
      this.$emit('closeModal');
      this.resetUserLicenceState();
    },
    processUserLicenceEdit() {
      if (this.userService.status === this.userServiceStatuses.assigned) {
        this.changeLicence().then(() => {
          this.onUserLicenceEditComplete();
        });
      } else {
        this.assignOffice365ToUser().then(() => {
          this.onUserLicenceEditComplete();
        });
      }
    },
    onUserLicenceEditComplete() {
      this.closeModal();
      this.setUserServiceStatusToInProgress();
      this.sucessToaster(this.getToasterOptions());
    },
    disableProcess() {
      this.passwordInvalid = true;
    },
    approvePassword(password) {
      this.passwordInvalid = false;
      this.setUserLicensePassword(password);
    },
    getToasterOptions() {
      return {
        text: 'The requested changes will be applied shortly. It might take up to 5 minutes.',
        duration: 5000,
      };
    },
  },
  created() {
    this.getProduct();
    if (this.userService.status === this.userServiceStatuses.assigned) {
      this.rolesLoaded = false;
      this.licenceLoaded = false;
      this.getAssignedLicences().then(response => {
        const licenseResponse = response.data.result;
        if (licenseResponse !== undefined) {
          this.assignedLicence = response.data.result.office365Offer.cloudPlusProductIdentifier;
          this.setInitiallyAssignedProductIdentifier(this.assignedLicence);
        } else {
          this.setInitiallyAssignedProductIdentifier('');
        }
        this.licenceLoaded = true;
      });
      this.getAssignedRoles().then(response => {
        this.assignedRoles = response.data.result.userRoles;
        this.setUserLicenceRoles(response.data.result.userRoles);
        this.rolesLoaded = true;
      });
    }
    if (this.userService.status === this.userServiceStatuses.available) {
      const domain = this.userLicenceAssignment.userPrincipalName.split('@')[1];
      office365CustomersService.isDomainFederated(domain).then(response => {
        this.isDomainFederated = response.data.result;
      });
    }
  },
};
</script>

<style lang="scss" scoped>
</style>
