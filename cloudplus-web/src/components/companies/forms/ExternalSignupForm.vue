<template>
        <div class="columns is-centered">
          <div class="column is-6 card-column" :class="{'sucess-signup':signupStatus === status.Success }">
            <div class="card">
              <div class="card-content">
                <div class="card-title">

                  <div class="column form-section">
                    <p class="form-section-header" :style="{'color': primaryBrandColor}">General Company Information</p>
                    <div>
                      <cloud-plus-text-input name="Company Name" :value="company.name" v-validate="'required'" :validationErrors="errors.first('Company Name')" @input="onInput('name', $event)">Account Name</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-text-input name="Primary Domain" :value="company.domain" v-validate="'required|url|verify_domain_exists:Primary Domain'" :validationErrors="errors.first('Primary Domain')" @input="setWebsitesByDomainValue('domain', $event)" :isLoading="checkingDomainAvailability['Primary Domain']">Primary Domain</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-textfield v-model="websiteHost" data-vv-name="companyWebsite" name="companyWebsite" v-validate="'url'" data-vv-as="Company website" :validationErrors="errors.first('companyWebsite')" data-vv-delay="500">
                        Website
                        <span class="same-as-website" @click="setWebsiteToDomain()">
                              <div class="checkbox-icon">
                                <brand-input-icon class="brand-checkbox" >
                                  <div :style="{'color': primaryBrandColor}">
                                  <i v-show="company.websiteSameAsPrimaryDomain" class="fa fa-check-square" aria-hidden="true"></i>
                                  <i v-show="!company.websiteSameAsPrimaryDomain" class="fa fa-square-o" aria-hidden="true"></i>
                                  </div>
                                </brand-input-icon>
                              </div>
                          <span class="same-as-website-text">Same as primary domain</span>
                        </span>

                      </cloud-plus-textfield>
                    </div>
                    <div v-if="company.accountType === accountTypes.Reseller">
                      <cloud-plus-textfield v-model="supportSiteHost" data-vv-name="companySupportsite" name="companySupportsite" v-validate="'url'" data-vv-as="Company Support Site" :validationErrors="errors.first('companySupportsite')" data-vv-delay="500">Support Site</cloud-plus-textfield>
                    </div>
                    <div v-if="company.accountType === accountTypes.Reseller">
                      <cloud-plus-textfield v-model="controlPanelSiteUrlHost" data-vv-name="controlPanelSite" name="controlPanelSite" v-validate="'url|verify_no_special_characters'" data-vv-as="Control Panel Site" :validationErrors="errors.first('controlPanelSite')" data-vv-delay="500" v-show="controlPanelSiteDisabled">Control Panel Site</cloud-plus-textfield>
                    </div>
                    <div>
                      <cloud-plus-textfield name="email" v-validate="'required|email'" data-vv-as="Email" :validationErrors="errors.first('email')" @input="onInput('email', $event)" :value="company.email">Support Email Address
                      </cloud-plus-textfield>
                    </div>
                  </div>

                  <div class="column form-section">
                    <p class="form-section-header" :style="{'color': primaryBrandColor}">First User Contact Information</p>
                    <div>
                      <cloud-plus-text-input name="First Name" :value="user.firstName" v-validate="'required'" :validationErrors="errors.first('First Name')" @input="onInput('firstName', $event, 'user')">First Name</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-text-input name="Last Name" :value="user.lastName" v-validate="'required'" :validationErrors="errors.first('Last Name')" @input="onInput('lastName', $event, 'user')">Last Name</cloud-plus-text-input>
                    </div>
                    <div >
                      <cloud-plus-textfield @focus="onDisplayNameFocus" @input="onInput('displayName', $event, 'user')" name="displayName" v-validate="'required'" data-vv-as="Display Name" :value="user.displayName" :validationErrors="errors.first('displayName')">Display Name</cloud-plus-textfield>
                    </div>
                    <div>
                      <cloud-plus-password-input name="Password" :value="user.password" v-validate="'required|min:6'" :validationErrors="errors.first('Password')" @input="onInput('password', $event, 'user')">Password</cloud-plus-password-input>
                    </div>
                    <div>
                      <cloud-plus-password-input name="Retype Password" :value="user.passwordRetyped" v-validate="`required|min:6|retyped_password:${user.password}`" :validationErrors="errors.first('Retype Password')" @input="onInput('passwordRetyped', $event, 'user')">Retype Password</cloud-plus-password-input>
                    </div>
                    <div class="emailContainer">
                      <cloud-plus-textfield class="inline" :value="user.userName" @input="setEmailUsername($event)" :isLoading="checkingEmailAvailability" data-vv-name="emailUsername" name="emailUsername" data-vv-delay="500" v-validate="'required|verify_no_special_characters|verify_email_available'" data-vv-as="Email Address" :validationErrors="errors.first('emailUsername')">Email Address</cloud-plus-textfield>
                      <div class="atSign">@</div>
                      <cloud-plus-textfield class="inline emailDomain" :disabled="disableEmailDomain" :value="company.domain"></cloud-plus-textfield>
                    </div>
                    <div>
                      <cloud-plus-text-input name="Alternative Email Address" :value="user.alternativeEmail" v-validate="`required|email`" :validationErrors="errors.first('Alternative Email Address')" @input="onInput('alternativeEmail', $event, 'user')">Alternative Email Address</cloud-plus-text-input>
                    </div>
                  </div>

                  <div class="column form-section">
                    <p class="form-section-header" :style="{'color': primaryBrandColor}">Company Contact Information</p>
                    <div>
                      <cloud-plus-text-input :value="company.phoneNumber" name="Phone Number" v-validate="'required|phone_number_format'" :validationErrors="errors.first('Phone Number')" @input="onInput('phoneNumber', $event, 'both')">Company Phone
                      </cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-text-input :value="company.streetAddress" name="Street Address" v-validate="'required'" :validationErrors="errors.first('Street Address')" @input="onInput('streetAddress', $event, 'both')">Street</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-text-input :value="company.city" name="City" v-validate="'required'" :validationErrors="errors.first('City')" @input="onInput('city', $event, 'both')">City</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-select v-if="showStatesDropdown" @input="onInput('state', $event, 'both')" :value="company.state" :selected="company.state" :options="states" :icon="'fa-globe'">State/Province</cloud-plus-select>
                      <cloud-plus-text-input v-else :value="company.state" name="State" v-validate="'required'" :validationErrors="errors.first('State')" @input="onInput('state', $event, 'both')">State/Province</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-text-input :value="company.zipCode" name="Zip Code" v-validate="'required|alpha_num|max:10'" :validationErrors="errors.first('Zip Code')" @input="onInput('zipCode', $event, 'both')">Zip/Postal Code</cloud-plus-text-input>
                    </div>
                    <div>
                      <cloud-plus-select :value="company.country" @input="updateCountry($event)" :selected="company.country" :disabled="countries.length == 1" :options="countries.map(c => c.name)" :icon="'fa-globe'">Country</cloud-plus-select>
                    </div>

                    <div class="separator-30"></div>
                    <div class="control is-pulled-left" v-show="signupStatus !== status.Success">
                      <vue-recaptcha
                        ref="invisibleRecaptcha"
                        @verify="onVerify"
                        @expired="onExpired"
                        :sitekey="sitekey">
                        <button type="submit" class="button is-info" :style="{'background-color': primaryBrandColor}">Sign me up</button>
                      </vue-recaptcha>
                    </div>
                    <button type="submit" class="button is-info" v-show="signupStatus === status.Success" :style="{'background-color': primaryBrandColor}">Sign me up</button>
                  </div>
                </div>
                <div class="separator-20"></div>
                <brand-spinner v-show="signupStatus == status.Saving"></brand-spinner>
                <brand-error-message v-show="signupStatus == status.Error">
                  Something went wrong with your sign up attempt. Please try again, or contact the administrator.
                </brand-error-message>
              </div>
            </div>
          </div>
          <template slot="footer"> Problems with your account, don't have access to those email addresses?
            <a href="">Contact us!</a>
          </template>
          <external-signup-success :accountType="accountType" v-show="signupStatus === status.Success"></external-signup-success>
        </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import appConfig from 'appConfig';
