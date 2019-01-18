<template>
  <div>
    <component-sticky-header title="User List" class="component-min-width">
        <brand-primary-btn @click="addNewUser()" class="is-pulled-right">Add new user</brand-primary-btn>
        <cloud-plus-textfield class="is-pulled-right margin-large right" v-if="pagedUsers.results.length > 0 || search !== ''" :hasIconRight="true" :icon="'fa-search'" v-model="search" :placeholder="'Search'"></cloud-plus-textfield>
    </component-sticky-header>
    <div class="component-main">
      <div class="component-main__white">
        <users-table :search="search" :companyId="Number(userProfile.companyId)"></users-table>
      </div>

    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import UsersTable from '@/components/users/UsersTable';

export default {
  components: {
    UsersTable,
    ComponentStickyHeader,
  },
  data() {
    return {
      search: '',
    };
  },
  computed: {
    ...mapGetters({
      userProfile: 'userAuth/userProfile',
      pagedUsers: 'company/pagedUsers',
    }),
  },
  methods: {
    addNewUser() {
      this.$router.push({
        path: '/users/create',
        query: {},
      });
    },
  },
};
</script>

<style scoped lang="scss">
.component-min-width{
  min-width: 30rem;
}
</style>
