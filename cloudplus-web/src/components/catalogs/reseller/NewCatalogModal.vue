<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal" :modalContentHeight="'auto'"  width="50rem">
      <p slot="header">Add New Price Schedule</p>
      <div slot="content" class="content">
        <h1 class="title"> You are adding <b>New Price Schedule</b> for available products</h1>
        <h2 class="subtitle">Please add some detailed description</h2>
      </div>
      <section slot="section">
        <div class="columns">
          <div class="column is-one-half">
            <label class="label has-text-weight-semibold">Schedule Name</label>
            <cloud-plus-textfield :value="currentCatalog.name" data-vv-name="Catalog Name" v-validate="'required'" :validationErrors="errors.first('Catalog Name')" @input="onInput('name', $event)"></cloud-plus-textfield>
            <label class="label has-text-weight-semibold">Valid From</label>
            <cloud-plus-textfield :value="today" disabled="disabled"></cloud-plus-textfield>
          </div>

          <div class="column is-one-half">
            <label class="label has-text-weight-semibold">Use prices from</label>
            <cloud-plus-select :options="catalogs.map(catalog  => ({value: catalog.id, name: catalog.name}))" :selected="selectedItem" @input="usePricesFrom($event)"></cloud-plus-select>
          </div>
        </div>

        <div class="columns">
          <div class="column">
            <label class="label has-text-weight-semibold">Applicable To</label>
            <assing-company-catalog></assing-company-catalog>
          </div>
        </div>
        <div class="columns">
          <div class="column">
             <label class="label has-text-weight-semibold">Description</label>
            <cloud-plus-text-area placeholder="Description ..." :value="currentCatalog.description" @input="onInput('description', $event)"></cloud-plus-text-area>
          </div>
        </div>
      </section>
      <div slot="footer">
        <brand-primary-btn class="btn-new-catalog--modal-footer" :disabled="saving" @click="createNewCatalog()">
          Save
          <loading-icon :inline="true" v-show="saving"></loading-icon>
        </brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>e

<script>
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import toasterMixin from '@/mixins/toaster';
import { mapActions, mapGetters, mapMutations } from 'vuex';
import moment from 'moment';
import CloudPlusTextArea from '@/components/shared/input-components/CloudPlusTextarea';
import AssingCompanyCatalog from './AssingCompanyCatalog';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin],
  components: {
    CloudPlusCardModal,
    AssingCompanyCatalog,
    CloudPlusTextArea,
  },
  props: {
    showModal: {
      type: Boolean,
    },
  },
  data() {
    return {
      selectedItem: '',
      saving: false,
    };
  },
  computed: {
    ...mapGetters({
      newCatalog: 'catalog/createCatalog',
      catalogs: 'catalog/resellerCatalogs',
      currentCatalog: 'catalog/resellerCatalog',
    }),
    today() {
      return moment().format('MM/DD/YYYY');
    },
  },
  methods: {
    ...mapActions({
      createCatalog: 'catalog/createCatalog',
    }),
    ...mapMutations({
      updateCatalogProperty: 'catalog/UPDATE_CATALOG_PROPERTY',
      resetCatalog: 'catalog/RESET_CURRENT_PRICE_CATALOG_STATE',
    }),
    createNewCatalog() {
      const catalogToCreate = Object.assign({}, this.newCatalog);
      catalogToCreate.sourceCatalogId = this.selectedItem;
      this.$validator.validateAll().then(result => {
        if (result) {
          this.saving = true;
          this.createCatalog(catalogToCreate).then(() => {
            this.closeModal();
            this.sucessToaster({
              icon: 'building-o',
              text: 'Your new schedule has been created.',
            });
          }).finally(() => {
            this.saving = false;
          });
        }
      });
    },
    closeModal() {
      this.$emit('closeModal');
      this.resetCatalog();
    },
    usePricesFrom(value) {
      this.selectedItem = value.toString();
    },
    onInput(key, value) {
      this.updateCatalogProperty({
        key,
        value,
      });
    },
  },
  mounted() {
    this.usePricesFrom(this.catalogs.find(c => c.default).id);
  },
};
</script>

<style scoped lang="scss">
.content {
  padding-top: 1.875rem;
  padding-bottom: 1rem;
}

.title {
  text-align: center;
  color: $label-color;
  font-size: $font-size;
  padding-top: 1.875rem;
  padding-bottom: 1rem;
}

.subtitle {
  color: $subtitle-color;
  font-size: $secondary-font-size;
}

.description {
  width: 100%;
  border-radius: $border-radius;
}

.field:first-of-type {
  min-height: 4.5rem;
  padding-top: 0.3rem;
}

</style>
