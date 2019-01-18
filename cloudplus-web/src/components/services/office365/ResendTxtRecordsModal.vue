<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal" :ignoreMinHeight="true">
      <p slot="header">Resend Txt Records</p>
      <section slot="section">
        <cloud-plus-textfield 
          @input="updateResendTxtEmail" 
          v-model="email"
          data-vv-name="email" 
          name="email" 
          v-validate="'required|email'"
          data-vv-as="email" 
          :validationErrors="errors.first('email')">Email
        </cloud-plus-textfield>
      </section>
      <div slot="footer">
        <brand-primary-btn @click="sendTxtRecords" :disabled="errors.any()">Send</brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import { mapMutations, mapActions } from 'vuex';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  components: {
    CloudPlusCardModal,
  },
  props: {
    showModal: {
      type: Boolean,
    },
    companyEmail: {
      type: String,
    },
  },
  data() {
    return {
      email: this.companyEmail,
    };
  },
  methods: {
    ...mapActions({
      resendTxtRecords: 'office365/resendTxtRecords',
    }),
    ...mapMutations({
      setResendTxtRecordsEmail: 'office365/SET_RESEND_TXT_RECORDS_EMAIL',
    }),
    closeModal() {
      this.$emit('closeModal');
    },
    updateResendTxtEmail() {
      this.setResendTxtRecordsEmail(this.email);
    },
    sendTxtRecords() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.resendTxtRecords();
          this.closeModal();
        }
      });
    },
  },
  created() {
    this.setResendTxtRecordsEmail(this.email);
  },
};
</script>

<style scoped lang="scss">
</style>

