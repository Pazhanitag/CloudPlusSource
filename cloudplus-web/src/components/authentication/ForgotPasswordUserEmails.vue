<template>
  <div>
    <div>Please choose an email you want to use for password recovery:</div>
    <div class="separator-30"></div>
    <div>
      <cloud-plus-radio-btn name="recovery_email" :checked="userEmails.email == selectedEmail"
      :checkedValue="userEmails.email" v-model="email" @input="onInput">{{userEmails.email}}
      </cloud-plus-radio-btn>
    </div>
    <div>
      <cloud-plus-radio-btn name="recovery_email" v-if="userEmails.alternativeEmail && (userEmails.alternativeEmail != userEmails.email) " :checked="userEmails.alternativeEmail == selectedEmail"
      :checkedValue="userEmails.alternativeEmail" v-model="email" @input="onInput">{{userEmails.alternativeEmail}}
      </cloud-plus-radio-btn>
    </div>
  </div>
</template>

<script>
export default {
  props: ['userEmails', 'selectedEmail'],
  data() {
    return {
      email: this.selectedEmail,
    };
  },
  methods: {
    onInput() {
      this.$emit('input', this.email);
    },
  },
};
</script>

<style scoped>

</style>

