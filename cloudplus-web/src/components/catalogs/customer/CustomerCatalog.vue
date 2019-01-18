<template>
  <div>
    <component-sticky-header class="component-min-width" title="The Catalog">
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div v-else>
          <customer-catalog-product v-for="product in customerProducts.filter(p => p.categoryId === productTypes.product)" v-bind:key="product.id" :productId="product.id"></customer-catalog-product>
          <customer-catalog-service v-for="service in customerProducts.filter(p => p.categoryId === productTypes.service)" v-bind:key="service.id" :productId="service.id"></customer-catalog-service>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import loadingMixin from '@/mixins/loading';
import { catalogTypeConstants } from '@/assets/constants/commonConstants';
import CustomerCatalogProduct from './CustomerCatalogProduct';
import CustomerCatalogService from './CustomerCatalogService';

export default {
  data() {
    return {
      productTypes: catalogTypeConstants,
    };
  },
  components: {
    ComponentStickyHeader, CustomerCatalogProduct, CustomerCatalogService,
  },
  mixins: [loadingMixin],
  computed: {
    ...mapGetters({
      customerProducts: 'catalog/customerProducts',
    }),
  },
  methods: {
    ...mapActions({
      getCustomerProducts: 'catalog/getCustomerProducts',
    }),
  },
  mounted() {
    this.getCustomerProducts().then(() => {
      this.isLoading = false;
    });
  },
};
</script>

<style scoped>
.component-min-width{
  min-width: 50rem;
}
</style>
