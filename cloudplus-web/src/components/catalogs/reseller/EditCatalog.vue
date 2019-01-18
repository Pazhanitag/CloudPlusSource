<template>
  <div>
    <component-sticky-header title="Price Schedules">
      <brand-primary-btn class="is-pulled-right" @click="saveChanges()" :disabled="saving">
        Save changes
        <loading-icon :inline="true" v-show="saving"></loading-icon>
      </brand-primary-btn>
      <cloud-plus-textfield class="is-pulled-right margin-large right" :hasIconRight="true" :icon="'fa-search'" :placeholder="'Search'" v-model="search"></cloud-plus-textfield>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div v-else>
          <h5 class="title is-5">{{resellerCatalog.name}}</h5>
          <brand-tabs>
            <tabs>
              <tab-pane label="Product Pricing">
                <edit-catalog-product v-for="(product, index) in filteredProducts" v-bind:key="index"
                :productId="product.id"
                :search="search">
                </edit-catalog-product>
              </tab-pane>
              <tab-pane label="General Settings">
                <edit-catalog-general-settings></edit-catalog-general-settings>
              </tab-pane>
            </tabs>
          </brand-tabs>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapActions, mapGetters, mapMutations } from 'vuex';
import { Tabs, TabPane } from '@/components/shared/tabs';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import EditCatalogProduct from './EditCatalogProduct';
import EditCatalogGeneralSettings from './EditCatalogGeneralSettings';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin, loadingMixin],
  components: {
    ComponentStickyHeader,
    Tabs,
    TabPane,
    EditCatalogProduct,
    EditCatalogGeneralSettings,
  },
  props: {
    catalogId: {
      required: true,
    },
  },
  data() {
    return {
      saving: false,
      search: '',
    };
  },
  computed: {
    ...mapGetters({
      resellerCatalog: 'catalog/resellerCatalog',
      updateCatalog: 'catalog/updateCatalog',
    }),
    filteredProducts() {
      if (this.resellerCatalog.products) {
        return this.resellerCatalog.products.filter(product =>
          product.name.toLowerCase().includes(this.search.toLowerCase()) ||
          product.vendor.toLowerCase().includes(this.search.toLowerCase()) ||
          product.productItems.filter(productItem =>
            productItem.name.toLowerCase().includes(this.search.toLowerCase())).length > 0);
      }
      return [];
    },
  },
  methods: {
    ...mapActions({
      getResellerCatalog: 'catalog/getResellerCatalog',
      updateResellerCatalogPrices: 'catalog/updateResellerCatalogPrices',
      getResellerCompaniesAssignedCatalogs: 'catalog/getResellerCompaniesAssignedCatalogs',
    }),
    ...mapMutations({
      resetCatalogState: 'catalog/RESET_CATALOG_STATE',
    }),
    saveChanges() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.saving = true;
          this.updateResellerCatalogPrices({
            catalog: this.updateCatalog,
            catalogId: this.catalogId,
          }).then(() => {
            this.sucessToaster({
              text: 'Price Schedule updated',
              icon: 'usd',
            });
          }).finally(() => {
            this.saving = false;
          });
        } else {
          this.errorToaster({
            text: 'Please make sure that all fields are filled in correctly.',
            icon: 'exclamation-triangle',
          });
        }
      });
    },
  },
  mounted() {
    Promise.all([this.getResellerCatalog(this.catalogId),
      this.getResellerCompaniesAssignedCatalogs()])
      .then(() => {
        this.isLoading = false;
      });
  },
  beforeRouteLeave(to, from, next) {
    this.resetCatalogState();
    next();
  },
};
</script>

<style scoped="true">
.component-min-width{
  min-width: 50rem;
}
@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .component-main__white{
    height: 75rem;
  }
}
</style>
