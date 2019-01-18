<template>
  <div class="content">
    <cloud-plus-radio-btn @input="onInput" :checked="user.roles[0] === role.id" :checkedValue="role.id" :subtitle="role.description" :value="user.roles[0]" v-for="(role, index) in roles" :key="index">
      {{role.friendlyName}}
    </cloud-plus-radio-btn>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import roleService from '@/services/roleService';
import appConfig from 'appConfig';

export default {
  inject: {
    $validator: '$validator',
  },
  data() {
    return {
      roles: [],
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/userRoles',
    }),
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      unsavedUserChangesPresent: 'user/UNSAVED_USER_CHANGES_PRESENT',
    }),
    onInput(roleId) {
      this.unsavedUserChangesPresent();
      this.assignRole(roleId);
    },
    assignRole(roleId) {
      this.selectedRoleId = roleId;
      this.updateUserProperty({
        key: 'roles',
        value: [roleId],
      });
    },
    getAvailableRoles() {
      roleService.getAllRoles().then(response => {
        this.roles = response.data.result;
        if (this.user.roles.length === 0) {
          const userRole = this.roles.filter(role => role.name === appConfig.userRole)[0];
          if (userRole) {
            this.assignRole(userRole.id);
          }
        } else {
          this.assignRole(this.user.roles[0]);
        }
      });
    },
  },
  created() {
    this.getAvailableRoles();
  },
};
</script>

<style>

</style>
