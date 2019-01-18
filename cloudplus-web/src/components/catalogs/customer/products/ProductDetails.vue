<template>
  <div class="content-wrapper" v-if="!loading">
    <component-sticky-header title="Product Details"></component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <div class="product-details">
          <product-overview @availibilityChanged="toggleAvailability" hasSwitch :product="product" :productAvailableSwitch="productAvailableSwitch"></product-overview>
        </div>
      </div>
      <div class="available-products-message">
        After enabling this product for assignment following products will be available:
      </div>
    </div>
     <product-items class="component-min-width" :product="product"></product-items>
    <confirmation-modal
      v-if="showModal" :showModal="showModal"
      @cancel="closeModal"
      :cancelText="'Cancel'"
      :title="'Would you like to configure Office 365?'"
      @confirm = "confgureProduct"
      confirmText="Let’s do this!">You are about to activate Office 365 service for your organization. Before it will be available, there are a few things needed.
    </confirmation-modal>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import CloudPlusQuantityBox from '@/components/shared/input-components/CloudPlusNumericStepper';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import provisioningService from '@/services/provisioningService';
import ProductItems from './ProductItems';
import ProductOverview from './ProductOverview';

export default {
  props: ['productId'],
  inject: {
    $validator: '$validator',
  },
  components: {
    ComponentStickyHeader,
    CloudPlusQuantityBox,
    ConfirmationModal,
    ProductItems,
    ProductOverview,
  },
  data() {
    return {
      search: '',
      productAvailable: false,
      showModal: false,
      loading: true,
      productAvailableSwitch: false,
    };
  },
  mounted() {
    this.getCustomerProducts().then(() => {
      this.loading = false;
    });
  },
  computed: {
    ...mapGetters({
      customerProducts: 'catalog/customerProducts',
      companyId: 'userAuth/companyId',
    }),
    product() {
      if (!this.customerProducts.find) {
        return [];
      }
      // eslint-disable-next-line
      return this.customerProducts.find(product => product.id == this.productId);
    },
  },
  methods: {
    ...mapActions({
      getCustomerProducts: 'catalog/getCustomerProducts',
    }),
    formatPrice(price) {
      return parseFloat(Math.round(price * 100) / 100).toFixed(2);
    },
    toggleAvailability(productAvailable) {
      this.productAvailable = productAvailable;
      if (this.productAvailable) {
        this.showModal = true;
      }
    },
    closeModal() {
      this.showModal = false;
      this.productAvailableSwitch = !this.productAvailableSwitch;
    },
    confgureProduct() {
      provisioningService.setProductAvailability(
        this.companyId,
        this.product.id,
        this.productAvailable,
      )
        .then(() => {
          this.$router.push({
            name: 'myServices',
            params: { showModalOnOpen: true },
          });
        });
    },
  },
};
</script>

<style scoped lang='scss'>
a {
  border-bottom-color: #2d8ac2 !important;
  color: #4a4a4a !important;
}

a.is-unselectable {
  border-bottom-color: #2d8ac2 !important;
  color: #4a4a4a !important;
}

.available-products-message {
  padding-top: 1.75rem;
  padding-left: 1.75rem;
  font-size: 0.875rem;
}
.component-min-width{
  min-width: 40rem;
}
</style>

