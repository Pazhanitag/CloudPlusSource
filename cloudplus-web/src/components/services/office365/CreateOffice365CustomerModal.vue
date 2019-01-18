<template>
  <div>
    <cloud-plus-card-modal :closeButtonVisible="!validatingAddress" :showModal="showModal" @closeModal="closeModal" :width="'900px'" :modalContentHeight="'75.23'">
      <p slot="header">Product Configuration</p>
      <section slot="section">
        <div class="create-customer-header">
          <h1 class="create-customer-title"> Please fill in the required data in order to configure this product.</h1>
          <h2 class="create-customer-subtitle">After configuration, you will need to validate the domain.</h2>
        </div>
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div class="columns create-customer-form" v-else>
          <div class="column">
            <cloud-plus-select @input="onInput('domain', $event)" :selected="getPrimaryDomain(company.domains)" :options="company.domains.map(d => d.name)">Company Domain</cloud-plus-select>
            <cloud-plus-textfield
              @input="onInput('firstName', $event)"
              :value="company.firstName"
              data-vv-name="firstName"
              name="firstName"
              v-validate="'required'"
              data-vv-as="first name"
              :validationErrors="errors.first('firstName')">First Name
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onInput('email', $event)"
              :value="company.email"
              data-vv-name="email"
              name="email"
              v-validate="'required|email'"
              data-vv-as="email"
              :validationErrors="errors.first('email')">Email
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onInput('address1', $event)"
              :value="company.address1"
              data-vv-name="address1"
              name="address1"
              v-validate="'required'"
              data-vv-as="address"
              :validationErrors="errors.first('address1')">Address 1
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onInput('city', $event)"
              :value="company.city"
              data-vv-name="city"
              name="city"
              v-validate="'required'"
              :validationErrors="errors.first('city')">City
            </cloud-plus-textfield>


            <cloud-plus-textfield
              @input="onInput('postalCode', $event)"
              :value="company.postalCode"
              data-vv-name="postalCode"
              name="postalCode"
              v-validate="'required'"
              data-vv-as="postal code"
              :validationErrors="errors.first('postalCode')">Zip/Postal Code</cloud-plus-textfield>

          </div>
          <div class="column">
            <cloud-plus-textfield
              @input="onInput('name', $event)"
              :value="company.name"
              data-vv-name="name"
              name="name"
              v-validate="'required'"
              :validationErrors="errors.first('name')">Company Name
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onInput('lastName', $event)"
              :value="company.lastName"
              data-vv-name="lastName"
              name="lastName"
              v-validate="'required'"
              data-vv-as="last name"
              :validationErrors="errors.first('lastName')">Last Name
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onInput('phoneNumber', $event)"
              :value="company.phoneNumber"
              data-vv-name="phoneNumber"
              name="phoneNumber"
              v-validate="'required|phone_number_format_office'"
              data-vv-as="phone number"
              :validationErrors="errors.first('phoneNumber')">Phone Number
            </cloud-plus-textfield>
            <cloud-plus-textfield @input="onInput('address2', $event)" :value="company.address2">Address 2</cloud-plus-textfield>

            <cloud-plus-select
              @input="updateState($event)"
              :options="states.map(c => ({value: c.abbreviation, name: c.name}))"
              :selected="company.state">State/Province
            </cloud-plus-select>

            <cloud-plus-select
              @input="updateCountry($event)"
              :options="countries.map(c => c.name)"
              :selected="company.country"
              :icon="'fa-globe'">Country
            </cloud-plus-select>

          </div>
        </div>
        <div class="address-error" v-show="!addressIsValid">
          Address and phone number details seems to be wrong, please correct entries and try again.
        </div>
      </section>
      <div slot="footer">
        <brand-primary-btn :disabled="errors.any()" @click="saveCustomer">
          Save
          <loading-icon :inline="true" v-show="validatingAddress"></loading-icon>
        </brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from 'vuex';
import loadingMixin from '@/mixins/loading';
import Countries from '@/assets/constants/countries';
import { usStates } from '@/assets/constants/states';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [loadingMixin],
  components: {
    CloudPlusCardModal,
  },
  props: {
    showModal: {
      type: Boolean,
    },
  },
  data() {
    return {
      countries: Countries,
      states: usStates,
      addressIsValid: true,
      validatingAddress: false,
    };
  },
  computed: {
    ...mapGetters({
      company: 'office365/companyForCustomerConfiguration',
      companyId: 'userAuth/companyId',
      userProfile: 'userAuth/userProfile',
    }),
  },
  methods: {
    ...mapActions({
      getCompany: 'office365/getCompany',
      validateCustomerAddress: 'office365/validateCustomerAddress',
      createOffice365Customer: 'office365/createCustomer',
    }),
    ...mapMutations({
      updateCustomerProperty: 'office365/UPDATE_CUSTOMER_PROPERTY',
      resetCustomerState: 'office365/RESET_CUSTOMER_STATE',
    }),
    closeModal() {
      this.$emit('closeModal');
      this.resetCustomerState();
    },
    onInput(key, value) {
      this.updateCustomerProperty({
        key,
        value,
      });
    },
    updateCountry(countryName) {
      this.updateCustomerProperty({
        key: 'country',
        value: countryName,
      });
      this.updateCustomerProperty({
        key: 'countryCode',
        value: this.countries.find(c => c.name === countryName).code,
      });
    },
    updateState(abbreviation) {
      this.updateCustomerProperty({
        key: 'state',
        value: this.states.find(s => s.abbreviation === abbreviation).name,
      });
      this.updateCustomerProperty({
        key: 'stateCode',
        value: abbreviation,
      });
    },
    saveCustomer() {
      this.addressIsValid = true;
      this.$validator.validateAll().then(result => {
        if (result) {
          this.validatingAddress = true;
          this.validateCustomerAddress().then(response => {
            this.validatingAddress = false;
            if (response.data.result.isAddressValid) {
              this.createOffice365Customer().then(() => {
                this.$emit('provisioningStarted');
                this.closeModal();
              });
            } else {
              this.addressIsValid = false;
            }
          });
        }
      });
    },
    getPrimaryDomain(domains) {
      return domains.find(dom => dom.isPrimary === true);
    },
  },
  mounted() {
    this.updateCustomerProperty({
      key: 'firstName',
      value: this.userProfile.firstName,
    });
    this.updateCustomerProperty({
      key: 'lastName',
      value: this.userProfile.lastName,
    });
    this.updateCustomerProperty({
      key: 'companyId',
      value: this.userProfile.companyId,
    });
    this.getCompany(this.companyId).then(() => {
      this.updateCustomerProperty({
        key: 'countryCode',
        value: this.countries.find(c => c.name === this.company.country).code,
      });
      const customerState = this.states.find(s => s.name === this.company.state
      || s.abbreviation === this.company.state);
      if (customerState !== undefined) {
        this.updateCustomerProperty({
          key: 'stateCode',
          value: customerState.abbreviation,
        });
      } else {
        this.updateCustomerProperty({
          key: 'stateCode',
          value: this.states[0].abbreviation,
        });
      }
      this.isLoading = false;
    });
  },
};
</script>

<style scoped lang="scss">
  .create-customer-header {
    margin: 2rem;
  }

  .create-customer-title {
    text-align: center;
    color: $label-color;
    font-size: $font-size;
  }

  .create-customer-subtitle {
    text-align: center;
    color: $subtitle-color;
    font-size: $secondary-font-size;
  }

  .create-customer-form {
    padding: 1rem;
  }

  .address-error {
    color: $danger-color;
    font-size: $secondary-font-size;
    padding-left: 1rem;
  }
</style>

