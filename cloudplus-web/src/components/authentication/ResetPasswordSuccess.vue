<template>
  <div>
    <div>
      <brand-success-message>Password is set!</brand-success-message>
      <div class="separator-30"></div>
      <div class="has-text-centered">Now you can log in with new password.</div>
      <div class="separator-40"></div>
    </div>
    <div class="control is-pulled-right">
      <button class="button is-info" @click="redirect">Log in</button>
    </div>
  </div>
</template>

<script>
export default {
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
