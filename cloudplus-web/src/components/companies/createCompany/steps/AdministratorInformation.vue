<template>
  <div class="columns" v-scroll-top>
    <div class="column column-padding-right">
      <brand-section-title>Primary Contact Information</brand-section-title>
      <user-personal-information></user-personal-information>

      <brand-section-title>General Information</brand-section-title>
      <user-general-information :domains="domains"></user-general-information>

      <brand-section-title>Security</brand-section-title>
      <user-security-information></user-security-information>
    </div>
    <div class="column column-padding-left">
      <brand-section-title>User Profile Image</brand-section-title>
      <user-profile-image :enableCropping="true"></user-profile-image>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import UserPersonalInformation from '@/components/users/forms/UserPersonalInformation';
import UserGeneralInformation from '@/components/users/forms/UserGeneralInformation';
import UserSecurityInformation from '@/components/users/forms/UserSecurityInformation';
import UserProfileImage from '@/components/users/forms/UserProfileImage';

export default {
  inject: {
    $validator: '$validator',
  },
  components: {
    UserPersonalInformation,
    UserGeneralInformation,
    UserSecurityInformation,
    UserProfileImage,
  },
  computed: {
    ...mapGetters({
      domains: 'company/companyDomains',
    }),
  },
};
</script>

<style>

</style>
