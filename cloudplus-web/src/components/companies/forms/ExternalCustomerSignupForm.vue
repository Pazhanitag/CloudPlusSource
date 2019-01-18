<template>
  <external-signup-form :accountType="accountTypes.Customer" :parentId="$props.parentId"></external-signup-form>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import accountTypesConstants from '@/assets/constants/accountTypes';
import ExternalSignupForm from '@/components/companies/forms/ExternalSignupForm';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  components: {
    ExternalSignupForm,
  },
  inject: {
    $validator: '$validator',
  },
  props: ['parentId'],
  data() {
    return {
      accountTypes: accountTypesConstants,
      roles: {
        customerAdmin: {},
        resellerAdmin: {},
      },
    };
  },
  created() {
    this.setAccountType(this.accountTypes.Customer, this.roles.customerAdmin.id);
  },
  computed: {
    ...mapGetters({
      company: 'company/externalSignupForm',
      user: 'user/externalSignupForm',
    }),
  },
  methods: {
    ...mapMutations({
      setCompany: 'company/SET_COMPANY',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
    }),
    setAccountType(accountType, roleId) {
      this.updateCompanyProperty({
        key: 'accountType',
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
  },
};
</script>

<style scoped>

</style>
