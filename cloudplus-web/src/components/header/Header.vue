<template>
  <header class="header">
    <div class="header__logo">
      <img :src="branding.logoUrl !== '' ? branding.logoUrl : imagePlaceholder" @click="goHome()">
    </div>
    <div class="header__content">
      <!-- <header-content-notification icon="envelope-o" :count="messagesCount"></header-content-notification>
      <header-content-notification icon="bell-o" :count="notificationsCount"></header-content-notification> -->
      <header-content-user :user="user"></header-content-user>
    </div>
  </header>
</template>

<script>
import { mapGetters } from 'vuex';
import appConfig from 'appConfig';
import HeaderContentNotification from './HeaderContentNotification';
import HeaderContentUser from './HeaderContentUser';

export default {
  components: { HeaderContentNotification, HeaderContentUser },
  data() {
    return {
      infoMessage: 'You are currently logged as an administrator to manage customer details. You can simply go back by choosing where you want to go next.',
      messagesCount: 2,
      notificationsCount: 3,
      imagePlaceholder: appConfig.imagePlaceholder,
    };
  },
  computed: {
    ...mapGetters({
      user: 'userAuth/userProfile',
      branding: 'branding/brand',
    }),
  },
  methods: {
    goHome() {
      this.$router.push('/');
    },
  },
};
</script>

<style lang="scss">
.header {
  height: 5rem;
  background-color: white;
  border-bottom: $border-height solid #f2f7f8;
  display: flex;
  flex-flow: row wrap;
  align-items: center;
  padding: 0.93rem 2.5rem;
  &__logo {
    flex: 1;
    img {
      height: 3.187rem;
    }
    cursor: pointer;
  }
  &__content {
    display: flex;
    flex: 2;
    justify-content: flex-end;
    align-items: center;
    &__user {
      display: flex;
      align-items: center;
    }
  }
}
</style>
