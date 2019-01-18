<template>
  <edit-company :id="userProfile.companyId"></edit-company>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import EditCompany from './EditCompany';

export default {
  components: {
    EditCompany,
  },
  methods: {
    ...mapMutations({
      resetCompanyState: 'company/RESET_COMPANY_STATE',
    }),
  },
  computed: {
    ...mapGetters({
      userProfile: 'userAuth/userProfile',
    }),
  },
  beforeRouteLeave(to, from, next) {
    this.resetCompanyState();
    next();
  },
};
</script>

<style>

</style>
