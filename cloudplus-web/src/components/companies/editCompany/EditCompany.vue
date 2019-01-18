<template>
  <div>
    <component-sticky-header class="component-min-width" title="Company Details">
      <div class="is-4 is-offset-2">
        <brand-primary-btn :disabled="editInProgress" @click="edit">Save Company Details</brand-primary-btn>
      </div>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div v-else>
          <brand-tabs>
            <tabs>
              <tab-pane label="Company Details">
                <company-information :companyId="id" class="main-form" :newCompany ="false" :controlPanelSiteHidden="false"></company-information>
              </tab-pane>
              <tab-pane label="Company Users">
                <company-users :companyId="id"></company-users>
              </tab-pane>
            </tabs>
          </brand-tabs>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapMutations, mapGetters } from 'vuex';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import { Tabs, TabPane } from '@/components/shared/tabs';
import CompanyInformation from '@/components/companies/createCompany/steps/CompanyInformation';
import UsersTable from '@/components/users/UsersTable';
import accountTypesConstants from '@/assets/constants/accountTypes';
import CompanyUsers from '../CompanyUsers';

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
    CompanyInformation,
    CompanyUsers,
    UsersTable,
  },
  data() {
    return {
      editInProgress: false,
    };
  },
  computed: {
    ...mapGetters({
      company: 'company/generalInformation',
      companyDomains: 'company/domainInformation',
      userProfile: 'userAuth/userProfile',
      userPermissions: 'userAuth/permissions',
    }),
  },
  methods: {
    ...mapActions({
      updateCompany: 'company/updateCompany',
      getCompany: 'company/getCompany',
      getResellerCatalogs: 'catalog/getResellerCatalogs',
      getBranding: 'branding/getBranding',
    }),
    ...mapMutations({
      resetCompanyState: 'company/RESET_COMPANY_STATE',
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
    }),
    edit() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.editInProgress = true;
          this.updateCompany().then(() => {
            this.sucessToaster({
              icon: 'building-o',
              text: `The Company will be updated shortly. You will now be redirected to ${this.company.type === accountTypesConstants.Reseller ? 'My Resellers' : 'My Customers'} list`,
              onComplete: () => {
                if (this.id === this.userProfile.companyId) {
                  this.getBranding();
                }
                this.$router.go(-1);
              },
            });
          });
        } else {
          this.validationErrorToaster();
          this.$bus.emit('changeFocus');
          this.$bus.emit('fieldFocus');
        }
      });
    },
    checkIfWebsiteIsTheSameAsPrimaryDomain() {
      if (this.companyDomains.domains
        .find(domain => domain.isPrimary === true).name === this.companyDomains.website) {
        this.updateCompanyProperty({
          key: 'websiteSameAsPrimaryDomain',
          value: true,
        });
      }
      this.isLoading = false;
    },
  },
  mounted() {
    Promise.all([this.getCompany(this.id)]).then(() => {
      if (this.userPermissions.filter(permission => permission.name === 'ViewPriceCatalog').length) {
        Promise.all([this.getResellerCatalogs()]).then(() => {
          this.checkIfWebsiteIsTheSameAsPrimaryDomain();
        });
      } else {
        this.checkIfWebsiteIsTheSameAsPrimaryDomain();
      }
    });
  },
  beforeRouteLeave(to, from, next) {
    this.$bus.off();
    this.resetCompanyState();
    next();
  },
};
</script>

<style lang="scss" scoped>
.main-form {
  margin-top: -3rem;
}
.component-min-width{
  min-width: 30rem;
}
@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .component-main__white{
    height: 88rem;
  }
}
</style>
