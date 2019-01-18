<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal">
      <p slot="header">Change Password</p>
      <section class="security-form" slot="section">
        <user-security-information></user-security-information>
      </section>
      <div slot="footer">
        <brand-primary-btn :disabled="errors.any()" @click="updatePassword()">Change Password</brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import { mixin as clickaway } from 'vue-clickaway';
import { mapMutations, mapActions, mapGetters } from 'vuex';
import toasterMixin from '@/mixins/toaster';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import UserSecurityInformation from './forms/UserSecurityInformation';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [clickaway, toasterMixin],
  components: {
    CloudPlusCardModal,
    UserSecurityInformation,
  },
  props: {
    showModal: {
      type: Boolean,
    },
    userId: {
      type: Number,
    },
    emailList: {
      type: Object,
    },
    isMultipleUser: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
    };
  },
  computed: {
    ...mapGetters({
      userEmails: 'user/userSecurityInformation',
    }),
  },
  methods: {
    ...mapMutations({
      resetSecurityInformations: 'user/RESET_SECURITY_STATE',
      setSelectedUserId: 'user/SET_SELECTED_USER_ID',
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      resetUser: 'user/RESET_USER_STATE',
    }),
    ...mapActions({
      changePassword: 'user/changePassword',
    }),
    closeModal() {
      this.$emit('closeModal');
      this.resetSecurityInformations();
      this.resetUser();
    },
    updatePassword() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.checkEmailSetup();
          this.setSelectedUserId(this.userId);
          this.changePassword().then(() => {
            this.closeModal();
            this.sucessToaster({
              icon: 'lock',
              text: 'Your password will be updated shortly.',
            });
          });
        }
      });
    },
    checkEmailSetup() {
      if (this.userEmails.passwordSetupMethod === 2 && !this.userEmails.sendPlainPasswordViaEmail) {
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
  },
  created() {
    if (this.isMultipleUser === true) {
      const [userName, domain] = this.emailList.primaryEmailAddress.split('@');
      this.updateUserProperty({
        key: 'userName',
        value: userName,
      });
      this.updateUserProperty({
        key: 'domain',
        value: domain,
      });
      this.updateUserProperty({
        key: 'alternativeEmail',
        value: this.emailList.alternativeEmailAddress,
      });
    }
  },
};
</script>

<style scoped lang="scss">
.security-form {
  padding: 10px 10px 10px 10px;
}
</style>
