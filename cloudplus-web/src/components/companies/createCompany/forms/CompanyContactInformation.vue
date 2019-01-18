<template>
<div class="content">
  <div class="columns">
    <div class="column">
      <cloud-plus-textfield
        name="email"
        v-validate="'required|email|max:254'"
        data-vv-as="Email"
        :validationErrors="errors.first('email')"
        @input="onInput('email', $event)"
        :value="company.email">Support Email Address</cloud-plus-textfield>
    </div>
    <div class="column">
      <cloud-plus-textfield
        name="phoneNumber"
        v-validate="'required|phone_number_format'"
        data-vv-as="Phone Number"
        :validationErrors="errors.first('phoneNumber')"
        @input="onInput('phoneNumber', $event)"
        :value="company.phoneNumber">Company Phone</cloud-plus-textfield>
    </div>
  </div>
  <div class="columns">
    <div class="column is-half">
      <cloud-plus-textfield
        name="streetAddress"
        v-validate="'required|max:90'"
        data-vv-as="Street Address"
        :validationErrors="errors.first('streetAddress')"
        @input="onInput('streetAddress', $event)"
        :value="company.streetAddress">Street Address</cloud-plus-textfield>
    </div>
    <div class="column is-half">
      <cloud-plus-textfield
        name="city"
        v-validate="'required|max:90'"
        data-vv-as="City"
        :validationErrors="errors.first('city')"
        @input="onInput('city', $event)"
        :value="company.city">City</cloud-plus-textfield>
    </div>
  </div>
  <div class="columns">
    <div class="column is-half">
      <cloud-plus-select v-if="showStatesDropdown" @input="onInput('state', $event)" :value="company.state" :selected="company.state" :options="states" :icon="'fa-globe'">State/Province</cloud-plus-select>
      <cloud-plus-textfield v-else
        name="state"
        value="N/A"
        :disabled="true">State/Province</cloud-plus-textfield>
    </div>
    <div class="column">
      <cloud-plus-textfield
        name="zipCode"
        v-validate="'required|alpha_num|max:10'"
        data-vv-as="Zip Code"
        :validationErrors="errors.first('zipCode')"
        @input="onInput('zipCode', $event)"
        :value="company.zipCode">Zip/Postal Code</cloud-plus-textfield>
    </div>
  </div>

  <div class="columns">
    <div class="column is-half">
      <cloud-plus-select
        @input="onInput('country', $event)"
        :value="company.country"
        :selected="company.country"
        :disabled="countries.length == 1"
        :options="countries.map(c => c.name)"
        :icon="'fa-globe'">Country</cloud-plus-select>
    </div>
  </div>
</div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import statesHandling from '@/mixins/statesHandling';
import Countries from '@/assets/constants/countries';

export default {
  inject: {
    $validator: '$validator',
  },
  mixins: [statesHandling],
  data() {
    return {
      countries: Countries,
    };
  },
  computed: {
    ...mapGetters({
      company: 'company/contactInformation',
    }),
  },
  methods: {
    ...mapMutations({
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
    }),
    onInput(key, value) {
      this.updateCompanyProperty({
        key,
        value,
      });
      if (key !== 'email') {
        this.updateUserProperty({
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
  },
};
</script>

<style>

</style>
