<template>
  <div class="content">
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-select :value="user.userStatus" :selected="user.userStatus == 1 ? '1' : '0'" @input="onInput('userStatus', $event)" :options="[{name: 'Active', value: '1'}, {name: 'Suspended', value: '0'}]">Status</cloud-plus-select>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';

export default {
  inject: {
    $validator: '$validator',
  },
  computed: {
    ...mapGetters({
      user: 'user/userAccountOptions',
    }),
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      unsavedUserChangesPresent: 'user/UNSAVED_USER_CHANGES_PRESENT',
    }),
    onInput(key, value) {
      this.unsavedUserChangesPresent();
      this.updateUserProperty({
        key,
        value,
      });
    },
  },
};
</script>

<style>

</style>
