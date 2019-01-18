<template>
  <div>
    <brand-tabs>
      <tabs>
        <tab-pane label="Service Assignment">
          <loading-icon size="2" v-if="licencesAreLoading"></loading-icon>
          <div v-else v-for="(productItem, key) in mainProductItems" :key="key" class="cursor-pointer" @click="toggleProduct(productItem)">
            <div class="level service" :class="{'last-child' : checkIfLastItem(mainProductItems, key)}">
              <div class="level-item">
                <span class="service-name" :title="productItem.name">{{productItem.name}}</span>
              </div>
              <div class="level-right">
                <div class="level-item">
                  <cloud-plus-switch :checked="selectedProduct === productItem.identifier" @input="toggleProduct(productItem)" v-on:click.stop class="product-overview-switch-component"></cloud-plus-switch>
                </div>
              </div>
            </div>
            <!-- <div class="level service__addon" v-for="(addon, key) in addons" :key="key">
              <div class="level-item">
                <span class="service-name">{{addon.name}}</span>
              </div>
              <div class="level-right">
                <div class="level-item">
                  <cloud-plus-switch v-on:click.stop class="product-overview-switch-component"></cloud-plus-switch>
                </div>
              </div>
            </div> -->
          </div>
        </tab-pane>
        <tab-pane label="Role Assignment">
          <loading-icon size="2" v-if="rolesAreLoading"></loading-icon>
          <div v-else v-for="(role, key) in roles" :key="key" class="cursor-pointer" @click="toogleRole(role.name)">
            <div class="level role" :class="{'last-child' : checkIfLastItem(roles, key)}">
              <div class="level-item">
                <span class="role-name">{{role.displayName}}</span>
              </div>
              <div class="level-right">
                <div class="level-item">
                  <cloud-plus-check-box :value="isSelected(role.name)"></cloud-plus-check-box>
                </div>
              </div>
            </div>
          </div>
        </tab-pane>
        <tab-pane v-if="showCredentials" label="Credentials" class="credentials-tab">
          <div class="field is-horizontal">
            <div class="field-label">
              <label class="password-label">Password</label>
            </div>
            <div class="field-body">
              <cloud-plus-password-textfield
                @input="emitPassword"
                v-model="password"
                v-validate="'required|min:8|max:16|office365_password'"
                data-vv-name="password"
                :validationErrors="errors.first('password')">
              </cloud-plus-password-textfield>
              <i class="fa fa-refresh random-password" @click="generatePassword" title="Random password"></i>
            </div>
          </div>
          <div class="field is-horizontal">
            <div class="field-label">
              <label>Confirm Password</label>
            </div>
            <div class="field-body">
              <cloud-plus-password-textfield
                @input="emitPassword"
                v-model="passwordRetyped"
                v-validate="`retyped_password:${password}|required|min:8|max:16|office365_password`"
                data-vv-name="retypedPassword"
                data-vv-as="retype password"
                :validationErrors="errors.first('retypedPassword')">
              </cloud-plus-password-textfield>
            </div>
          </div>
        </tab-pane>
      </tabs>
    </brand-tabs>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import PasswordGenerator from 'generate-password';
