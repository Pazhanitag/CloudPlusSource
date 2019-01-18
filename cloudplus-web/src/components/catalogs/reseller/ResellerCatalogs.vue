<template>
  <div>
    <component-sticky-header title="Price Schedules" class="component-min-width">
      <brand-primary-btn class="is-pulled-right" @click="openCreateCatalogModal()">New Schedule</brand-primary-btn>
      <cloud-plus-textfield class="is-pulled-right margin-large right" :hasIconRight="true" :icon="'fa-search'" :placeholder="'Search'" v-model="search"></cloud-plus-textfield>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div v-if="!isLoading">
          <div v-if="filteredCatalogs.length > 0">
            <reseller-catalogs-table :catalogs="filteredCatalogs" :showAdditionalCatalogOptions="this.resellerCatalogs.length > 1"></reseller-catalogs-table>
          </div>
          <div v-else class="no-catalog-warning">
            <div class="no-catalog-warning__title">No results match your search criteria.</div>
          </div>
		   </div>
      </div>
    </div>
    <new-catalog-modal v-if="showCreateCatalogModal" :catalogs="resellerCatalogs" :showModal="showCreateCatalogModal" @closeModal="closeModal"> </new-catalog-modal>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import loadingMixin from '@/mixins/loading';
import ResellerCatalogsTable from './ResellerCatalogsTable';
import NewCatalogModal from './NewCatalogModal';

export default {
  components: { ComponentStickyHeader, ResellerCatalogsTable, NewCatalogModal },
  mixins: [loadingMixin],
  data() {
    return {
      showCreateCatalogModal: false,
      search: '',
    };
  },
  computed: {
    ...mapGetters({
      resellerCatalogs: 'catalog/resellerCatalogs',
    }),
    filteredCatalogs() {
      if (this.resellerCatalogs) {
        return this.resellerCatalogs.filter(catalog =>
          catalog.name.toLowerCase().includes(this.search.toLowerCase()) ||
          catalog.companiesAssignedToCatalog.filter(company =>
            company.companyName.toLowerCase().includes(this.search.toLowerCase())).length > 0);
      }
      return [];
    },
  },
  methods: {
    ...mapActions({
      getResellerCatalogs: 'catalog/getResellerCatalogs',
      getResellerCompaniesAssignedCatalogs: 'catalog/getResellerCompaniesAssignedCatalogs',
    }),
    openCreateCatalogModal() {
      this.showCreateCatalogModal = true;
    },
    closeModal() {
      this.showCreateCatalogModal = false;
    },
  },
  mounted() {
    Promise.all([this.getResellerCatalogs(), this.getResellerCompaniesAssignedCatalogs()])
      .finally(() => { this.isLoading = false; });
  },
};
</script>

<style scoped lang="scss">
.component-min-width{
  min-width: 40rem;
}
.no-catalog-warning{
  text-align: center;
  &__title {
    color: $label-color;
    font-size: $big-font-size;
  }
}
@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .component-main__white{
    height: 88rem;
  }
}
</style>
