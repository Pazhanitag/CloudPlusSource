<template>
  <div class="content">
    <create-company-domains v-if="newCompany" @input="onInput('domains', $event)" @setSupportSite="setSupportSite($event)" @sameAsPrimary="sameAsPrimaryDomainChecked" :value="company.domains" :sameAsPrimaryDomain="company.websiteSameAsPrimaryDomain">additional domain</create-company-domains>
    <edit-company-domains v-else :existingDomains="company.domains" @input="onInput('newDomains', $event)" :value="company.newDomains"></edit-company-domains>
    <cloud-plus-textfield v-model="websiteHost" data-vv-name="companyWebsite" name="companyWebsite" v-validate="'url'" data-vv-as="Company website" :validationErrors="errors.first('companyWebsite')" data-vv-delay="500">
      <span>Website
          <span class="same-as-website noselect" @click="setWebsite()">
              <div class="checkbox-icon">
                  <brand-input-icon class="brand-checkbox" >
                       <div class="checkbox-size" :style="{'color': branding.primaryColor}">
                         <i v-show="company.websiteSameAsPrimaryDomain" class="fa fa-check-square" aria-hidden="true"></i>
                         <i v-show="!company.websiteSameAsPrimaryDomain" class="fa fa-square-o" aria-hidden="true"></i>
                       </div>
                  </brand-input-icon>
              </div>
            <span class="same-as-website-text">Same as primary domain</span>
          </span>
      </span>
    </cloud-plus-textfield>
    <div v-if="isReseller">
      <div class="columns is-desktop is-1 is-variable">
        <div class="column">
      <cloud-plus-textfield v-model="supportSiteHost" data-vv-name="companySupportsite" name="companySupportsite" v-validate="'required|url'" data-vv-as="Company Support Site" :validationErrors="errors.first('companySupportsite')" data-vv-delay="500">Support Site</cloud-plus-textfield>
        </div>
      </div>
      <div v-show="!controlPanelSiteHidden">
        <label class="label has-text-weight-semibold">Control Panel Site</label>
        <div class="columns is-desktop is-1 is-variable">
          <div class="column">
            <cloud-plus-textfield v-model="controlPanelSiteUrlHost" data-vv-name="controlPanelSite" name="controlPanelSite" v-validate="'required|url'" data-vv-as="Control Panel Site" :validationErrors="errors.first('controlPanelSite')" data-vv-delay="500" :disabled="controlPanelSiteDisabled"></cloud-plus-textfield>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import EditCompanyDomains from '@/components/companies/editCompany/EditCompanyDomains';
import CreateCompanyDomains from './CreateCompanyDomains';

export default {
  props: {
    isReseller: {
      type: Boolean,
      default: false,
    },
    newCompany: {
      type: Boolean,
      default: true,
    },
    controlPanelSiteHidden: {
      type: Boolean,
      default: true,
    },
  },
  inject: {
    $validator: '$validator',
  },
  components: {
    CreateCompanyDomains,
    EditCompanyDomains,
  },
  data() {
    return {
      CloudPlusSubdomain: '',
      controlPanelSiteDisabled: true,
    };
  },
  computed: {
    ...mapGetters({
      company: 'company/domainInformation',
      branding: 'branding/brand',
    }),
    supportSiteHost: {
      // eslint-disable-next-line
      get: function() {
        return this.company.supportSite;
      },
      // eslint-disable-next-line
      set: function(newValue) {
        this.setUrlProperty(newValue, 'supportSite');
      },
    },
    websiteHost: {
      // eslint-disable-next-line
      get: function() {
        return this.company.website;
      },
      // eslint-disable-next-line
      set: function(newValue) {
        this.setUrlProperty(newValue, 'website');
      },
    },
    controlPanelSiteUrlHost: {
      // eslint-disable-next-line
      get: function() {
        return this.company.controlPanelSiteUrl;
      },
      // eslint-disable-next-line
      set: function(newValue) {
        this.setUrlProperty(newValue, 'controlPanelSiteUrl');
      },
    },
  },
  methods: {
    ...mapMutations({
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
    }),
    onInput(key, value) {
      if (key === 'domains') {
        this.updateUserProperty({
          key: 'domain',
          value: value.find(domain => domain.isPrimary).name,
        });
      }
      this.updateCompanyProperty({
        key,
        value,
      });
    },
    setWebsite() {
      this.onInput('websiteSameAsPrimaryDomain', !this.company.websiteSameAsPrimaryDomain);
      if (this.company.websiteSameAsPrimaryDomain) {
        this.onInput('website', this.company.domains[0].name);
        this.$validator.validate('companyWebsite', this.company.website);
      }
    },
    sameAsPrimaryDomainChecked() {
      this.updateCompanyProperty({
        key: 'website',
        value: this.company.domains[0].name,
      });
      this.$validator.validate('companyWebsite', this.company.website);
    },
    setSupportSite(newValue) {
      this.setUrlProperty(`support.${newValue}`, 'supportSite');
      // this.setUrlProperty(`my.${newValue}`, 'controlPanelSiteUrl');
    },
    setUrlProperty(newValue, key) {
      let url = newValue.replace('https://', '');
      url = url.replace('http://', '');
      this.updateCompanyProperty({
        key,
        value: url,
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.tooltip {
  position: relative;
  display: inline-block;
  top: 0.5rem;
}

.tooltip .tooltiptext {
  visibility: hidden;
  width: 21rem;
  background-color: black;
  color: #fff;
  text-align: center;
  border-radius: 0.8rem;
  padding: 0.5rem 0;
  position: absolute;
  z-index: 1;
  bottom: 100%;
  left: 50%;
  margin-left: -10rem;
}

.tooltip:hover .tooltiptext {
  visibility: visible;
}

.same-as-website{
  float: right;
  min-width: 12.6rem;
  position: relative;
}

.same-as-website-text{
  float: right;
  padding-top: 0.16rem;
}

@media screen and (max-width: 1600px) {
  .checkbox-size {
    font-size: 1.15rem;
  }
}
</style>
