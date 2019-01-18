<template>
  <div>
    <div class="columns">
      <div class="column">
        <label class="label has-text-weight-semibold">Catalog Name</label>
        <cloud-plus-textfield placeholder="Catalog Name" :value="resellerCatalog.name" @input="onInput('name', $event)" v-validate="'required'" data-vv-name="catalogName" data-vv-as="catalog name" name="catalogName" :validationErrors="errors.first('catalogName')"></cloud-plus-textfield>
      </div>
      <div class="column is-half">
        <label class="label has-text-weight-semibold">Applicable To</label>
        <assing-company-catalog></assing-company-catalog>
      </div>
      <div class="column">
        <label class="label has-text-weight-semibold">Valid From</label>
        <cloud-plus-textfield :value="formatDate(resellerCatalog.createDate)" :disabled="true"></cloud-plus-textfield>
      </div>
    </div>
    <div class="columns">
      <div class="column is-full">
        <label class="label has-text-weight-semibold">Description</label>
        <cloud-plus-text-area placeholder="Description ..." :value="resellerCatalog.description" @input="onInput('description', $event)"></cloud-plus-text-area>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import moment from 'moment';
import CloudPlusTextArea from '@/components/shared/input-components/CloudPlusTextarea';
import AssingCompanyCatalog from './AssingCompanyCatalog';

export default {
  inject: {
    $validator: '$validator',
  },
  components: { AssingCompanyCatalog, CloudPlusTextArea },
  computed: {
    ...mapGetters({
      resellerCatalog: 'catalog/resellerCatalog',
    }),
  },
  methods: {
    ...mapMutations({
      updateCatalogProperty: 'catalog/UPDATE_CATALOG_PROPERTY',
    }),
    onInput(key, value) {
      this.updateCatalogProperty({
        key,
        value,
      });
    },
    formatDate(date) {
      if (date) {
        return moment(date).format('MM/DD/YYYY');
      }
      return '';
    },
  },
};
</script>

<style scoped>
</style>
