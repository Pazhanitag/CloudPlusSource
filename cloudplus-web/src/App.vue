<template>
  <div v-if="userProfile.email || showApp" id="app">
    <div class="fixed-header">
      <header-notification v-show="!showApp && impersonated"></header-notification>
      <header-block v-show="!showApp"></header-block>
    </div>
    <scrollable-content-wrapper :notificationVisible="impersonated" :windowHeight="windowHeight" :menuVisible="!showApp" v-if="!isExternalSignupForm">
      <main-menu v-show="!showApp"></main-menu>
      <div class="main-content" v-scrolled>
        <router-view></router-view>
      </div>
    </scrollable-content-wrapper>
    <!-- If it's external signup form, there should be no custom scroller on the side -->
    <div class="main-content main-content-hidden-overflow" v-else>
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import 'bulma/css/bulma.css';
import 'font-awesome/css/font-awesome.css';
import { mapGetters } from 'vuex';
import Config from 'appConfig';
import MainMenu from './components/menus/MainMenu';
import HeaderBlock from './components/header/Header';
import HeaderNotification from './components/header/HeaderNotification';
import ScrollableContentWrapper from './components/shared/styled-components/brand-wrappers/ScrollableContentWrapper';
import './assets/fonts/icon-fonts.css';

export default {
  components: {
    MainMenu,
    HeaderBlock,
    HeaderNotification,
    ScrollableContentWrapper,
  },
  data() {
    return {
      windowHeight: 0,
      fontSize: 0,
    };
  },
  computed: {
    ...mapGetters({
      userProfile: 'userAuth/userProfile',
    }),
    impersonated() {
      return this.userProfile.parentUserId !== undefined;
    },
    showApp() {
      return this.isForgotPassword || this.isChangePassword || this.isExternalSignupForm;
    },
    isForgotPassword() {
      return this.$route.name === Config.authConfig.forgotPasswordRoute;
    },
    isChangePassword() {
      return this.$route.name === Config.authConfig.changePasswordRoute;
    },
    isExternalSignupForm() {
      return this.$route.name === Config.externalResellerSignupForm
      || this.$route.name === Config.externalCustomerSignupForm;
    },
  },
  mounted() {
    this.$nextTick(() => {
      window.addEventListener('resize', this.getFontSize);
      window.addEventListener('resize', this.getWindowHeight);
      this.getFontSize();
      this.getWindowHeight();
    });
  },
  methods: {
    getWindowHeight() {
      this.windowHeight = document.documentElement.clientHeight / this.fontSize;
    },
    getFontSize() {
      const fontSizeWithPx = window.getComputedStyle(document.body).getPropertyValue('font-size');
      this.fontSize = Number(fontSizeWithPx.substring(0, fontSizeWithPx.length - 2));
    },
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.getFontSize);
    window.removeEventListener('resize', this.getWindowHeight);
  },
};
</script>

<style src="./assets/style/main.scss" lang="scss">

</style>
