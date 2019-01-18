<template>
  <div>
    <cloud-plus-card :email="email">
      <forgot-password-send-email-success v-if="sendingPasswordRecoveryStatus == status.Success" :email="selectedEmail"></forgot-password-send-email-success>
      <div v-else>
        <div v-if="loadingUserEmailsStatus == status.Loading">
          <brand-spinner></brand-spinner>
        </div>
        <forgot-password-user-emails v-show="loadingUserEmailsStatus == status.Success" v-model="selectedEmail" :selectedEmail="selectedEmail" :userEmails="userEmails"></forgot-password-user-emails>
        <forgot-password-input-email v-show="loadingUserEmailsStatus == status.Error" v-model="selectedEmail">
          Email
        </forgot-password-input-email>
        <div class="separator-40">
          <brand-spinner v-show="sendingPasswordRecoveryStatus == status.Saving"></brand-spinner>
          <brand-error-message v-show="sendingPasswordRecoveryStatus == status.Error">
            Something went wrong with your reset link building. Please try again, or contact the administrator.
          </brand-error-message>
        </div>
        <div class="control is-pulled-right" v-show="loadingUserEmailsStatus != status.Loading">
          <button class="button is-info" @click="goBack">Back</button>
          <button class="button is-info"  v-if="loadingUserEmailsStatus != status.Success" @click="send">Next</button>
          <button class="button is-info"  v-else @click="send">Send</button>
        </div>
      </div>
      <template slot="footer"> </template>
    </cloud-plus-card>
  </div>
</template>

<script>
import userUtilitiesService from '@/services/userUtilitiesService';
import CloudPlusCard from '@/components/shared/CloudPlusCard';
import ForgotPasswordSendEmailSuccess from './ForgotPasswordSendEmailSuccess';
import ForgotPasswordUserEmails from './ForgotPasswordUserEmails';
import ForgotPasswordInputEmail from './ForgotPasswordInputEmail';
import statusConstants from '../../assets/constants/status';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  components: {
    ForgotPasswordSendEmailSuccess,
    ForgotPasswordUserEmails,
    ForgotPasswordInputEmail,
    CloudPlusCard,
  },
  props: ['email'],
  data() {
    return {
      selectedEmail: this.email,
      loadingUserEmailsStatus: statusConstants.Loading,
      sendingPasswordRecoveryStatus: statusConstants.Inaction,
      status: statusConstants,
      userEmails: [],
    };
  },
  watch: {
    userEmails() {
      if (this.userEmails.email !== undefined ||
        this.userEmails.alternativeEmail !== undefined) {
        this.loadingUserEmailsStatus = this.status.Success;
      } else {
        this.loadingUserEmailsStatus = this.status.Error;
      }
    },
  },
  methods: {
    redirect() {
      this.$router.push('/');
    },
    send() {
      this.$validator.validateAll().then(result => {
        if (!result) return result;
        userUtilitiesService.getUserEmails(this.selectedEmail).then(response => {
          if (response) {
            this.userEmails = response;
            this.$router.push({ query: { email: `${this.selectedEmail}` } });
          }
        }).catch(() => {
          this.loadingUserEmailsStatus = this.status.Error;
        });
        if (this.loadingUserEmailsStatus === this.status.Success) {
          this.sendingPasswordRecoveryStatus = this.status.Saving;
          userUtilitiesService
            .sendForgotPasswordEmail(this.userEmails.id, this.selectedEmail, this.userEmails.email)
            .then(response => {
              if (response.status === 200) {
                this.sendingPasswordRecoveryStatus = this.status.Success;
              }
            }).catch(() => {
              this.sendingPasswordRecoveryStatus = this.status.Error;
            });
        }
        return result;
      });
    },
    goBack() {
      this.$router.push({ path: '/' });
      // HACK: In order to trigger router.onReady we need to reload the page.
      // Try and find better solution. Kljuco 11/22/2017
      window.location.reload();
    },
  },
  created() {
    if (this.email) {
      userUtilitiesService.getUserEmails(this.email).then(result => {
        this.userEmails = result;
      }).catch(() => {
        this.loadingUserEmailsStatus = this.status.Error;
      });
    } else {
      this.loadingUserEmailsStatus = this.status.Error;
    }
  },
};
</script>

<style scoped>

</style>
