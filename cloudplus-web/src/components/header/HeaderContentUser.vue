<template>
  <div class="header-content-user">
    <popper trigger="click" :options="{placement: 'bottom-start'}">
      <div class="popper">
        <div class="dropdown-content">
          <brand-hover>
            <span class="dropdown-item" @click="editProfile">
              Edit my profile
            </span>
          </brand-hover>
          <hr class="dropdown-divider">
          <brand-hover>
            <span class="dropdown-item" @click="logOut">
              Log out
            </span>
          </brand-hover>
        </div>
      </div>
      <div slot="reference" class="cursor-pointer">
        <div class="level">
          <div class="level-left">
            <div class="level-item"><avatar :src="user.profilePicture" :fullName="user.fullName"></avatar></div>
            <div class="level-item">
              <a>{{user.fullName}}</a>
            </div>
            <div class="level-ite">
              <i class="fa fa-angle-down" aria-hidden="true"></i>
            </div>
          </div>
        </div>
      </div>
    </popper>
  </div>
</template>

<script>
import Popper from '@/components/shared/popper/Popper';
import Avatar from '@/components/shared/image/Avatar';
import AuthService from '@/services/authService';

export default {
  props: ['user'],
  components: { Avatar, Popper },
  methods: {
    logOut() {
      AuthService.signOut();
    },
    editProfile() {
      this.$router.push({ path: '/profile/edit' });
    },
  },
};
</script>

<style scoped lang="scss">
.header-content-user {
  display: flex;
  align-items: center;
  font-size: $secondary-font-size;
  margin-top: -0.3rem;
}

@media screen and (max-width: 1600px) {
  .header-content-user {
    margin-top: -0.4rem;
  }
}
</style>
