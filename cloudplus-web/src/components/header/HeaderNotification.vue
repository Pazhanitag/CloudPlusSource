<template>
  <brand-background-with-text-color>
  <div class="header-notification">
    <div class="level">
      <div class="level-left">
        <div v-if="parentUserProfileRole.role" class="level-item"><span>You are currently logged in as {{parentUserProfileRole.role}}. Click your name to go back to your dashboard.</span></div>
      </div>
      <div class="level-right">
        <div class="level-item">
          <popper trigger="click" :options="{placement: 'bottom-auto'}">
            <div class="popper">
              <div class="dropdown-content">
                <brand-hover>
                  <span class="dropdown-item" @click="revertImpersonate">
                   Go to My Dashboard
                  </span>
                </brand-hover>
                <hr class="dropdown-divider">
                <brand-hover>
                  <span class="dropdown-item" @click="logout">
                    Log out
                  </span>
                </brand-hover>
              </div>
            </div>
            <div slot="reference" class="cursor-pointer">
              <div class="level">
                <div class="level-item">
                  <avatar :src="parentUser.profilePicture" :fullName="`${parentUser.firstName} ${parentUser.lastName}`" :size="25"></avatar>
                </div>
                <div class="level-item">
                  <a>{{parentUser.firstName}} {{parentUser.lastName}}</a>
                </div>
                <div class="level-item">
                  <i class="fa fa-angle-down" aria-hidden="true"></i>
                </div>
              </div>
            </div>
          </popper>
        </div>
      </div>
    </div>

    </div>
  </brand-background-with-text-color>
</template>

<script>
import Popper from '@/components/shared/popper/Popper';
import { mapGetters } from 'vuex';
import AuthService from '@/services/authService';
import Avatar from '@/components/shared/image/Avatar';

export default {
  components: {
    Popper,
    Avatar,
  },
  computed: {
    ...mapGetters({
      parentUser: 'userAuth/parentUserProfile',
      parentUserProfileRole: 'userAuth/parentUserProfileRole',
      branding: 'company/brandingInformation',
    }),
  },
  methods: {
    revertImpersonate() {
      AuthService.revertImpersonate();
    },
    logout() {
      AuthService.signOut();
    },
  },
};
</script>

<style lang="scss" scoped>
.header-notification {
  padding: 0.6rem 3rem 0.6rem 1.875rem;
  line-height: 1.25rem;
  font-size: 0.8rem;
}
.fa-angle-down {
  margin-left: 0.2rem;
}
.dropdown-item{
  color: #54667a !important;
}

.dropdown-item:hover {
    color: inherit !important;
}
</style>
