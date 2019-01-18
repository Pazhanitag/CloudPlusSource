<template>
  <div>
    <component-sticky-header title="Product Transition"></component-sticky-header>
      <div class="component-main">
        <div class="component-main__white">
          <product-overview v-if="product" :product="product"></product-overview>
          <div class="transition-buttons">
            <brand-primary-btn :disabled="isLoading" @click="continueTransition" class="transition-button">Continue Transition</brand-primary-btn>
            <brand-reverse-primary-btn @click="cancelTransition" class="transition-button">Cancel Transition</brand-reverse-primary-btn>
          </div>
          <div v-if="!domainAuthorized && !isLoading" class="warning-message">
            <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> 
            Your domain is not authorized. You need to authorize the transfer to proceed. 
            <span @click="goToMicrosoftPortal" class="in-text-action">Click here</span> to be redirected to Microsoft Control Panel.
          </div>
          <div v-show="isLoading" class="warning-message__loading">
            <loading-icon :inline="true" size="1"></loading-icon> 
            Reviewing transition status. Please wait!
          </div>
        </div>
      </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import { provisioningStatus } from '@/assets/constants/commonConstants';
import loadingMixin from '@/mixins/loading';
import office365CustomersService from '@/services/office365CustomersService';
import provisioningService from '@/services/provisioningService';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import ProductOverview from '@/components/catalogs/customer/products/ProductOverview';

export default {
  mixins: [loadingMixin],
  data() {
    return {
      domainAuthorized: true,
      provisioningStatus: {
        inTransition: provisioningStatus.InTransition,
      },
    };
  },
  components: {
    ComponentStickyHeader,
    ProductOverview,
  },
  computed: {
    ...mapGetters({
      product: 'product/selectedProduct',
      companyId: 'userAuth/companyId',
    }),
  },
  methods: {
    ...mapActions({
      getProduct: 'product/getProduct',
      setProvisioningStatusToActive: 'office365/setProvisioningStatusToActive',
    }),
    getProductAvailability() {
      provisioningService.getProductAvailability(this.companyId, this.product.id)
        .then(response => {
          const provisioningStatusResponse = response.data.result;
          if (provisioningStatusResponse !== this.provisioningStatus.inTransition) {
            this.$router.push({
              path: '/my-services',
            });
          }
          this.isLoading = false;
        });
    },
    continueTransition() {
      this.isLoading = true;
      office365CustomersService.checkAuthorization().then(response => {
        this.isLoading = false;
        this.domainAuthorized = response.data.result;
        if (this.domainAuthorized) {
          this.$router.push({
            path: '/transition-user-licenses',
          });
        }
      });
    },
    cancelTransition() {
      this.setProvisioningStatusToActive();
      this.$router.push({
        path: '/my-services',
      });
    },
    goToMicrosoftPortal() {
      window.open('https://portal.office.com');
    },
  },
  created() {
    if (this.product === null) {
      this.getProduct().then(() => {
        this.getProductAvailability();
      });
    } else {
      this.getProductAvailability();
    }
  },
};
</script>

<style lang="scss" scoped>
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
