<template>
  <div>
    <loading-icon v-if="usersLoading" size="2"></loading-icon>
    <div v-else>
      <div class="selected-users-count">{{allSelectedUsers.length}} users selected</div>
      <table class="table">
        <thead>
          <tr>
            <th class="select-all-checkbox">
              <cloud-plus-check-box :value="areAllSelected" @input="toggleAll()"></cloud-plus-check-box>
            </th>
            <th></th>
            <th v-for="(column, key) in columns" :key="key">
              <a class="is-capitalized has-text-weight-normal" @click="sortUsers(column.value)" :class="{ 'has-text-weight-bold	has-text-black': orderBy === column.value }">
                {{ column.name}}
                <i class="fa fa-sort" aria-hidden="true"></i>
              </a>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, key) in pagedUsers.results" :key="key" @click="toogleUser(user)" class="cursor-pointer">
            <td><cloud-plus-check-box :value="isSelected(user.email)"></cloud-plus-check-box></td>
            <td><avatar :fullName="user.displayName" :src="user.profilePicture"></avatar></td>
            <td>{{user.displayName}}</td>
            <td>{{user.assignedLicense}}</td>
          </tr>
        </tbody>
      </table>
      <cloud-plus-pagination class="pagination-zero-bottom"
        @setPage="setPage"
        @setPageNext="setPageNext"
        @setPagePrevious="setPagePrevious"
        :usersLength="pagedUsers.results.length"
        :usersPerPage="this.pageSize"
        :currentPage="pagedUsers.pageNumber"
        :pageCount="pagedUsers.totalNumberOfPages"
        :userCount="pagedUsers.totalNumberOfRecords">
      </cloud-plus-pagination>
    </div>
  </div>
</template>

<script>
import debounce from 'lodash.debounce';
import { mapGetters, mapActions } from 'vuex';
import sortByMixin from '@/mixins/sortBy';
import Avatar from '@/components/shared/image/Avatar';
import CloudPlusPagination from '@/components/shared/pagination/CloudPlusPagination';

export default {
  components: {
    Avatar,
    CloudPlusPagination,
  },
  mixins: [sortByMixin],
  props: {
    search: {
      type: String,
    },
    domainIndex: {
      type: Number,
      required: true,
    },
    domainName: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      columns: [
        {
          name: 'Name',
          value: 'DisplayName',
        },
        {
          name: 'Assigned License',
          value: 'AssignedLicense',
        },
      ],
      selectedUsers: [],
      pageSize: 10,
      usersLoading: false,
    };
  },
  computed: {
    pagedUsers() {
      return this.domainUsers(this.domainIndex);
    },
    areAllSelected() {
      return this.pagedUsers.results.every(user =>
        this.allSelectedUsers.includes(user.email));
    },
    ...mapGetters({
      clearSelectedUsers: 'office365/clearMultiuserLicenceForms',
      domainUsers: 'office365/domainUsers',
      allSelectedUsers: 'office365/allSelectedLicenseUsersEmails',
    }),
  },
  methods: {
    ...mapActions({
      getDomainUsers: 'office365/getDomainUsers',
    }),
    toogleUser(user) {
      const userIndex = this.getUserIndex(user.email);
      if (userIndex > -1) {
        this.selectedUsers.splice(userIndex, 1);
      } else {
        this.selectedUsers.push({
          profilePicture: user.profilePicture,
          userPrincipalName: user.email,
          password: '',
          displayName: user.displayName,
          isProvisioned: user.isProvisioned,
        });
      }
      this.$emit('userSelected', this.selectedUsers);
    },
    getUserIndex(username) {
      return this.selectedUsers
        .findIndex(user => user.userPrincipalName === username);
    },
    isSelected(username) {
      return this.selectedUsers.some(user => user.userPrincipalName === username);
    },
    setPage(pageNumber) {
      this.getDomainUsersAtPage(pageNumber);
    },
    setPagePrevious(pageNumber) {
      this.setPage(pageNumber);
    },
    setPageNext(pageNumber) {
      this.setPage(pageNumber);
    },
    getDomainUsersAtPage(page) {
      this.usersLoading = true;
      return this.getDomainUsers({
        domain: this.domainName,
        index: this.domainIndex,
        pageNumber: page,
        pageSize: this.pageSize,
        orderBy: this.orderBy,
        order: this.order,
        searchTerm: this.search,
      }).then(() => {
        this.usersLoading = false;
      });
    },
    sortUsers(columnName) {
      this.sortBy(columnName);
      this.getDomainUsersAtPage(this.pagedUsers.pageNumber);
    },
    toggleAll() {
      if (this.areAllSelected) {
        this.deselectAllUsers();
      } else {
        this.selectAllUsers();
      }
      this.$emit('userSelected', this.selectedUsers);
    },
    deselectAllUsers() {
      this.pagedUsers.results.forEach(user => {
        const userIndex = this.getUserIndex(user.email);
        if (userIndex > -1) {
          this.selectedUsers.splice(userIndex, 1);
        }
      });
    },
    selectAllUsers() {
      this.pagedUsers.results.forEach(user => {
        if (this.getUserIndex(user.email) === -1) {
          this.selectedUsers.push({
            userPrincipalName: user.email,
            password: '',
            displayName: user.displayName,
            isProvisioned: user.isProvisioned,
          });
        }
      });
    },
  },
  created() {
    this.orderBy = 'DisplayName';
  },
  watch: {
    clearSelectedUsers() {
      if (this.clearSelectedUsers) {
        this.selectedUsers = [];
      }
    },
    search: debounce(
      // eslint-disable-next-line
      function() {
        this.getDomainUsersAtPage(1);
      },
      500,
    ),
  },
};
</script>

<style lang="scss" scoped>
table {
  width: 100%;
}
tr {
  height: $tr-height;
}
.select-all-checkbox {
  padding: 0.2rem 0.3rem 0.2rem 0.3rem;
}
.selected-users-count {
  font-size: 0.875rem;
  margin-bottom: 1rem;
}
</style>

