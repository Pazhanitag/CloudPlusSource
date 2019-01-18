<template>
  <div>
    <component-sticky-header title="User Details">
      <brand-primary-btn @click="edit" :disabled="updatingProfile">Save user profile</brand-primary-btn>
    </component-sticky-header>
    <div class="component-main">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div class="columns main-form" v-else>
            <div class="column column-padding-right">
              <brand-section-title>Primary Contact Information</brand-section-title>
              <user-personal-information></user-personal-information>

              <brand-section-title>General Information</brand-section-title>
              <user-general-information :disableEmail="true" :domains="companyDomains"></user-general-information>

              <brand-section-title>Security Information</brand-section-title>
              <brand-primary-btn @click="openChangeUserPasswordModal()">Change Password</brand-primary-btn>
            </div>
            <div class="column column-padding-left">
              <brand-section-title>User Profile Image</brand-section-title>
              <user-profile-image :enableCropping="true"></user-profile-image>
            </div>
        </div>
      </div>
    </div>
    <change-password-modal v-if="showModal" :showModal="showModal" :userId="userId" @closeModal="closeModal"> </change-password-modal>
  </div>
</template>

<script>
import { mapActions, mapMutations, mapGetters } from 'vuex';
import toasterMixin from '@/mixins/toaster';
import store from '@/store';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import loadingMixin from '@/mixins/loading';
import UserPersonalInformation from './forms/UserPersonalInformation';
import UserGeneralInformation from './forms/UserGeneralInformation';
import UserProfileImage from './forms/UserProfileImage';
import ChangePasswordModal from './ChangePasswordModal';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin, loadingMixin],
  data() {
    return {
      showModal: false,
      updatingProfile: false,
    };
  },
  components: {
    ComponentStickyHeader,
    UserPersonalInformation,
    UserGeneralInformation,
    UserProfileImage,
    ChangePasswordModal,
  },
  created() {
    this.setUserCompany(this.companyId);
    this.getCompany(this.companyId);
  },
  computed: {
    ...mapGetters({
      companyDomains: 'company/companyDomains',
      companyId: 'userAuth/companyId',
      userId: 'user/userId',
    }),
  },
  methods: {
    ...mapActions({
      updateUserProfile: 'user/updateUserProfile',
      reloadUserProfile: 'userAuth/reloadUserProfile',
      getCompany: 'company/getCompany',
    }),
    ...mapMutations({
      setUserCompany: 'user/SET_USER_COMPANY',
      resetUserState: 'user/RESET_USER_STATE',
      resetCompanyState: 'company/RESET_COMPANY_STATE',
    }),
    edit() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.updatingProfile = true;
          this.updateUserProfile().then(() => {
            this.sucessToaster({
              icon: 'user',
              text: 'Your profile will be updated shortly.',
              onComplete: () => {
                this.updatingProfile = false;
                this.reloadUserProfile();
              },
            });
          });
        } else {
          this.validationErrorToaster();
          this.$bus.emit('changeFocus');
          this.$bus.emit('fieldFocus');
        }
      });
    },
    openChangeUserPasswordModal() {
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
    },
  },
  beforeRouteLeave(to, from, next) {
    this.$bus.off();
    this.resetUserState();
    this.resetCompanyState();
    next();
  },
  mounted() {
    store.dispatch('user/getUserProfile').then(() => {
      this.isLoading = false;
    });
  },
};
</script>

<style scoped lang='scss'>
.upload-avatar-container {
  //In order for User Profile Image section to be aligned with Personal Information section
  height: 10.625rem;
}
.main-form {
  margin-top: -3rem;
}
</style>
