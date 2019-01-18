<template>
  <div>
    <cloud-plus-textfield class="search-field" :hasIconRight="true" :icon="'fa-search'" v-model="search" :placeholder="'Search'"></cloud-plus-textfield>
    <users-table :search="search" :companyId="companyId" disableEditUser></users-table>
  </div>
</template>

<script>
import UsersTable from '@/components/users/UsersTable';

export default {
  props: {
    companyId: {
      required: true,
    },
  },
  components: {
    UsersTable,
  },
  data() {
    return {
      search: '',
    };
  },
};
</script>

<style lang="scss" scoped>
.search-field {
  width: 20rem;
  padding-bottom: $size-medium;
}
</style>
