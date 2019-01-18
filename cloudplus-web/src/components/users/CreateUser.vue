<template>
  <div>
    <component-sticky-header title="Create New User" class="component-min-width">
      <brand-primary-btn v-can-see="['AddUsers']" @click="create" :disabled="creatingUser">Save user profile</brand-primary-btn>
    </component-sticky-header>
    <div class="component-main component-min-width">
      <div class="component-main__white">
        <loading-icon size="2" v-if="isLoading"></loading-icon>
        <div v-else class="columns main-form">
              <div class="column column-padding-right">
                  <brand-section-title>Primary Contact Information</brand-section-title>
                  <user-personal-information></user-personal-information>

                  <brand-section-title>General Information</brand-section-title>
                  <user-general-information :domains="companyDomains"></user-general-information>

                  <brand-section-title>Security</brand-section-title>
                  <user-security-information></user-security-information>

                  <brand-section-title>Account Options</brand-section-title>
                  <user-account-options></user-account-options>
                </div>
                <div class="column column-padding-left">
                  <brand-section-title>User Profile Image</brand-section-title>
                  <user-profile-image :enableCropping="true"></user-profile-image>

                  <brand-section-title>Roles</brand-section-title>
                  <user-roles></user-roles>
             </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Tabs, TabPane } from '@/components/shared/tabs';
import { mapActions, mapMutations, mapGetters } from 'vuex';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import { usStates, canadianStates } from '@/assets/constants/states';
import UserPersonalInformation from './forms/UserPersonalInformation';
import UserGeneralInformation from './forms/UserGeneralInformation';
import UserSecurityInformation from './forms/UserSecurityInformation';
import UserAccountOptions from './forms/UserAccountOptions';
import UserProfileImage from './forms/UserProfileImage';
import UserRoles from './forms/UserRoles';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin, loadingMixin],
  components: {
    ComponentStickyHeader,
    Tabs,
    TabPane,
    UserPersonalInformation,
    UserGeneralInformation,
    UserSecurityInformation,
    UserAccountOptions,
    UserProfileImage,
    UserRoles,
  },
  data() {
    return {
      creatingUser: false,
      usStates,
      canadianStates,
      showWelcomeLetter: true,
    };
  },
  created() {
    this.setUserCompany(this.companyId);
    const that = this;
    this.getCompany(this.companyId).then(response => {
      if (response.status === 200) {
        if (['United States', 'US'].indexOf(response.data.result.country) >= 0) {
          this.updateProperty('state', that.mapStates(usStates).filter(state =>
            state.name === response.data.result.state
            || state.value === response.data.result.state)[0].value);
        } else if (['Canada', 'CA'].indexOf(response.data.result.country) >= 0) {
          this.updateProperty('state', that.mapStates(canadianStates).filter(state =>
            state.name === response.data.result.state
            || state.value === response.data.result.state)[0].value);
        } else {
          this.updateProperty('state', response.data.result.state);
        }
        this.updateProperty('city', response.data.result.city);
        this.updateProperty('zipCode', response.data.result.zipCode);
        this.updateProperty('streetAddress', response.data.result.streetAddress);
        this.updateProperty('country', response.data.result.country);
        this.updateProperty('companyName', response.data.result.name);
        this.updateProperty('phoneNumber', response.data.result.phoneNumber);
      }
    });
  },
  computed: {
    ...mapGetters({
      companyDomains: 'company/companyDomains',
      companyId: 'userAuth/companyId',
      userEmails: 'user/userSecurityInformation',
    }),
  },
  methods: {
    ...mapActions({
      createUser: 'user/createUser',
      getCompanyDomains: 'company/getCompanyDomains',
      getCompany: 'company/getCompany',
    }),
    ...mapMutations({
      setUserCompany: 'user/SET_USER_COMPANY',
      resetUserState: 'user/RESET_USER_STATE',
      setUserDomain: 'user/SET_USER_DOMAIN',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      resetCompanyState: 'company/RESET_COMPANY_STATE',
      resetUnsavedUserChanges: 'user/RESET_UNSAVED_USER_CHANGES',
    }),
    create() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.creatingUser = true;
          this.checkEmailSetup();
          this.createUser().then(() => {
            this.sucessToaster(this.getToasterOptions());
          });
        } else {
          this.validationErrorToaster();
          this.$bus.emit('changeFocus');
          this.$bus.emit('fieldFocus');
        }
      });
    },
    checkEmailSetup() {
      if (
        this.userEmails.passwordSetupMethod === 2 &&
        !this.userEmails.sendPlainPasswordViaEmail
      ) {
        this.updateUserProperty({
          key: 'passwordSetupEmail',
          value: null,
        });
        this.updateUserProperty({
          key: 'passwordSetupEmailRetyped',
          value: null,
        });
      }
    },
    getToasterOptions() {
      return {
        text: 'Your user will be created shortly. Do you want to create new one or do you want to go to user list?',
        icon: 'user',
        duration: 5000,
        action: [
          {
            text: 'New user',
            onClick: () => {
              window.location.reload();
            },
          },
          {
            text: 'User list',
            push: {
              path: '/users',
            },
          },
        ],
        onComplete: () => {
          this.$router.push({ path: '/users' });
        },
      };
    },
    updateProperty(key, value) {
      this.updateUserProperty({
        key,
        value,
      });
    },
    mapStates(states) {
      return states.map(state => ({
        name: state.name,
        value: state.abbreviation,
      }));
    },
  },
  mounted() {
    this.getCompanyDomains(this.companyId).then(() => {
      this.setUserDomain(this.companyDomains[0].name);
      this.isLoading = false;
    });
  },
  beforeRouteLeave(to, from, next) {
    this.$bus.off();
    this.resetUserState();
    this.resetUnsavedUserChanges();
    this.resetCompanyState();
    next();
  },
};
</script>

<style scoped lang='scss'>
.main-form {
  margin-top: -3rem;
}
.component-min-width{
  min-width: 30rem;
}
@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .component-main__white{
    height: 93rem;
  }
}
</style>
