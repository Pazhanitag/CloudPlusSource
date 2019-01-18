<template>
  <div>
    <div>
      <div>Password recovery email was sent to this address:</div>
      <div class="separator-30"></div>
      <strong>{{email}}</strong>
      <div class="separator-30"></div>
      <div>Please check your email and follow instructions. After setting new password, you will be able to log in.</div>
      <div class="separator-40"></div>
    </div>
    <div class="control is-pulled-right">
      <button class="button is-info" @click="redirect">Log in</button>
    </div>
  </div>
</template>

<script>
export default {
  props: ['email'],
  methods: {
    redirect() {
      this.$router.push('/');
      // [Kljuco 12/5/2017]: This is hack so it would reload a page. We should look into this later.
      window.location.reload();
    },
  },
};
</script>

<style scoped>

</style>
