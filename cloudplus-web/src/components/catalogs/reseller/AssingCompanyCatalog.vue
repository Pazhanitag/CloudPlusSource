<template>
  <div style="position: relative"  v-on-clickaway="closeShowCompanies">
    <div class="select-container" @click="toggleShowCompanies">
      <div class="tag-container">
        <brand-background-with-text-color class="select-option" v-for="(company, index) in companiesAssignedToCatalog" v-bind:key="index">{{company.companyName}}</brand-background-with-text-color>
      </div>
      <div class="show-more" v-show="companiesAssignedToCatalog.length > 3">
        <span>...</span>
      </div>
      <i class="fa fa-angle-down" aria-hidden="true" style="display: flex"></i>
    </div>
    <div class="companies-container" v-show="showCompanies">
      <div class="search-container">
        <brand-input>
          <input type="text" class="input" placeholder="Search..." v-model="search">
        </brand-input>
      </div>
      <div class="companies">
        <table>
          <tr v-for="(company, index) in filteredCompanies" v-bind:key="index" @click="addRemoveCompany(company.companyId)">
            <td class="selected-company">
              <cloud-plus-check-box class="company-checkbox" :value="isCompanyInCurrentCatalog(company.companyId)"></cloud-plus-check-box>
            </td>
            <td>
              <div class="company-name has-text-weight-semibold">{{company.companyName}}</div>
              <div class="company-catalog-name has-text-weight-light">{{company.catalogName}}</div>
            </td>
            <td class="company-type">
              <span class="reseller-tag tag is-success">Reseller</span>
            </td>
          </tr>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import Popper from '@/components/shared/popper/Popper';
import { mixin as clickaway } from 'vue-clickaway';

export default {
  components: { Popper },
  mixins: [clickaway],
  data() {
    return {
      showCompanies: false,
      search: '',
    };
  },
  computed: {
    ...mapGetters({
      catalog: 'catalog/resellerCatalog',
      myCompaniesAssignedCatalogs: 'catalog/companiesAssignedCatalogs',
    }),
    filteredCompanies() {
      if (!this.myCompaniesAssignedCatalogs) {
        return [];
      }
      return this.myCompaniesAssignedCatalogs.filter(company =>
        company.companyName.toLowerCase().includes(this.search.toLowerCase()));
    },
    companiesAssignedToCatalog() {
      if (!this.catalog.companiesAssignedToCatalog) {
        return [];
      }
      return this.catalog.companiesAssignedToCatalog;
    },
  },
  methods: {
    ...mapMutations({
      addCompanyToCatalog: 'catalog/ADD_COMPANY_TO_CATALOG',
      removeCompanyFromCatalog: 'catalog/REMOVE_COMPANY_FROM_CATALOG',
    }),
    toggleShowCompanies() {
      this.showCompanies = !this.showCompanies;
    },
    closeShowCompanies() {
      this.showCompanies = false;
    },
    isCompanyInCurrentCatalog(companyId) {
      if (!this.catalog.companiesAssignedToCatalog) {
        return false;
      }
      return this.catalog.companiesAssignedToCatalog.filter(company =>
        company.companyId === companyId).length > 0;
    },
    addRemoveCompany(companyId) {
      if (this.isCompanyInCurrentCatalog(companyId)) {
        this.removeCompanyFromCatalog(companyId);
      } else {
        this.addCompanyToCatalog(companyId);
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.select-container {
  height: 2.75rem;
  width: 100%;
  border-radius: 0.3125rem;
  border: 1px solid transparent;
  -webkit-box-shadow: inset 0 1px 2px rgba(10, 10, 10, 0.1);
  box-shadow: inset 0 1px 2px rgba(10, 10, 10, 0.1);
  cursor: pointer;
  border-color: #b5b5b57d;
}
.select-container .fa.fa-angle-down {
  float: right;
  position: relative;
  right: 1rem;
  bottom: -0.5rem;
  font-size: 1.5rem;
  color: #b5b5b5;
}
.tag-container {
  float: left;
  max-width: 77%;
  height: 2.75rem;
  overflow: hidden;
}
.select-option {
  margin: 0.3rem 0rem 0.3rem 0.3rem;
  border-radius: 4px;
  padding: 0.3rem 0;
  font-size: 0.75rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  align-items: center;
  display: inline-block;
  padding: 0.45rem 1rem;
}
.companies-container {
  border-radius: 0.3125rem;
  border: 1px solid #d6d6d6;
  position: absolute;
  width: 100%;
  background-color: white;
  z-index: 1;
  box-shadow: 7px 5px 12px -6px rgba(0,0,0,0.2);
  padding: 1.5rem;
}
.companies-container input {
  margin-bottom: 1rem;
  font-size: 0.875rem;
}

.companies {
  max-height: 12.8rem;
  overflow-y: auto;
  margin-top: 0.5rem;
}
.companies table {
  width: 100%;
}
.companies table tr {
  cursor: pointer;
}
.companies table tr:hover {
  background-color: #f9f9f9;
}
.companies table tr td {
  border-bottom: 1px solid rgba(0, 0, 0, 0.1);
  vertical-align: middle;
  padding: 0.5rem;
}
.companies table tr td.company-type {
  text-align: center;
  padding-right: 0.5rem;
}
.companies table tr td.company-type,
.companies table tr td.selected-company {
  width: 1rem;
}
.show-more {
  display: flex;
  justify-content:center;
  align-content:center;
  flex-direction:column;height: 100%;
  float: left;
  margin-left: 0.5rem;
}
.popper {
  background-color: white;
  padding: 1.2rem;
  min-width: 6rem;
}

.company-name {
  font-size: 0.8rem;
}

.company-catalog-name {
  font-size: 0.75rem;
  color: $subtitle-color;
}

.company-checkbox .fa {
  font-size: 1rem;
}

.reseller-tag {
  margin-bottom: 3px;
}
.companies table tr:last-child td{
  border-bottom: transparent;
}

</style>
