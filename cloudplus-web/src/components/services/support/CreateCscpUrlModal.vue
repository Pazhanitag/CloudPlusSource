<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal" :width="'900px'" :modalContentHeight="'75.23'">
      <p slot="header">Setting up the Custom Control Panel URL</p>
      <section slot="section">
        <div class="create-customer-header">
          <h1 class="create-customer-title"> Please fill in the required data in order to configure Custom Control Panel URL.</h1>
          <h2 class="create-customer-subtitle">You are about to activate the Custom Control Panel URL for your control panel. Once activated, it is immediately billable, as we will purchase the necessary IP address and generate the CSR with the information provided here. If you need assistance, we are here to help!
IMPORTANT: The information below must match the information in the registrar (ie where you purchased the domain).</h2>
        </div>
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div class="columns create-customer-form" v-else>
          <div class="column">
            <cloud-plus-textfield
              @input="updateCscpUrl($event)"
              :value="cscpDetailsForConfiguration.cscpUrl"
              data-vv-name="cscpUrl"
              name="cscpUrl"
              v-validate="'required|verify_domain'"
              data-vv-as="Custom Control Panel URL"
              :validationErrors="errors.first('cscpUrl')">Custom Control Panel URL
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onUpdateCompany('address1', $event)"
              :value="cscpDetailsForConfiguration.address1"
              data-vv-name="address1"
              name="address1"
              v-validate="'required'"
              data-vv-as="address"
              :validationErrors="errors.first('address1')">Company Address - Street
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onUpdateCompany('city', $event)"
              :value="cscpDetailsForConfiguration.city"
              data-vv-name="city"
              name="city"
              v-validate="'required'"
              :validationErrors="errors.first('city')">Company Address - City
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onUpdateCompany('postalCode', $event)"
              :value="cscpDetailsForConfiguration.postalCode"
              data-vv-name="postalCode"
              name="postalCode"
              v-validate="'required'"
              data-vv-as="postal code"
              :validationErrors="errors.first('postalCode')">Company Address - Zip/Postal Code
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onUpdateReseller('contactPhone', $event)"
              :value="cscpDetailsForConfiguration.contactPhone"
              data-vv-name="contactPhone"
              name="contactPhone"
              v-validate="'required|phone_number_format_office'"
              data-vv-as="phone number"
              :validationErrors="errors.first('contactPhone')">Contact Phone
            </cloud-plus-textfield>
          </div>
          <div class="column">
            <cloud-plus-textfield
              @input="onUpdateCompany('name', $event)"
              :value="cscpDetailsForConfiguration.name"
              data-vv-name="name"
              name="name"
              v-validate="'required'"
              :validationErrors="errors.first('name')">Company Name
            </cloud-plus-textfield>
            <cloud-plus-select
              @input="updateState($event)"
              :options="states.map(c => ({value: c.abbreviation, name: c.name}))"
              :selected="cscpDetailsForConfiguration.state">Company Address -  State/Province
            </cloud-plus-select>
            <cloud-plus-select
              @input="updateCountry($event)"
              :options="countries.map(c => c.name)"
              :selected="cscpDetailsForConfiguration.country"
              :icon="'fa-globe'">Company Address - Country
            </cloud-plus-select>
            <cloud-plus-textfield
              @input="onUpdateReseller('contactPerson', $event)"
              :value="cscpDetailsForConfiguration.contactPerson"
              data-vv-name="contactPerson"
              name="contactPerson"
              v-validate="'required'"
              :validationErrors="errors.first('contactPerson')">Contact Person
            </cloud-plus-textfield>
            <cloud-plus-textfield
              @input="onUpdateReseller('email', $event)"
              :value="cscpDetailsForConfiguration.email"
              data-vv-name="email"
              name="email"
              v-validate="'required|email'"
              :validationErrors="errors.first('email')">Contact Email
            </cloud-plus-textfield>
          </div>
        </div>
      </section>
      <div slot="footer">
        <brand-primary-btn :disabled="errors.any()" @click="saveCustomer">
          Save
        </brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from 'vuex';
import loadingMixin from '@/mixins/loading';
import toasterMixin from '@/mixins/toaster';
import Countries from '@/assets/constants/countries';
import { usStates } from '@/assets/constants/states';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [loadingMixin, toasterMixin],
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
    };
  },
  computed: {
    ...mapGetters({
      companyId: 'userAuth/companyId',
      userProfile: 'userAuth/userProfile',
      cscpCompany: 'cscpURL/company',
      reseller: 'cscpURL/reseller',
      cscpUrl: 'cscpURL/cscpUrl',
      cscpDetailsForConfiguration: 'cscpURL/cscpDetailsForConfiguration',
    }),
  },
  methods: {
    ...mapActions({
      getcscpCompany: 'cscpURL/getCompany',
      createCscpUrl: 'cscpURL/createCscpUrl',
    }),
    ...mapMutations({
      updateReseller: 'cscpURL/UPDATE_RESELLER',
      updateCscpurl: 'cscpURL/SET_CSCPURL',
      updateCompany: 'cscpURL/UPDATE_COMPANY',
      resetCscpDetailsForConfiguration: 'cscpURL/RESET_CSCP_DETAILS_FOR_CONFIGURATION',
    }),
    closeModal() {
      this.$emit('closeModal');
      this.resetCscpDetailsForConfiguration();
    },
    onUpdateCompany(key, value) {
      this.updateCompany({
        key,
        value,
      });
    },
    onUpdateReseller(key, value) {
      this.updateReseller({
        key,
        value,
      });
    },
    updateCscpUrl(cscpUrl) {
      this.updateCscpurl(cscpUrl);
    },
    updateCountry(countryName) {
      this.onUpdateCompany(
        'country',
        countryName,
      );
      this.onUpdateCompany(
        'countryCode',
        this.countries.find(c => c.name === countryName).code,
      );
    },
    updateState(abbreviation) {
      this.onUpdateCompany(
        'state',
        this.states.find(s => s.abbreviation === abbreviation).name,
      );
      this.onUpdateCompany(
        'stateCode',
        abbreviation,
      );
    },
    saveCustomer() {
      this.addressIsValid = true;
      this.$validator.validateAll().then(result => {
        if (result) {
          this.createCscpUrl().then(() => {
            this.closeModal();
            this.$router.push({
              name: 'myServices',
              params: { showModalOnOpen: true },
            });
            this.sucessToaster({
              text: 'Custom Control Panel URL is successfully saved',
            });
          });
        }
      });
    },
  },
  mounted() {
    this.onUpdateCompany('id', parseInt(this.companyId, 10));
    this.isLoading = true;
    this.getcscpCompany(this.companyId).then(() => {
      this.updateReseller({ key: 'contactPerson', value: this.userProfile.fullName });
      this.updateReseller({ key: 'contactPhone', value: this.userProfile.phoneNumber });
      this.updateReseller({ key: 'email', value: this.userProfile.email });
      this.onUpdateCompany(
        'countryCode',
        this.countries.find(c => c.name === this.cscpCompany.country).code,
      );
      const customerState = this.states.find(s => s.name === this.cscpCompany.state
      || s.abbreviation === this.cscpCompany.state);
      if (customerState !== undefined) {
        this.onUpdateCompany(
          'stateCode',
          customerState.abbreviation,
        );
      } else {
        this.onUpdateCompany(
          'stateCode',
          this.states[0].abbreviation,
        );
      }
      this.isLoading = false;
    });
    this.updateCscpUrl('my.[accountdomain.com]');
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

