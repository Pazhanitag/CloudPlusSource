<template>
  <div class="content">
    <label class="label has-text-weight-semibold">Email</label>
    <div class="columns is-desktop is-1 is-variable">
      <div class="column">
        <cloud-plus-textfield
          :disabled="disableEmail"
          :value="user.userName"
          @input="onInputUpdateEmail(['userName', 'passwordSetupEmail','passwordSetupEmailRetyped'], $event)"
          :isLoading="checkingEmailAvailability"
          data-vv-name="userEmail"
          name="username"
          data-vv-delay="500"
          v-validate="emailValidations"
          data-vv-as="Email"
          :validationErrors="errors.first('userEmail')"></cloud-plus-textfield>
      </div>
      <div class="column">
        <cloud-plus-select
          :value="user.domain"
          :selected="user.domain"
          @input="domainChanged"
          :disabled="domains.length == 1 || disableEmail"
          :options="domains.map(d => d.name)"
          :icon="'fa-at'"></cloud-plus-select>
      </div>
    </div>
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-textfield
          :value="user.alternativeEmail"
          v-validate="'email'"
          data-vv-delay="300"
          data-vv-name="alternativeEmail"
          @input="onInput('alternativeEmail', $event)"
          data-vv-as="email"
          name="alternativeEmail"
          :validationErrors="errors.first('alternativeEmail')">Alternative Email</cloud-plus-textfield>
      </div>
    </div>
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-select
          :value="user.country"
          @input="updateCountry($event)"
          :selected="user.country"
          :disabled="countries.length == 1"
          :options="countries.map(c => c.name)"
          :icon="'fa-globe'">Country</cloud-plus-select>
      </div>
      <div class="column">
        <cloud-plus-select v-if="showStatesDropdown" @input="onInput('state', $event)" :value="user.state" :selected="user.state" :options="states" :icon="'fa-globe'">State/Province</cloud-plus-select>
      <cloud-plus-textfield v-else
        name="state"
        value="N/A"
        :disabled="true">State/Province</cloud-plus-textfield>
      </div>
    </div>
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-textfield
          :value="user.city"
          @input="onInput('city', $event)"
          v-validate="'required|max:90'"
          data-vv-name="city"
          name="city"
          data-vv-as="City"
          :validationErrors="errors.first('city')">City</cloud-plus-textfield>
      </div>
      <div class="column">
        <cloud-plus-textfield
          :value="user.zipCode"
          @input="onInput('zipCode', $event)"
          v-validate="'required|alpha_num|max:10'"
          data-vv-name="zipCode"
          name="zipCode"
          data-vv-as="Zip Code"
          :validationErrors="errors.first('zipCode')">Zip/Postal Code</cloud-plus-textfield>
      </div>
    </div>
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-textfield
          :value="user.streetAddress"
          @input="onInput('streetAddress', $event)"
          v-validate="'required|max:90'"
          data-vv-name="streetAddress"
          data-vv-as="street Address"
          name="streetAddress"
          :validationErrors="errors.first('streetAddress')">Street Address</cloud-plus-textfield>
      </div>
      <div class="column">
        <cloud-plus-textfield
          :value="user.phoneNumber"
          @input="onInput('phoneNumber', $event)"
          name="phoneNumber"
          v-validate="'required|phone_number_format'"
          data-vv-as="Phone Number"
          :validationErrors="errors.first('phoneNumber')">Phone Number</cloud-plus-textfield>
      </div>
    </div>
  </div>
</template>

<script>
import { Validator } from 'vee-validate';
import { mapGetters, mapMutations } from 'vuex';
import Countries from '@/assets/constants/countries';
import UserUtilitiesService from '@/services/userUtilitiesService';
import statesHandling from '@/mixins/statesHandling';

export default {
  inject: {
    $validator: '$validator',
  },
  mixins: [statesHandling],
  props: {
    domains: {
      type: Array,
      required: true,
    },
    disableEmail: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      checkingEmailAvailability: false,
      countries: Countries,
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/generalInformation',
    }),
    emailValidations() {
      return `required|max:200${(!this.disableEmail ? '|verify_no_special_characters|verify_email_exists' : '')}`;
    },
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      unsavedUserChangesPresent: 'user/UNSAVED_USER_CHANGES_PRESENT',
    }),
    onInputUpdateEmail(keys, value) {
      keys.forEach(key => {
        if (key === 'userName') {
          this.unsavedUserChangesPresent();
          this.updateUserProperty({ key, value });
        } else {
          const userNameWithDomain = value !== '' ? `${value}@${this.user.domain}` : '';
          if (this.validateEmail(userNameWithDomain) === true) {
            this.updateUserProperty({ key, value: userNameWithDomain });
          }
        }
      });
    },
    onInput(key, value) {
      this.unsavedUserChangesPresent();
      if (key === 'alternativeEmail') {
        this.updateUserProperty({
          key: 'alternativeEmail',
          value: value !== '' ? value : null,
        });
      } else {
        this.updateUserProperty({
          key,
          value,
        });
      }
    },
    validateEmail(email) {
      // eslint-disable-next-line
      const regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return regex.test(email);
    },
    addEmailExistValidator() {
      if (!this.disableEmail) {
        Validator.extend('verify_email_exists', {
          getMessage: field => `The ${field} already exists.`,
          validate: () => new Promise(resolve => {
            this.checkingEmailAvailability = true;
            UserUtilitiesService.emailAvailable(this.user.emailAddress).then(result => {
              this.checkingEmailAvailability = false;
              resolve({
                valid: result.data.result,
              });
            });
          }),
        });
      }
    },
    updateCountry(countryName) {
      this.unsavedUserChangesPresent();
      this.updateUserProperty({
        key: 'country',
        value: countryName,
      });
      this.updateUserProperty({
        key: 'countryCode',
        value: this.countries.find(c => c.name === countryName).code,
      });
    },
    setState(state) {
      this.updateUserProperty({
        key: 'state',
        value: state,
      });
    },
    domainChanged(domain) {
      this.unsavedUserChangesPresent();
      this.user.emailAddress = `${this.user.userName}@${domain}`;
      this.$validator.validate('userEmail');
      this.updateUserProperty({
        key: 'domain',
        value: domain,
      });
    },
  },
  created() {
    this.addEmailExistValidator();
    if (this.user.domain === '') {
      this.updateUserProperty({
        key: 'domain',
        value: this.domains.find(d => d.isPrimary).name,
      });
    }
    this.updateUserProperty({
      key: 'countryCode',
      value: this.countries.find(c => c.name === this.user.country).code,
    });
  },
};
</script>

<style>

</style>
