<template>
  <div>
    <component-sticky-header class="component-min-width" :title="accountType === `${accountTypes.Reseller}` ? 'My Resellers' : 'My Customers'">
      <div class="is-4 is-offset-2">
        <cloud-plus-textfield class="search-component" :hasIconRight="true" :icon="'fa-search'" v-if="companies.length > 0 || searchTerm !== ''" v-model="searchTerm" :placeholder="'Search'" @input="onSearch"></cloud-plus-textfield>
        <brand-primary-btn @click="createAccount">Create new {{accountType === `${accountTypes.Reseller}` ? 'reseller' : 'customer'}}</brand-primary-btn>
      </div>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white"  v-if="isLoading">
        <loading-icon size="2"></loading-icon>
      </div>
      <div v-else>
        <div v-if="companies.length > 0">
          <nav class="level navigation-level" v-if="companies.length">
            <div class="level-left">
              <div class="filtering-label level-item">
                <span class="label is-size-6">Filtering</span>
              </div>
              <div v-for="(item, index) in orderByItems" :key="index">
                <order-by-item :orderValue='item' @sortBy='sortBy' :orderBy='orderBy' :order='order'></order-by-item>
              </div>
              <brand-primary-btn @click="selectedView = selectedView === 1 ? 2 : 1">
                  {{selectedView === view.listView ? 'Card' : 'List'}} View
              </brand-primary-btn>
            </div>
          </nav>
          <!-- CARD VIEW -->
          <div class="columns is-multiline" v-if="selectedView === view.cardView">
            <div class="column is-one-third-desktop is-one-quarter-fullhd is-half-tablet" v-for="(company, key) in companies" :key="key">
                <div class="card" v-if="company">
                  <header class="card-header">
                    <div class="card-header-title">ID: {{company.id}}</div>
                    <popper trigger="click" :appendToBody="true" :options="{placement: 'bottom-start'}">
                      <div class="popper">
                        <div class="dropdown-content">
                          <brand-hover>
                            <span class="dropdown-item" @click="showUsersModal(company.id)">
                              Login as <b>{{company.name}}</b> user
                            </span>
                          </brand-hover>
                          <hr class="dropdown-divider">
                          <brand-hover>
                            <span @click ="editCompany(company.id)" class="dropdown-item">
                              Edit company details
                            </span>
                          </brand-hover>
                        </div>
                      </div>
                      <a slot="reference" class="card-header-icon" aria-label="more options">
                        <span class="icon">
                        <i class="fa fa-ellipsis-v fa-lg" aria-hidden="true"></i>
                        </span>
                      </a>
                    </popper>
                  </header>
                  <div class="card-content">
                    <div class="content">
                      <figure @click="showUsersModal(company.id)">
                        <img :src="company.logoUrl === '' ? defaultLogo : company.logoUrl" alt="Company Logo">
                      </figure>
                      <div class="has-text-centered">
                        <span class="company-name">{{company.name}}</span>
                      </div>

                      <div class="has-text-centered">
                          <span class="company-additional-info">
                            {{getPrimaryDomainName(company.domains)}} <span v-if="company.domains.length > 1"> +</span>
                          </span>
                          <popper trigger="hover" :appendToBody="true" :options="{placement: 'bottom-start'}">
                            <div class="popper">
                              <div class="level-right dropdown is-right is-active">
                                <div class="dropdown-menu">
                                  <div class="dropdown-content" :class="{'domains-overflow' : (company.domains.length > 5)}">
                                    <div v-for="(domain, key) in company.domains" :key="key">
                                      <div class="dropdown-item column-padding-left column-padding-right" v-if="!domain.isPrimary">
                                        <div class="domain-name-padding"> {{domain.name}} </div>
                                        <hr v-if="key < company.domains.length - 1" class="dropdown-divider">
                                      </div>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                            <a slot="reference" class="dropdown-domain-font" v-show="company.domains.length > 1">{{company.domains.length - 1}} more </a>
                          </popper>
                      </div>

                      <div class="has-text-centered">
                        <span class="company-additional-info">{{company.country}}, {{company.state}}, {{company.city}}</span>
                      </div>
                      <div class="company-status">
                        <div class="status-indicator">
                          <i class="fa fa-circle" :class="{'has-text-success': company.status === status.Active, 'has-text-danger': company.status === status.Suspended, }" aria-hidden="true"></i>
                        </div>
                        <div>
                          <span class="company-additional-info">{{company.status === status.Active ? 'Active': 'Suspended'}}</span>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="level company-additional-info"  v-if="company.type == `${accountTypes.Reseller}`">
                    <div class="level-item has-text-centered">
                      <div>
                        <p>{{company.numberOfResellers}}</p>
                        <p v-if="company.numberOfResellers === 1">Reseller</p>
                    <p v-else>Resellers</p>
                      </div>
                    </div>
                    <div class="level-item has-text-centered">
                      <div>
                        <p>{{company.numberOfCustomers}}</p>
                        <p v-if="company.numberOfCustomers === 1">Customer</p>
                    <p v-else>Customers</p>
                      </div>
                    </div>
                  </div>

                  <div class="level"  v-else>
                    <div class="level-item has-text-centered company-additional-info">
                      <div>
                        <p>{{company.numberOfUsers}}</p>
                        <p v-if="company.numberOfUsers === 1">User</p>
                    <p v-else>Users</p>
                      </div>
                    </div>
                  </div>

                </div>
            </div>
          </div>
          <!-- LIST VIEW -->
          <div v-if="selectedView === view.listView">
            <table class="table is-fullwidth">
            <thead>
              <th>Account #</th>
              <th class="company-heading">Company</th>
              <th>Creation Date</th>
              <th>Status</th>
              <th v-if="accountType === `${accountTypes.Customer}`">Users</th>
              <template v-if="accountType === `${accountTypes.Reseller}`">
                <th class="customer-count">Direct Customers</th>
                <th class="customer-count">Direct Resellers</th>
                <th class="customer-count">Total Customers</th>
                <th class="customer-count">Total Resellers</th>
              </template>
              <th></th>
            </thead>
            <tbody>
              <tr v-for="(company, key) in companies" :key="key" @click="showUsersModal(company.id)">
                <td>
                   <div class="company-wrap">
                    {{company.id}}
                   </div>
                </td>
                <td class="table-company-name">
                  <div class="company-wrap">
                    {{company.name}}
                  </div>
                </td>
                <td>
                {{formatDate(company.createDate)}}
                </td>
                <td>
                  {{company.status === status.Active ? 'Active': 'Suspended'}}
                </td>
                <td v-if="accountType === `${accountTypes.Customer}`">
                  {{company.numberOfUsers}}
                </td>
                <template v-if="accountType === `${accountTypes.Reseller}`">
                  <td class="customer-count">
                    {{company.numberOfCustomers}}
                  </td>
                  <td class="customer-count">
                    {{company.numberOfResellers}}
                  </td>
                  <td class="customer-count">
                    {{company.numberOfTotalCustomers}}
                  </td>
                  <td class="customer-count">
                    {{company.numberOfTotalResellers}}
                  </td>
                </template>
                <td v-on:click.stop>
                  <popper class="cog-wheel" trigger="click" :appendToBody="true" :options="{placement: 'bottom-start'}">
                    <div class="popper">
                      <div class="dropdown-content">
                        <brand-hover>
                          <span class="dropdown-item" @click="showUsersModal(company.id)">
                            Login as <b>{{company.name}}</b> user
                          </span>
                        </brand-hover>
                        <hr class="dropdown-divider">
                        <brand-hover>
                          <span @click ="editCompany(company.id)" class="dropdown-item">
                            Edit company details
                          </span>
                        </brand-hover>
                      </div>
                    </div>
                    <cloud-plus-cog slot="reference"></cloud-plus-cog>
                    </popper>
                </td>
              </tr>
            </tbody>
          </table>
          </div>
        </div>
        <div v-if="checkIfCompaniesExist(companies.length, !searchTerm)" class="no-companies-warning">
          <div class="no-companies-warning__title">Oops! Seems like you don&#39;t have any  {{accountType === '0' ? 'Resellers' : 'Customers'}}!</div>
          <router-link class="no-companies-warning__subtitle" :to="createNewAccountLink" >
            <brand-color-primary><span class="link">Start building your {{accountType === '0' ? 'Reseller' : 'Customer'}} network</span></brand-color-primary>
          </router-link>
        </div>
        <div v-if="checkIfCompaniesExist(companies.length, searchTerm)" class="no-companies-warning">
          <div class="no-companies-warning__title">No results match your search criteria.</div>
        </div>
      </div>
    </div>

    <user-list-modal v-if="showModal" :showModal="showModal" :companyId='selectedCompanyId' @closeModal="closeModal"> </user-list-modal>
  </div>
