<template>
  <div class="wrapper" :style="{top: (accountType === this.accountTypes.Reseller ? '92rem' : '82rem')}" >
    <div class="success-border">
      <brand-success-message class="success-message">THANK YOU</brand-success-message>
      <div class="has-text-centered">
        Your account is now being created.<br>
        Please check your inbox for your Welcome Letter…and you can get started
        today!
      </div>
    </div>
  </div>
</template>
<script>
import accountTypesConstants from '@/assets/constants/accountTypes';

export default {
  props: ['accountType'],
  data() {
    return {
      accountTypes: accountTypesConstants,
    };
  },
};
</script>

<style scoped>
.success-message {
  padding: 2rem 1rem 1rem;
  font-size: 10px;
}
.has-text-centered{
  padding: 1rem 3rem 3rem;
}
.success-border{
  z-index: 10000;
  opacity: 1;
  box-shadow: 0 0 0 7px #fff, 0 0 0 5px #a2f0f0, 0 0 0 10px #fff, 0 0 2px 10px #000000;
  background-color: #ffffff;
}
.wrapper{
  padding: 3rem;
  position: absolute;
}
</style>
