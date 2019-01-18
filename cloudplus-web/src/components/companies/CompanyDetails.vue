<template>
  <div>
    <component-sticky-header class="component-min-width" :title="companyLevel === `${companyLevelConstants.MyCompany}` ? 'My Company' : 'Support'">
      <brand-primary-btn v-if="companyLevel === `${companyLevelConstants.MyCompany}`" v-can-see="['EditMyCompany']" @click="editCompany()" class="is-pulled-right">Edit company</brand-primary-btn>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <section v-else>
          <div class="content">
            <div>
              <figure>
                <img :src="company.logoUrl === '' ? defaultLogo : company.logoUrl" class="imgstyle" alt="Company Logo">
              </figure>
            </div>

            <div class="has-text-centered">
              <span class="has-text-weight-semibold">{{company.name}}</span>
            </div>

            <div class="has-text-centered" v-if="company.domains != undefined">
                      <span class="company-additional-info">
                        {{getPrimaryDomainName(company.domains)}} <span v-if="company.domains.length > 1"> +</span>
                        <span class="has-text-weight-light dropdown-domain-font" v-show="company.domains.length > 1">
                          {{company.domains.length - 1}} more
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
                        </span>
                    </span>
            </div>

            <div class="has-text-centered">
              <span class="company-additional-info">{{company.country}}, {{company.state}}, {{company.city}}</span>
            </div>
            <div class="has-text-centered">
              <span class="company-additional-info">ID: {{company.id}}</span>
            </div>
            <div class="has-text-centered">
              <i class="fa fa-circle" :class="{'has-text-success': company.status === status.Active, 'has-text-danger': company.status === status.Suspended}" aria-hidden="true"></i>
              <span class="company-additional-info">{{company.status === status.Active ? 'Active': 'Suspended'}}</span>
            </div>
          </div>
          <div class="column-padding-left column-padding-right">

            <div class="columns">
              <div class="column">
                <brand-section-title class="branding-section-margin">Contact Information</brand-section-title>
              </div>
            </div>

            <div class="columns">
              <div class="column" v-for="(contact,key) in contactInformation" :key="key" >
                <label class="radio has-text-weight-semibold">
                  {{contact.display}}
                </label>
                <p v-if="contact.label !== 'link'" class="company-data has-text-weight-light">{{company[contact.value]}}</p>
                <brand-color-primary v-else><p> <a class="company-data has-text-weight-light" target="_blank" :href="`//${company[contact.value]}`" >{{company[contact.value]}}</a> </p></brand-color-primary>
              </div>
            </div>

            <div class="columns">
              <div class="column">
                <brand-section-title class="branding-section-margin">Location Information</brand-section-title>
              </div>
            </div>

            <div class="columns">
              <div class="column" v-for="(location,key) in locationInformation" :key="key" >
                <label class="radio has-text-weight-semibold">
                  {{location.display}}
                </label>
                <p class="company-data has-text-weight-light">{{company[location.value]}}</p>
              </div>
            </div>

           <div v-can-see="['ViewExternalSignupLink']">
            <div class="columns" v-if="companyLevel === `${companyLevelConstants.MyCompany}`">
              <div class="column">
                 <brand-section-title class="company-data branding-section-margin">Onboarding <cloud-plus-tooltip :tooltipText="tooltipText"> </cloud-plus-tooltip>
                </brand-section-title>
              </div>
            </div>

            <div class="columns" v-if="companyLevel === `${companyLevelConstants.MyCompany}`">
              <div class="column" >
                <label class="radio has-text-weight-semibold">
                  Reseller Signup Form
                </label>
                <p class="company-data iframe-code has-text-weight-light">
                  <code>{{resellerLink}}</code>
                </p>
              </div>

              <div class="column" >
                <label class="radio has-text-weight-semibold">
                  Customer Signup Form
                </label>
                <p class="company-data iframe-code has-text-weight-light">
                  <code>{{customerLink}}</code>
                </p>
              </div>
            </div>
            </div>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>

<script>
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import loadingMixin from '@/mixins/loading';
import { mapGetters, mapActions, mapMutations } from 'vuex';
import appConfig from 'appConfig';
import { contactResellerInformation, contactCustomerInformation, locationCompanyInformation, companyStatus, companyLevel } from '@/assets/constants/commonConstants';