</template>

<script>
import { sortCompare } from '@/helpers/utils';
import { mapActions, mapGetters } from 'vuex';
import Popper from '@/components/shared/popper/Popper';
import moment from 'moment';
import sortByMixin from '@/mixins/sortBy';
import loadingMixin from '@/mixins/loading';
import store from '@/store';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import UserListModal from '@/components/users/impersonateUser/UserListModal';
import constantsAccountType from '@/assets/constants/accountTypes';
import { companyStatus } from '@/assets/constants/commonConstants';
import CloudPlusCog from '@/components/shared/misc/CloudPlusCog';
import companyService from '@/services/companyService';
import appConfig from 'appConfig';
import OrderByItem from './OrderByItem';

export default {
  mixins: [sortByMixin, loadingMixin],
  props: {
    accountType: {
      type: String,
    },
  },
  components: {
    ComponentStickyHeader,
    UserListModal,
    OrderByItem,
    CloudPlusCog,
    Popper,
  },
  created() {
    this.orderBy = 'createDate';
    this.order = 'desc';
  },
  data() {
    return {
      selectedView: 1,
      view: {
        listView: 1,
        cardView: 2,
      },
      selectedCompanyId: 0,
      orderByItems: [
        {
          value: 'name',
          display: 'name',
        },
        {
          value: 'state',
          display: 'state',
        },
        {
          value: 'createDate',
          display: 'date',
        },
        {
          value: 'status',
          display: 'status',
        },
        {
          value: 'domains',
          display: 'domains',
        },
      ],
      searchTerm: '',
      showModal: false,
      accountTypes: constantsAccountType,
      status: companyStatus,
      defaultLogo: appConfig.imagePlaceholder,
      createNewAccountLink: `companies/create?accountType=${this.accountType}`,
      filteredCompanies: null,
    };
  },
  watch: {
    accountType() {
      // this.sort(this.companies);
      this.onSearch(this.searchTerm);
    },
  },
  computed: {
    ...mapGetters({
      companyId: 'userAuth/companyId',
    }),
    companies() {
      return this.sort(this.filteredCompanies || this.$store.getters['company/companies']);
    },
  },
  methods: {
    ...mapActions({
      getCompanies: 'company/getCompanies',
    }),
    formatDate(date) {
      if (date) {
        return moment(date).format('MM/DD/YYYY');
      }
      return '';
    },
    /* filter(companies) {
      return companies.filter(company =>
        (company.name && company.name.toLowerCase().includes(this.searchTerm)) ||
        (company.email && company.email.toLowerCase().includes(this.searchTerm)) ||
        (company.phoneNumber && company.phoneNumber.toLowerCase().includes(this.searchTerm)) ||
        (company.streetAddress && company.streetAddress.toLowerCase().includes(this.searchTerm)) ||
        (company.city && company.city.toLowerCase().includes(this.searchTerm)) ||
        (company.zipCode && company.zipCode.toLowerCase().includes(this.searchTerm)) ||
        (company.state && company.state.toLowerCase().includes(this.searchTerm)) ||
        (company.country && company.country.toLowerCase().includes(this.searchTerm)) ||
        (company.domains && this.searchTroughDomains(company.domains)));
    }, */
    sort(companies) {
      if (this.orderBy === 'domains') {
        return companies.sort((left, right) =>
          sortCompare(
            this.getPrimaryDomainName(left.domains), this.getPrimaryDomainName(right.domains),
            this.order,
          ));
      }
      return companies.sort((left, right) =>
        sortCompare(left[this.orderBy], right[this.orderBy], this.order));
    },
    showUsersModal(companyId) {
      this.selectedCompanyId = companyId;
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
    },
    createAccount() {
      this.$router.push({ path: `/companies/create?accountType=${this.accountType}` });
    },
    editCompany(companyId) {
      this.$router.push({
        path: `companies/edit/${companyId}`,
      });
    },
    getPrimaryDomainName(domains) {
      return domains.find(domain => domain.isPrimary === true).name;
    },
    searchTroughDomains(domains) {
      for (let i = 0; i < domains.length; i += 1) {
        if (domains[i].name.toLowerCase().includes(this.searchTerm)) {
          return true;
        }
      }
      return false;
    },
    checkIfCompaniesExist(companiesLength, searchTerm) {
      return companiesLength === 0 && searchTerm;
    },
    onSearch(searchKeys) {
      this.isLoading = true;
      if (searchKeys !== '') {
        setTimeout(() => {
          this.search(searchKeys);
        }, 500);
      } else {
        this.filteredCompanies = this.$store.getters['company/companies'];
        this.isLoading = false;
      }
    },
    search(term) {
      if (term === this.searchTerm) {
        term.replace(/%20/g, ' ');
        companyService.getCompaniesBySearch(this.companyId, this.accountType, term)
          .then(response => {
            this.filteredCompanies = response.data.result;
            // console.log(this.filteredCompanies);
            this.isLoading = false;
          });
      }
    },
  },
  mounted() {
    this.getCompanies({
      companyId: this.companyId,
      accountType: this.accountType,
    }).then(() => {
      this.isLoading = false;
    });
  },
  beforeRouteUpdate(to, from, next) {
    this.isLoading = true;
    to.meta.friendlyName = to.query.accountType === '0' ? 'Resellers' : 'Customers';
    store.dispatch('company/getCompanies', {
      companyId: store.getters['userAuth/userProfile'].companyId,
      accountType: to.query.accountType,
    }).then(() => {
      this.isLoading = false;
      next();
    });
  },
};
</script>
<style scoped lang="scss">

