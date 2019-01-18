<template>
  <div>
    <div v-if="provisioningStatusClone !== statusNotEnabled">
      <loading-icon size="2" v-if="isLoading"></loading-icon>
      <div v-else>
        <product-overview v-if="product && provisioningStatusClone !== statusCompleted" :product="product" :showDescription="provisioningStatusClone !== statusCompleted" :showProductLogo="false" :showOverviewNameAndVendor="false"></product-overview>
        <div v-if="showProductTransition">
          <div class="transition-buttons">
            <brand-primary-btn :disabled="tempLoading" @click="continueTransition()" class="transition-button">Continue Transition</brand-primary-btn>
            <brand-reverse-primary-btn @click="cancelTransition()" class="transition-button">Cancel Transition</brand-reverse-primary-btn>
          </div>
          <div v-if="!domainAuthorized && !tempLoading" class="warning-message">
            <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
            Your domain is not authorized. You need to authorize the transfer to proceed.
            <span @click="goToMicrosoftPortal()" class="in-text-action">Click here</span> to be redirected to Microsoft Control Panel.
          </div>
          <div v-show="tempLoading" class="warning-message__loading">
            <loading-icon :inline="true" size="1"></loading-icon>
            Reviewing transition status. Please wait!
          </div>
        </div>
        <div v-else>
          <div v-if="provisioningStatusClone === statusEnabled">
            <brand-primary-btn class="configuration-button" @click="openConfigureProductModal()">Create New Office 365 Account</brand-primary-btn>
            <brand-primary-btn class="configuration-button" @click="startLicencesTransfer()">Transition Existing Office 365 Account</brand-primary-btn>
            <brand-primary-btn class="configuration-button" @click="deProvisionService()" :disabled="deProvisioning">
              <loading-icon :inline="true" v-show="deProvisioning"></loading-icon>
              Cancel
            </brand-primary-btn>
            <div class="product-configuration-warning">
              <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>Your product is not configured, please Create New Office 365 Account before you can start assigning this product to users.
               If you are already using this product and want to transfer licenses Transition Existing Office 365 Account

            </div>
          </div>
          <div v-if="provisioningStatusClone === statusInProgress">
            <brand-primary-btn :disabled="true" class="configuration-button">Create New Office 365 Account</brand-primary-btn>
            <brand-primary-btn :disabled="true" class="configuration-button">Transition Existing Office 365 Account</brand-primary-btn>
            <brand-primary-btn :disabled="true" class="configuration-button">Cancel</brand-primary-btn>
            <div class="product-configuration-warning">
              <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>It will take a few moments to provision your account on Office 365 Portal
              and to send you domain validations instructions on your company email (TXT records).
              Please <span @click="refreshPage()" class="in-text-action">refresh</span> page or recheck status <span class="in-text-action" @click="getProvisioningStatus">here.</span>
            </div>
          </div>
          <div v-if="provisioningStatusClone === statusCompleted">
            <office365-domains></office365-domains>
          </div>
        </div>
      </div>
    </div>
    <div v-else class="office-365-not-enabled-warning">
      <div class="office-365-not-enabled-warning__title">You have no enabled services for assignment.</div>
      <router-link class="office-365-not-enabled-warning__subtitle" :to="'/catalogs/customer'" >
        <brand-color-primary><span class="link">Please choose desired products and services from The Catalog</span></brand-color-primary>
      </router-link>
    </div>
    <create-office365-customer-modal @provisioningStarted="setProvisioningStatus(statusInProgress)" v-if="showConfigureProductModal" :showModal="showConfigureProductModal" @closeModal="closeConfigureProductModal"></create-office365-customer-modal>
    <confirmation-modal
      v-if="showTransitionModal" :showModal="showTransitionModal"
      @cancel="closeTransitionModal"
      @confirm = "startTransitionProcess"
      confirmText="Continue"
      title="Start Transition">
      <div class="continue-transition">
        By continuing, you are beggining the transition process to manage your users and licenses within this Control Panel.
        When you continue, you will be redirected to the Microsoft Portal within a new browser. Here you can continue your authorization.
      </div>
    </confirmation-modal>
  </div>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from 'vuex';
import { office365ConfigurationStatus, provisioningStatus } from '@/assets/constants/commonConstants';
import office365UtilitiesService from '@/services/office365UtilitiesService';
import provisioningService from '@/services/provisioningService';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import ProductItems from '@/components/catalogs/customer/products/ProductItems';
import ProductOverview from '@/components/catalogs/customer/products/ProductOverview';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import office365CustomersService from '@/services/office365CustomersService';
import CreateOffice365CustomerModal from './CreateOffice365CustomerModal';
import Office365Domains from './Office365Domains';

