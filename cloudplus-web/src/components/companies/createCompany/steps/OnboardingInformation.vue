<template>
  <div class="columns" v-scroll-top>
    <div class="column column-padding-right">
      <brand-section-title>Instructions</brand-section-title>
      <div>
        <cloud-plus-check-box @input="onInput('sendWelcomeLetters', $event)" :value="user.sendWelcomeLetters" :disabled="setPasswordViaEmail">
          <div class="content">
            <p class="checkbox-title">Send welcome letter</p>
            <p class="checkbox-description">Send this letter directly to the new User at your new <strong>{{companyGeneralInformation.type === accountTypes.Customer ? 'Customer' : 'Reseller'}}</strong> that you just created. They will immediately be able to log in as a full admin and start adding services!</p>
          </div>
        </cloud-plus-check-box>
        <p v-if="setPasswordViaEmail" class="checkbox-warning-message">Created account user will receive welcome letter email with chosen password setup option.</p>
        <p  v-if="user.sendWelcomeLetters" class="checkbox-warning-message" > Welcome letter will be sent to: <b>{{welcomeLetterEmail}}</b></p>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import { userPasswordSetupMethod } from '@/assets/constants/commonConstants';
import accountTypesConstants from '@/assets/constants/accountTypes';

export default {
  data() {
    return {
      domains: [],
      setPasswordViaEmail: false,
      welcomeLetterEmail: '',
      accountTypes: accountTypesConstants,
    };
  },
  computed: {
    ...mapGetters({
      userAccount: 'user/userAccountOptions',
      user: 'user/createUser',
      companyContact: 'company/contactInformation',
      companyGeneralInformation: 'company/generalInformation',
    }),
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
    }),
    onInput(key, value) {
      this.updateUserProperty({
        key,
        value,
      });
    },
  },
  created() {
    if (this.user.sendPlainPasswordViaEmail ||
      this.user.passwordSetupMethod ===
      userPasswordSetupMethod.GeneratePasswordViaLink) {
      this.setPasswordViaEmail = true;
      this.onInput('sendWelcomeLetters', true);
      this.welcomeLetterEmail = this.user.passwordSetupEmail;
    } else if (this.user.alternativeEmail !== null && this.user.alternativeEmail !== '') {
      this.welcomeLetterEmail = this.user.alternativeEmail;
    } else {
      this.welcomeLetterEmail = this.companyContact.email;
    }
  },
};
</script>

<style lang="scss" scoped>
  .checkbox-title {
    color: $label-color;
    font-weight: bold;
    margin-bottom: 0rem !important;
  }
  .checkbox-description {
    color: $subtitle-color;
  }
  .checkbox-warning-message {
    padding-left: 2.8rem;
    color: $subtitle-color;
    font-size: $secondary-font-size;
  }
  .columns{
    margin-bottom: 0rem;
  }
</style>

