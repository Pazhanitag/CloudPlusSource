<template>
  <div>
    <img v-if="user.profilePictureUrl" :src="user.profilePictureUrl" alt="Profile Picture">
    <div class="name-and-status">
      <div class="level">
        <div class="level-item">
          <div class="display-name">
            {{user.displayName}}
          </div>
        </div>
        <div class="level-item-tag level-item">
          <span :class="{'tag is-primary': user.userStatus === statusActive,'tag is-danger': user.userStatus === statusSuspended }">{{user.userStatusDisplay}}</span>
        </div>
      </div>
    </div>
    <table class="table">
      <tbody>
        <tr>
          <td>First Name:</td>
          <td>{{user.firstName}}</td>
        </tr>
        <tr>
          <td>Last Name:</td>
          <td>{{user.lastName}}</td>
        </tr>
        <tr>
          <td>User Role:</td>
          <td>{{user.userRole}}</td>
        </tr>
        <tr>
          <td>Email:</td>
          <td>{{user.emailAddress}}</td>
        </tr>
        <tr>
          <td>Phone:</td>
          <td>
            <span v-if="user.phoneNumber">{{user.phoneNumber}}</span>
            <span v-else class="not-provided">Not provided</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import { userStatus } from '@/assets/constants/commonConstants';

export default {
  data() {
    return {
      statusActive: userStatus.Active,
      statusSuspended: userStatus.Suspended,
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/basicUserInfo',
    }),
  },
};
</script>

<style lang="scss" scoped>
table {
  width: 100%;
  color: $label-color;
}
.table td {
  height: 2.5rem;
}
td:last-child {
  font-weight: bold;
}
.name-and-status {
  padding: 1.25rem 1.25rem 2.25rem 0rem;
  font-size: 1.2rem;
  font-weight: bold;
}
.display-name {
  width: 100%;
}
.level-item-tag {
  flex-grow: 0 !important;
}
</style>