import office365CustomersService from '@/services/office365CustomersService';
import { Tabs, TabPane } from '@/components/shared/tabs';
import CloudPlusSwitch from '@/components/shared/input-components/CloudPlusSwitch';
import CloudPlusPasswordTextfield from '@/components/shared/input-components/CloudPlusPasswordTextfield';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  props: {
    assignedRoles: {
      type: Array,
    },
    assignedLicence: {
      type: String,
    },
    assignedRolesLoaded: {
      type: Boolean,
      default: true,
    },
    assignedLicenceLoaded: {
      type: Boolean,
      default: true,
    },
    showCredentials: {
      type: Boolean,
      default: false,
    },
  },
  components: {
    Tabs,
    TabPane,
    CloudPlusSwitch,
    CloudPlusPasswordTextfield,
  },
  data() {
    return {
      roles: [],
      selectedProduct: '',
      selectedRoles: [],
      availableRolesLoaded: false,
      password: '',
      passwordRetyped: '',
    };
  },
  computed: {
    ...mapGetters({
      product: 'product/selectedProduct',
      clearSelectedUsers: 'office365/clearMultiuserLicenceForms',
    }),
    rolesAreLoading() {
      return !this.assignedRolesLoaded || !this.availableRolesLoaded;
    },
    licencesAreLoading() {
      return !this.assignedLicenceLoaded;
    },
    mainProductItems() {
      return this.productItems(false);
    },
    addons() {
      return this.productItems(true);
    },
  },
  methods: {
    ...mapMutations({
      setSelectedProductName: 'office365/SET_MULTI_USER_PRODUCT_NAME',
    }),
    getRoles() {
      this.availableRolesLoaded = false;
      office365CustomersService.getRoles().then(response => {
        this.availableRolesLoaded = true;
        this.roles = response.data.result;
      });
    },
    productItems(isAddon) {
      return this.product ? this.product.productItems.filter(item => item.isAddon === isAddon) : [];
    },
    toggleProduct(product) {
      if (this.selectedProduct === product.identifier) {
        this.selectedProduct = '';
        this.setSelectedProductName('');
      } else {
        this.selectedProduct = product.identifier;
        this.setSelectedProductName(product.name);
      }
      this.$emit('productSelected', this.selectedProduct);
    },
    toogleRole(roleName) {
      const toggledRoleIndex = this.selectedRoles.indexOf(roleName);
      if (toggledRoleIndex > -1) {
        this.selectedRoles.splice(toggledRoleIndex, 1);
      } else {
        this.selectedRoles.push(roleName);
      }
      this.$emit('roleSelected', this.selectedRoles);
    },
    generatePassword() {
      const password = PasswordGenerator.generate({
        length: 8,
        numbers: true,
        uppercase: true,
        strict: true,
      });
      this.password = password;
      this.passwordRetyped = password;
      this.emitPassword();
    },
    isSelected(role) {
      return this.selectedRoles.includes(role);
    },
    emitPassword() {
      if (this.passwordRetyped !== '' && this.password !== '') {
        this.$validator.validate('password', this.password).then(passwordValidated => {
          if (passwordValidated && this.password === this.passwordRetyped) {
            this.$emit('passwordConfirmed', this.password);
          } else {
            this.$emit('invalidPassword');
          }
        });
      }
    },
    checkIfLastItem(items, key) {
      return key === items.length - 1;
    },
  },
  watch: {
    clearSelectedUsers() {
      if (this.clearSelectedUsers) {
        this.selectedRoles = [];
        this.selectedProduct = '';
      }
    },
    assignedRoles() {
      this.selectedRoles = this.assignedRoles.slice();
    },
    assignedLicence() {
      this.selectedProduct = this.assignedLicence;
    },
  },
  created() {
    this.getRoles();
  },
};
</script>

<style lang="scss" scoped>
  .level {
    margin: 0rem;
  }

  .service {
    // background: #f2f2f2;
    border-bottom: 1px solid #f2f2f2;
    padding: 1rem 1rem 1rem  1rem;
    &__addon {
      padding-left: 2rem;
      border-bottom: 1px solid #f2f2f2;
    }
  }

  .service-name {
    font-size: $secondary-font-size;
    width: 320px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    font-size: .875rem;
  }

  .role {
    border-bottom: 1px solid #f2f2f2;
    padding: 1rem 0rem 1rem  1rem;
  }
  .last-child{
    border-bottom: 0px solid #f2f2f2;
  }
  .role-name {
    width: 100%;
    font-size: $secondary-font-size;
  }

  .product-overview-switch-component {
    display: inline-block;
    font-weight: 600;
  }

  .password-label {
    line-height: 3rem;
  }

  .random-password {
    top: 1.2rem;
    position: relative;
    cursor: pointer;
  }

  .credentials-tab {
    padding-right: 1.5rem;
  }

  .field-label {
    font-size: 0.875rem;
    display: flex;
    align-items: center;
    text-align: left;
    padding-left: 1rem;
  }
</style>
