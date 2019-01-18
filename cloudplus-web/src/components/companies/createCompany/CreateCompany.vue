<template>
  <div>
    <component-sticky-header class="component-min-width" title="Create New Account">
      <div class="is-4 is-offset-2">
        <brand-primary-btn class="top-button" :disabled="stepper.activeStep != 3 || creatingInProgress" @click="saveAccount()">Create {{generalInformation.type === accountTypes.Customer ? 'Customer' : 'Reseller'}}</brand-primary-btn>
        <brand-secondary-btn class="top-button" :disabled="stepper.activeStep == 3" @click="nextStep()">Next Step</brand-secondary-btn>
        <brand-secondary-btn class="top-button" :disabled="stepper.activeStep == 1 || creatingInProgress" @click="previousStep()">Previous Step</brand-secondary-btn>
      </div>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <cloud-plus-step-progressbar :activeStep="stepper.activeStep" :steps="stepper.steps" v-else>
          <company-information slot="1"></company-information>
          <administrator-information slot="2"></administrator-information>
          <onboarding-information slot="3"></onboarding-information>
        </cloud-plus-step-progressbar>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters, mapMutations } from 'vuex';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import CloudPlusStepProgressbar from '@/components/shared/step-progressbar/CloudPlusStepProgressbar';
import accountTypesConstants from '@/assets/constants/accountTypes';
import CompanyInformation from './steps/CompanyInformation';
import AdministratorInformation from './steps/AdministratorInformation';
import OnboardingInformation from './steps/OnboardingInformation';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin, loadingMixin],
  props: {
    accountType: {
      type: String,
      default: `${accountTypesConstants.Customer}`,
    },
  },
  data() {
    return {
      stepper: {
        activeStep: 1,
        steps: [
          'Company Information',
          'Administrator Information',
          'Onboarding Information',
        ],
      },
      domains: [],
      creatingInProgress: false,
      accountTypes: accountTypesConstants,
    };
  },
  computed: {
    ...mapGetters({
      companyInformation: 'company/domainInformation',
      generalInformation: 'company/generalInformation',
      userEmails: 'user/userSecurityInformation',
    }),
  },
  components: {
    CloudPlusStepProgressbar,
    CompanyInformation,
    AdministratorInformation,
    OnboardingInformation,
    ComponentStickyHeader,
  },
  methods: {
    ...mapActions({
      createCompany: 'company/createCompany',
      getParentBranding: 'company/getParentBranding',
      getResellerCatalogs: 'catalog/getResellerCatalogs',
    }),
    ...mapMutations({
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
      resetCompanyState: 'company/RESET_COMPANY_STATE',
      resetUserState: 'user/RESET_USER_STATE',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
    }),
    nextStep() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.stepper.activeStep += 1;
        } else {
          this.validationErrorToaster();
          this.$bus.emit('changeFocus');
          this.$bus.emit('fieldFocus');
        }
        this.domains = this.companyInformation.domains.filter(domain => domain.name !== '');
        this.updateCompanyProperty({
          key: 'domains',
          value: this.domains,
        });
        if (this.stepper.activeStep === 3) {
          this.checkEmailSetup();
        }
      });
    },
    previousStep() {
      this.stepper.activeStep -= 1;
    },
    saveAccount() {
      this.creatingInProgress = true;
      this.createCompany().then(() => {
        const companyType = this.generalInformation.type === 0 ? 'Reseller' : 'Customer';
        this.sucessToaster({
          icon: 'building',
          text: `Your ${companyType} will be created shortly. Redirecting to ${companyType} list.`,
          onComplete: () => {
            this.$router.push({ path: `/companies?accountType=${this.generalInformation.type}` });
          },
        });
      });
    },
    checkEmailSetup() {
      if (this.userEmails.passwordSetupMethod === 2 && !this.userEmails.sendPlainPasswordViaEmail) {
        this.updateUserProperty({
          key: 'passwordSetupEmail',
          value: null,
        });
        this.updateUserProperty({
          key: 'passwordSetupEmailRetyped',
          value: null,
        });
      }
    },
  },
  mounted() {
    this.resetCompanyState();
    this.resetUserState();
    if (this.accountType !== '') {
      this.updateCompanyProperty({
        key: 'type',
        value: parseInt(this.accountType, 10),
      });
    }
    Promise.all([this.getParentBranding(), this.getResellerCatalogs()]).finally(() => {
      this.isLoading = false;
    });
  },
  beforeRouteLeave(to, from, next) {
    this.$bus.off();
    this.resetCompanyState();
    this.resetUserState();
    next();
  },
};
</script>

<style lang="scss" scoped>
.top-button {
  float: right;
}
.component-min-width {
  min-width: 45rem;
}
</style>