nav {
  background-color: white;
}
figure {
  cursor: pointer;
}
figure img {
  height: 120px;
  margin: 0 auto;
  object-fit: scale-down;
  cursor: pointer;
}
.navigation-level {
  padding: 0.7rem;
  border-bottom-width: 1.25rem;
  text-align: right;
  font-size: $small-font-size;
}

.card {
  min-height: 26rem;
}

.card .level {
  width: 100%;
  bottom: 1rem;
  position: absolute;
}

.card-header{
  box-shadow: none;
}

.card .level-item:not(:last-child){
  border-right: $border-height solid #dbdbdb;
}

.search-component{
  padding-right: 1.25rem;
  float: left;
}
.customer-count {
    width: 89px;
}
.dropdown-menu .dropdown-content .dropdown-item {
  padding: initial;
  word-break: break-all;
}

.has-text-weight-light:hover  .dropdown .dropdown-menu {
  visibility: visible;
}

.has-text-weight-light .dropdown .dropdown-menu {
  visibility: hidden;
}

.dropdown-menu {
  min-width: 8rem;
  padding-top: 0.6rem;
}

.dropdown-content {
  min-width: 11.4rem;
  max-height: 9.3rem;
}

.dropdown.is-right .dropdown-menu {
  left: -1rem;
}
.dropdown-domain-font {
  font-size: $primary-font-size;
  background-color: #efefef;
  padding: 0.3rem 0.55rem;
  display: inline-block;
  line-height: 0.8rem;
  text-align: left;
}
.domains-overflow {
  overflow-y: scroll;
}
.domain-name-padding {
  margin-left: 1rem;
  margin-right: 1rem;
}
.no-companies-warning{
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
.link{
  text-decoration: underline;
}
.component-min-width{
  min-width: 35rem;
}
.card-header-title{
  font-size: $secondary-font-size;
  font-weight: 500;
}
.company-status {
  display: flex;
  align-items: center;
  justify-content: center;
}
.status-indicator {
  font-size: 0.7rem;
  margin-right: 7px;
}
.filtering-label {
  margin-right: 1.75rem !important;
}

.company-name {
  font-size: 0.875rem;
  font-weight: 600;
  margin-bottom: 0.8rem;
}

thead th, thead td {
  text-align: center;
}
tbody tr{
  -webkit-transition: 0.2s ease all;
  -moz-transition: 0.2s ease all;
  -ms-transition: 0.2s ease all;
   transition: 0.2s ease all;
}
tbody tr:hover{
  box-shadow: 1px 1px 4px 1px rgba(0, 0, 0, 0.2);
  -webkit-box-shadow: 1px 1px 4px 1px rgba(0, 0, 0, 0.2);
  -moz-box-shadow: 1px 1px 4px 1px rgba(0, 0, 0, 0.2);
  cursor: pointer;
}
tbody tr td:first-child{
  word-wrap: break-word;
  white-space: nowrap;
}
thead th.company-heading{
  text-align:left;
   padding: 20px;
}

tbody tr td.table-company-name{
  padding: 20px;
  max-width: 212px;
  word-wrap: break-word;
  text-align: left;
}
tbody th, tbody td {
  text-align: center;
}
.table td{
  height: 100%;
  line-height: 0px;
  padding: 0px !important;
}
.table td .company-wrap{
  line-height: 18px;
  height: 100%;
  padding: 12px 20px;
}
.cog-wheel {
  justify-content: flex-end;
}

@media screen and (min-width: 1021px) {
  figure img {
    max-width: 11rem;
  }
}
@media screen and (max-width: 1020px) {
  figure img {
    max-width: 13rem;
  }
}
@media screen and (max-width: 770px) {
  .card{
    min-height: 32rem;
  }
  figure img {
    max-width: 17rem;
  }
}
@media screen and (max-width: 600px) {
  .card{
    min-height: 32rem;
  }
  figure img {
    max-width: 13rem;
  }
}
@media screen and (max-width: 500px) {
  .card{
    min-height: 32rem;
  }
  figure img {
    max-width: 10rem;
  }
}
</style>
