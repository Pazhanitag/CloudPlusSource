<template>
    <div class="columns content account-types">
      <div class="column column-padding-left">
        <div  @click="setAccountType(accountTypes.Customer, roles.customerAdmin.id)" class="account-types__button" :class="{active: company.type === accountTypes.Customer}">
          <div class="account-types__button__content">
            <p class="account-types__button__content__title">Create a Customer Account</p>
            <p class="account-types__button__content__description">
            Create a customer that will consume the services you are offering.
            </p>
          </div>
        </div>
      </div>
      <div class="column column-padding-right">
        <div @click="setAccountType(accountTypes.Reseller, roles.resellerAdmin.id)" class="account-types__button" :class="{active: company.type === accountTypes.Reseller}">
          <div class="account-types__button__content">
            <p class="account-types__button__content__title">Create a Reseller Account</p>
            <p class="account-types__button__content__description">
              Have someone that is going to resell your services?
              Set their account up here.
            </p>
          </div>
        </div>
      </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import accountTypesConstants from '@/assets/constants/accountTypes';
import RoleService from '@/services/roleService';

export default {
  data() {
    return {
      accountTypes: accountTypesConstants,
      roles: {
        customerAdmin: {},
        resellerAdmin: {},
      },
    };
  },
  computed: {
    ...mapGetters({
      company: 'company/generalInformation',
    }),
  },
  methods: {
    ...mapMutations({
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
    }),
    setAccountType(accountType, roleId) {
      this.updateCompanyProperty({
        key: 'type',
        value: accountType,
      });
      this.assignRole(roleId);
    },
    assignRole(roleId) {
      this.updateUserProperty({
        key: 'roles',
        value: [roleId],
      });
    },
    getRoles() {
      const self = this;
      RoleService.getAllRoles().then(response => {
        const roles = response.data.result;
        self.roles.customerAdmin = roles.find(role => role.name === 'CustomerAdmin');
        self.roles.resellerAdmin = roles.find(role => role.name === 'ResellerAdmin');
        self.assignRole(self.company.type === self.accountTypes.Reseller ?
          self.roles.resellerAdmin.id : self.roles.customerAdmin.id);
      });
    },
  },
  created() {
    this.getRoles();
  },
};
</script>

<style lang="scss" scoped>
  .account-types {
    text-align: center;
    height: 11.25rem;
    &__button {
      display: table;
      height: 100%;
      border-radius: 0.25rem;
      border: solid $border-height #dce1ea;
      width: 100%;
      &.active {
        background: rgba(93, 191, 197, 0.1);
        border: solid $border-height rgba(93, 191, 197, 1);
      }
      &:hover {
        cursor: pointer;
      }
      &__content {
        display: table-cell;
        vertical-align: middle;
        color: $label-color;
        &__title {
          font-size: $primary-font-size;
          font-weight: bold;
          color: $label-color;
        }
        &__description {
          font-size: $secondary-font-size;
          color: $subtitle-color;
          padding: 0rem 1.25rem 0rem 1.25rem;
        }
      }
    }
  }
</style>
