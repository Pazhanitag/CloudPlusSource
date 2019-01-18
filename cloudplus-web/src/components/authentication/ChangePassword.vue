<template>
  <div>
    <cloud-plus-card>
      <div v-show="sendingResetPasswordStatus !== status.Success">
        <div>
          <div>Please choose password for your account. After that, you will be able to log in.</div>
          <div class="separator-30"></div>
          <cloud-plus-password-textfield data-vv-name="Password" v-validate="'required|min:6'" v-model="newPassword" :validationErrors="errors.first('Password')">
            New Password
          </cloud-plus-password-textfield>
          <cloud-plus-password-textfield data-vv-name="Re-type Password" v-validate="`retyped_password:${newPassword}|required`" v-model="confirmNewPassword" :validationErrors="errors.first('Re-type Password')">
            Re-type Password
          </cloud-plus-password-textfield>
          <div class="separator-50">
            <brand-spinner v-show="sendingResetPasswordStatus == status.Saving"></brand-spinner>
            <brand-error-message v-show="sendingResetPasswordStatus == status.Error">
              Something went wrong with reseting password. Please try again, or contact the administrator.
            </brand-error-message>
          </div>
          <div class="is-pulled-right">
            <button class="button is-info" @click="updatePassword" :disabled="errors.any() || sendingResetPasswordStatus == status.Saving">Update Password</button>
          </div>
        </div>
      </div>
      <reset-password-success v-show="sendingResetPasswordStatus === status.Success"></reset-password-success>
      <template slot="footer"> Problems with your account, don't have access to those email addresses?
        <a href="">Contact us!</a>
      </template>
    </cloud-plus-card>
  </div>
</template>

<script>
import CloudPlusCard from '@/components/shared/CloudPlusCard';
import userUtilitiesService from '@/services/userUtilitiesService';
import CloudPlusPasswordTextfield from '@/components/shared/input-components/CloudPlusPasswordTextfield';
import ResetPasswordSuccess from './ResetPasswordSuccess';
import statusConstants from '../../assets/constants/status';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  components: { ResetPasswordSuccess, CloudPlusCard, CloudPlusPasswordTextfield },
  data() {
    return {
      newPassword: '',
      confirmNewPassword: '',
      token: this.$route.query.token,
      userId: this.$route.query.userId,
      sendingResetPasswordStatus: statusConstants.Inaction,
      status: statusConstants,
    };
  },
  methods: {
    redirect() {
      this.$router.push('/');
    },
    updatePassword() {
      this.$validator.validateAll().then(result => {
        if (!result) return result;

        this.sendingResetPasswordStatus = this.status.Saving;

        const requestData = {
          UserId: this.userId,
          Token: this.token,
          Password: this.newPassword,
        };

        userUtilitiesService.updatePassword(requestData).then(response => {
          if (!response.data.errorMessage || response.status === 200) {
            this.sendingResetPasswordStatus = this.status.Success;
          }
        }).catch(() => {
          this.sendingResetPasswordStatus = this.status.Error;
        });

        return result;
      });
    },
  },
};

</script>

<style>

</style>