export default {
  components: {
    ComponentStickyHeader,
  },
  mixins: [loadingMixin],
  computed: {
    ...mapGetters({
      userProfile: 'userAuth/userProfile',
      companyId: 'userAuth/companyId',
      branding: 'branding/brand',
      userRole: 'userAuth/userProfileRole',
    }),
  },
  props: {
    companyLevel: {
      type: String,
    },
  },
  data() {
    return {
      status: companyStatus,
      contactInformation: [],
      locationInformation: locationCompanyInformation,
      companyLevelConstants: companyLevel,
      company: [],
      defaultLogo: appConfig.imagePlaceholder,
      resellerLink: appConfig.externalResellerIframe,
      customerLink: appConfig.externalCustomerIframe,
      customerRole: appConfig.customerRole,
      tooltipText: 'To enable reseller and/or customer signup on your website simply copy the code below to you web page',
    };
  },
  methods: {
    ...mapActions({
      getCompany: 'company/getCompany',
      getParentCompany: 'company/getParentCompanyByCompanyId',
    }),
    ...mapMutations({
      resetCompanyState: 'company/RESET_COMPANY_STATE',
    }),
    editCompany() {
      this.$router.push({
        path: '/companies/edit-my-company',
      });
    },
    handleRouteChange(to, from, next) {
      to.meta.friendlyName = to.query.companyLevel === `${this.companyLevelConstants.MyCompany}` ? 'My Company Details' : 'Parent Details';
      if (to.query.companyLevel === `${this.companyLevelConstants.MyCompany}`) {
        this.getCompany(this.userProfile.companyId).then(response => {
          if (response.status === 200) {
            this.setValues(response);
          }
        });
        next();
      }
      if (to.query.companyLevel === `${this.companyLevelConstants.Parent}`) {
        this.getParentCompany(this.userProfile.companyId).then(response => {
          if (response.status === 200) {
            this.setValues(response);
          }
        });
        next();
      }
    },
    setValues(response) {
      this.company = response.data.result;
      this.contactInformation = this.company.type === 1 ?
        contactCustomerInformation : contactResellerInformation;
      this.resellerLink = this.resellerLink.replace('uniqueIdentifier', this.company.uniqueIdentifier);
      this.resellerLink = this.resellerLink.replace('uniqueColor', this.branding.primaryColor.substring(1));
      this.customerLink = this.customerLink.replace('uniqueIdentifier', this.company.uniqueIdentifier);
      this.customerLink = this.customerLink.replace('uniqueColor', this.branding.primaryColor.substring(1));
      this.isLoading = false;
    },
    getPrimaryDomainName(domains) {
      return domains.find(domain => domain.isPrimary === true).name;
    },
  },
  mounted() {
    if (this.$route.query.companyLevel === `${this.companyLevelConstants.Parent}`) {
      this.getParentCompany(this.userProfile.companyId).then(response => {
        if (response.status === 200) {
          this.setValues(response);
        }
      });
    } else {
      this.getCompany(this.userProfile.companyId).then(response => {
        if (response.status === 200) {
          this.setValues(response);
        }
      });
    }
  },
  beforeRouteUpdate(to, from, next) {
    this.handleRouteChange(to, from, next);
  },
  beforeRouteLeave(to, from, next) {
    this.resetCompanyState();
    next();
  },
};
</script>

<style lang="scss" scoped>
figure img {
  height: 150px;
  width: 150px;
  margin: 0 auto;
  position: inherit !important;
  object-fit: scale-down;
}
figure{
  padding-top: 0 !important;
}
.branding-section-margin {
  margin-bottom: 0;
  margin-top: 1rem;
}
.column-padding-right{
  padding-right: 0.7rem;
}
.radio {
  font-size: 0.875rem;
  padding-bottom: 1rem;
}
.location-padding {
  padding-top: 3.5rem;
  text-align: center;
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
  max-width: 11.4rem;
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
.component-min-width{
  min-width: 40rem;
}
.company-data {
  font-size: 0.875rem;
}
.iframe-code {
  background-color: whitesmoke;
  margin-bottom: 3rem;
  padding: 1rem;
}
code {
  color: #393318;
}
</style>