export default {
  mixins: [loadingMixin],
  props: {
    provisioningStatus: {
      required: true,
    },
    productTransitionStatus: {
      required: true,
    },
  },
  data() {
    return {
      showConfigureProductModal: false,
      showTransitionModal: false,
      deProvisioning: false,
      provisioningStatusConstant: {
        notProvisioned: provisioningStatus.NotProvisioned,
        provisioned: provisioningStatus.Provisioned,
        inTransition: provisioningStatus.InTransition,
      },
      statusNotEnabled: office365ConfigurationStatus.NotEnabled,
      statusEnabled: office365ConfigurationStatus.Enabled,
      statusInProgress: office365ConfigurationStatus.InProgress,
      statusCompleted: office365ConfigurationStatus.Completed,
      showProductTransition: this.productTransitionStatus,
      domainAuthorized: true,
      tempLoading: false,
    };
  },
  components: {
    ComponentStickyHeader,
    CreateOffice365CustomerModal,
    ProductItems,
    ProductOverview,
    Office365Domains,
    ConfirmationModal,
  },
  computed: {
    ...mapGetters({
      product: 'product/selectedProduct',
      products: 'product/allProducts',
      companyId: 'userAuth/companyId',
    }),
    provisioningStatusClone: {
      get() {
        return this.provisioningStatus;
      },
      set(val) {
        this.$emit('setProvisioningStatus', val);
      },
    },
  },
  methods: {
    ...mapActions({
      setCustomerStatusToInTransition: 'office365/setProvisioningStatusToInTransition',
      setProvisioningStatusToActive: 'office365/setProvisioningStatusToActive',
    }),
    ...mapMutations({
      resetMultiUserLicence: 'office365/RESET_MULTI_USER_LICENCE',
      setProducts: 'product/SET_PRODUCTS',
    }),
    closeConfigureProductModal() {
      this.showConfigureProductModal = false;
    },
    openConfigureProductModal() {
      this.showConfigureProductModal = true;
    },
    setStatusIcon(target, id, icon) {
      return target.map(element => {
        if (element.id === id) {
          element.statusIcon = icon;
          return element;
        }
        return element;
      });
    },
    getProvisioningStatus() {
      this.isLoading = true;
      return provisioningService.getProductAvailability(this.companyId, this.product.id)
        .then(response => {
          const provisioningStatusResponse = response.data.result;
          if (provisioningStatusResponse === this.provisioningStatusConstant.inTransition) {
            this.showProductTransition = true;
            this.setProducts(this.setStatusIcon(this.products, this.product.id, 'fa fa-hourglass'));
            this.isLoading = false;
          } else if (provisioningStatusResponse
            === this.provisioningStatusConstant.notProvisioned) {
            this.provisioningStatusClone = office365ConfigurationStatus.NotEnabled;
            this.isLoading = false;
          } else {
            this.isLoading = true;
            office365UtilitiesService.getProvisioningStatus(this.companyId)
              .then(officeServiceResponse => {
                this.provisioningStatusClone = officeServiceResponse.data.result;
                if (officeServiceResponse.data.result === this.statusEnabled) {
                  this.setProducts(this.setStatusIcon(this.products, this.product.id, 'fa fa-exclamation-triangle'));
                } else if (officeServiceResponse.data.result === this.statusCompleted) {
                  this.setProducts(this.setStatusIcon(this.products, this.product.id, 'fa fa-check'));
                } else if (officeServiceResponse.data.result === this.statusInProgress) {
                  this.setProducts(this.setStatusIcon(this.products, this.product.id, 'fa fa-hourglass'));
                }
                this.isLoading = false;
              });
          }
        });
    },
    startLicencesTransfer() {
      this.showTransitionModal = true;
    },
    closeTransitionModal() {
      this.showTransitionModal = false;
    },
    startTransitionProcess() {
      this.isLoading = true;
      this.goToMicrosoftPortal();
      this.closeTransitionModal();
      this.setCustomerStatusToInTransition().then(() => {
        this.getProvisioningStatus().then(() => {
          this.isLoading = false;
        });
      });
    },
    deProvisionService() {
      this.deProvisioning = true;
      provisioningService.setProductAvailability(
        this.companyId,
        this.product.id,
      ).then(() => {
        this.provisioningStatusClone = this.statusNotEnabled;
        this.deProvisioning = false;
      }).catch(() => {
        this.deProvisioning = false;
      });
    },
    refreshPage() {
      window.location.reload();
    },
    continueTransition() {
      this.tempLoading = true;
      office365CustomersService.checkAuthorization().then(response => {
        this.tempLoading = false;
        this.domainAuthorized = response.data.result;
        if (this.domainAuthorized) {
          this.$router.push({
            path: '/transition-user-licenses',
          });
        }
      });
    },
    cancelTransition() {
      this.isLoading = true;
      this.showProductTransition = false;
      this.setProvisioningStatusToActive().then(() => {
        this.getProvisioningStatus().then(() => {
          this.isLoading = false;
        });
      });
    },
    goToMicrosoftPortal() {
      window.open('https://portal.office.com');
    },
  },
  mounted() {
    this.isLoading = false;
  },
  beforeRouteLeave(to, from, next) {
    this.resetMultiUserLicence();
    next();
  },
};
</script>

<style lang="scss" scoped>
  .office-365-not-enabled-warning{
    padding-top: 18rem;
    text-align: center;
    &__title {
      color: $label-color;
      font-size: $big-font-size;
    }
    &__subtitle {
      color: $subtitle-color;
      font-size: $primary-font-size;
    }
  }

  .link {
    text-decoration: underline;
  }

  .fa-exclamation-triangle {
    margin-right: 0.75rem;
  }

  .in-text-action {
    cursor: pointer;
    text-decoration: underline;
  }

  .continue-transition {
    font-size: 0.875rem;
  }

  .configuration-button {
    padding: 0 2rem;
  }

  .transition-buttons {
  margin-bottom: 2rem;
  }
  .transition-button{
    min-width: 21rem;
  }
  .warning-message {
    font-size: $secondary-font-size;
    margin-bottom: 2rem;
    &__loading {
      text-align: center;
      font-size: 0.875rem;
      margin: 4rem 0 2rem 0;
    }
  }
  .in-text-action {
    cursor: pointer;
    text-decoration: underline;
  }
</style>
