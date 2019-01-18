<template>
  <div>
    <company-account-types v-if="newCompany"></company-account-types>
    <div class="columns">
      <div class="column column-padding-right">
        <brand-section-title>Account Information</brand-section-title>
        <company-general-information :companyId="companyId"></company-general-information>

        <brand-section-title>Company Information</brand-section-title>
        <company-contact-information></company-contact-information>

        <brand-section-title>Domain Information</brand-section-title>
        <company-domain-information :newCompany="newCompany" :isReseller="isReseller" :controlPanelSiteHidden="controlPanelSiteHidden"></company-domain-information>
      </div>
      <div class="column column-padding-left">
        <brand-section-title>Branding Information <cloud-plus-tooltip :tooltipText="tooltipText"> </cloud-plus-tooltip></brand-section-title>
        <company-branding-information></company-branding-information>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import accountTypesConstants from '@/assets/constants/accountTypes';
import CompanyAccountTypes from '../forms/CompanyAccountTypes';
import CompanyGeneralInformation from '../forms/CompanyGeneralInformation';
import CompanyContactInformation from '../forms/CompanyContactInformation';
import CompanyDomainInformation from '../forms/CompanyDomainInformation';
import CompanyBrandingInformation from '../forms/CompanyBrandingInformation';

export default {
  inject: {
    $validator: '$validator',
  },
  props: {
    newCompany: {
      type: Boolean,
      default: true,
    },
    companyId: {
    },
    controlPanelSiteHidden: {
      type: Boolean,
      default: true,
    },
  },
  components: {
    CompanyAccountTypes,
    CompanyGeneralInformation,
    CompanyContactInformation,
    CompanyDomainInformation,
    CompanyBrandingInformation,
  },
  computed: {
    ...mapGetters({
      company: 'company/generalInformation',
    }),
    isReseller() {
      return this.company.type === accountTypesConstants.Reseller;
    },
  },
  data() {
    return {
      tooltipText: 'Predefine what logo and colors customers and/or resellers see when they log in to their control panel',
    };
  },
};
</script>

<style lang="scss" scoped>
</style>