import VueRecaptcha from 'vue-recaptcha';
import CloudPlusCard from '@/components/shared/CloudPlusCard';
import accountTypesConstants from '@/assets/constants/accountTypes';
import Countries from '@/assets/constants/countries';
import RoleService from '@/services/roleService';
import CloudPlusTextInput from '@/components/shared/input-components/CloudPlusTextfield';
import CloudPlusPasswordInput from '@/components/shared/input-components/CloudPlusPasswordTextfield';
import CloudPlusRadioButton from '@/components/shared/input-components/CloudPlusRadioButton';
import BrandButton from '@/components/shared/styled-components/brand-buttons/BrandButton';
import domainExistRule from '@/mixins/domainExistRule';
import emailAvailableRule from '@/mixins/emailAvailableRule';
import statesHandling from '@/mixins/statesHandling';
import ExternalSignupSuccess from './ExternalSignupSuccess';
import recaptchaService from '../../../services/recaptchaService';
import statusConstants from '../../../assets/constants/status';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [domainExistRule, emailAvailableRule, statesHandling],
  components: {
    CloudPlusCard,
    'vue-recaptcha': VueRecaptcha,
    CloudPlusTextInput,
    CloudPlusPasswordInput,
    CloudPlusRadioButton,
    BrandButton,
    ExternalSignupSuccess,
    statusConstants,
  },
  inject: {
    $validator: '$validator',
  },
  props: {
    accountType: {
      required: true,
    },
    parentId: {
      type: String,
      required: true,
    },
    brandColor: {
      type: String,
      default: '',
    },
  },
  data() {
    return {
      sitekey: appConfig.googleRecaptchaSiteKey,
      status: statusConstants,
      signupStatus: statusConstants.Inaction,
      accountTypes: accountTypesConstants,
      roles: {
        customerAdmin: {},
        resellerAdmin: {},
      },
      checkingDomainAvailability: [false],
      checkingEmailAvailability: false,
      countries: Countries,
      disableEmailDomain: true,
      CloudPlusSubdomain: '',
      primaryBrandColor: '',
      controlPanelSiteDisabled: false,
    };
  },
  created() {
    this.getRoles();
    this.onInput('passwordSetupMethod', 2, 'user');
    this.onInput('parentUniqueIdentifier', this.$props.parentId);
    const theRole = this.$props.accountType === this.accountTypes.Reseller ?
      this.roles.resellerAdmin : this.roles.customerAdmin;
    this.setAccountType(this.$props.accountType, theRole.id);
    this.CloudPlusSubdomain = appConfig.cloudPlusControlPanelSubdomain;
    this.onInput('websiteSameAsPrimaryDomain', true);
    this.primaryBrandColor = this.$route.query.brandColor;
  },
  computed: {
    ...mapGetters({
      company: 'company/externalSignupForm',
      user: 'user/externalSignupForm',
    }),
    supportSiteHost: {
      // eslint-disable-next-line
      get: function() {
        return this.company.supportSiteUrl;
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
      setCompany: 'company/SET_COMPANY',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
    }),
    onInput(key, value, subject) {
      if (subject === 'user') {
        this.updateUserProperty({
          key,
          value,
        });
      } else {
        if (subject === 'both') {
          this.updateUserProperty({
            key,
            value,
          });
        }
        if (key === 'name') {
          this.updateUserProperty({
            key: 'companyName',
            value,
          });
        }
        this.updateCompanyProperty({
          key,
          value,
        });
      }
    },
    setState(state) {
      this.updateCompanyProperty({
        key: 'state',
        value: state,
      });
    },
    setWebsitesByDomainValue(key, value) {
      this.onInput(key, value);
      this.onInput(key, value, 'user');
      this.onInput('emailAddress', `${this.user.userName}@${this.company.domain}`, 'user');
      if (this.company.websiteSameAsPrimaryDomain) {
        this.updateCompanyProperty({
          key: 'website',
          value: this.company.domain,
        });
      }
      this.onInput('supportSite', `support.${value}`);
      // this.onInput('controlPanelSiteUrl', `my.${value}`);
    },
    updateCountry(countryName) {
      this.onInput('country', countryName);
      this.onInput('country', countryName, 'user');
      this.onInput('countryCode', this.countries.find(c => c.name === countryName).code);
    },
    setEmailUsername(value) {
      this.onInput('userName', value, 'user');
      this.onInput('emailAddress', `${value}@${this.company.domain}`, 'user');
    },
    redirect() {
      this.$router.push('/');
    },
    onVerify(response) {
      // eslint-disable-next-line
      const sitekey = this.sitekey;
      const requestObject = {
        response, sitekey,
      };
      this.resetRecaptcha();

      this.$validator.validateAll().then(result => {
        if (!result) return false;

        recaptchaService.verifyRecaptcha(requestObject).then(recaptchaResponse => {
          const verificationResponse = recaptchaResponse.data.result;
          if (!verificationResponse.success) {
            return false;
          }
          return this.createCompany();
        });
        return true;
      });
    },
    onExpired() {
      this.resetRecaptcha();
    },
    resetRecaptcha() {
      this.$refs.invisibleRecaptcha.reset();
    },
    createCompany() {
      this.signupStatus = this.status.Saving;
      this.$store.dispatch('company/createCompanyFromExternalSignupForm').then(() => {
        this.signupStatus = this.status.Success;
      }).catch(err => {
        console.log(err);
        this.signupStatus = this.status.Error;
      });
    },
    getRoles() {
      const self = this;
      RoleService.getAllRolesRPC().then(response => {
        const roles = response.data.result;
        self.roles.customerAdmin = roles.find(role => role.name === appConfig.customerRole);
        self.roles.resellerAdmin = roles.find(role => role.name === appConfig.resellerRole);
        self.assignRole(self.company.accountType === self.accountTypes.Reseller ?
          self.roles.resellerAdmin.id : self.roles.customerAdmin.id);
      });
    },
    assignRole(roleId) {
      this.updateUserProperty({
        key: 'roles',
        value: [roleId],
      });
    },
    setAccountType(accountType, roleId) {
      this.updateCompanyProperty({
        key: 'accountType',
        value: accountType,
      });
      this.assignRole(roleId);
    },
    onDisplayNameFocus() {
      if (this.user.displayName === '') {
        this.updateUserProperty({
          key: 'displayName',
          value: `${this.user.firstName} ${this.user.lastName}`,
        });
        this.$validator.validate('displayName', this.user.displayName);
      }
    },
    setWebsiteToDomain() {
      this.company.websiteSameAsPrimaryDomain = !this.company.websiteSameAsPrimaryDomain;
      this.onInput('websiteSameAsPrimaryDomain', this.company.websiteSameAsPrimaryDomain);
      if (this.company.websiteSameAsPrimaryDomain) {
        this.updateCompanyProperty({
          key: 'website',
          value: this.company.domain,
        });
        this.$validator.validate('companyWebsite', this.company.website);
      }
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

<style scoped>
.form-section {
  padding-bottom: 20px;
  padding-top: 20px;
}

.form-section-header {
  font-weight: 900;
  font-size: 10pt;
  text-align: center;
  margin-bottom: 15px;
}

.inline {
  display: inline-block;
  width: 48%;
}

.emailContainer {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 0.75rem;
}

.emailContainer .field:not(:last-child) {
    margin-bottom: 0;
}

.form-section div .emailDomain{
  padding-top: 1.87rem;
}

.card-column{
  width: 100%;
  max-width: 100%;
  -webkit-box-flex: 0;
  -ms-flex: 0 0 100%;
  flex: 0 0 100%;
}

.is-centered{
  /*width: 36rem;*/
  width: auto;
  background: white;
  margin-right: 0px;
}

.card{
  box-shadow: none;
}

.is-info{
  top: 15px;
}

.same-as-website {
  float: right;
  width: 12rem;
}

.form-section div {
  padding-top: 3px;
}

.brand-checkbox{
  margin: -0.8525rem 0.3125rem 0 0;
}

.form-section div .atSign {
  padding-top: 2.5rem;
}

.sucess-signup{
  width: 100%;
  height: 100%;
  position: fixed;
  top: 0px;
  left: 0px;
  background-color: #fff;
  opacity: 0.3;
  z-index: -1;
}
</style>
