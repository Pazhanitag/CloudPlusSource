<template>
  <div class="content">
    <div class="columns">
      <div class="column">
        <cloud-plus-textfield
          @input="onInput('name', $event)"
          :value="company.name"
          data-vv-name="accountName"
          name="accountName"
          v-validate="'required|max:60'"
          data-vv-as="Account Name"
          :validationErrors="errors.first('accountName')">Account Name</cloud-plus-textfield>
      </div>
      <div class="column">
        <cloud-plus-select
          @input="onInput('billingType', $event)"
          :value="company.billingType"
          :options="billingTypes">Billing Type</cloud-plus-select>
      </div>
    </div>
    <div class="columns" v-show="showSelectCatalog">
      <div class="column">
        <cloud-plus-select
          @input="onInput('catalogId', $event)"
          :selected="company.catalogId"
          :options="availableCatalogs.map(catalog => ({value: catalog.id, name: catalog.name}))">Price Schedule</cloud-plus-select>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import billingTypeConstants from '@/assets/constants/billingTypes';

export default {
  inject: {
    $validator: '$validator',
  },
  props: {
    companyId: {
    },
  },
  data() {
    return {
      billingTypes: billingTypeConstants,
    };
  },
  computed: {
    ...mapGetters({
      company: 'company/generalInformation',
      availableCatalogs: 'catalog/resellerCatalogs',
      userProfile: 'userAuth/userProfile',
    }),
    showSelectCatalog() {
      return this.companyId !== this.userProfile.companyId;
    },
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
      if (key === 'name') {
        this.updateUserProperty({
          key: 'companyName',
          value,
        });
      }
    },
    setDefaultCatalog() {
      if (this.company.catalogId) {
        return;
      }
      const defaultCatalog = this.availableCatalogs.find(c => c.default);
      this.updateCompanyProperty({
        key: 'catalogId',
        value: defaultCatalog.id,
      });
    },
  },
  mounted() {
    this.setDefaultCatalog();
  },
};
</script>

<style>

</style>
