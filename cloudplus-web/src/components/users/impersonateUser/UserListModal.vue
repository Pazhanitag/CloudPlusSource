<template>
  <div>
    <cloud-plus-card-modal :width="'900px'" :showModal="showModal" @closeModal="closeModal">
      <p slot="header">Login in to reseller account</p>
      <div slot="content" class="content">
        <h1 class="title"> Which user would you like to impersonate?</h1>
        <h2 class="subtitle">Please select a user from the list which you would like to impresonate.</h2>

        <nav class="breadcrumb is-centered" aria-label="breadcrumbs">
          <div class="input-control">
            <cloud-plus-textfield :hasSearchIcon="true" v-model="search" :placeholder="'Search'"></cloud-plus-textfield>
          </div>
          <div class="dropdown-position">
          <cloud-plus-select @input="sortBy" :selected="selectedItem" :options="orderByItems.map(c => c.display)"></cloud-plus-select>
          </div>
        </nav>
      </div>
      <section slot="section">
        <div class="component-main__white"  v-if="isLoading">
          <loading-icon size="2"></loading-icon>
        </div>
        <div v-else>
          <div v-if="users.length > 0">
            <table>
              <tr v-for="(user, key) in filteredList" :key="key" @click="selectRow(user)" :class="{'row-selected': selectedUser.id == user.id, 'last-user': (filteredList.length-1) == key} ">
                <td class="table-padding">
                  <brand-input-icon class="checkbox-margin">
                    <i v-if="selectedUser.id == user.id" class="fa fa-dot-circle-o" aria-hidden="true"></i>
                    <i v-if="selectedUser.id != user.id" class="fa fa-circle-o" aria-hidden="true"></i>
                  </brand-input-icon>
                </td>
                <td>
                  <div class="image-div">
                <avatar :src="user.profilePicture" :fullName="`${user.firstName} ${user.lastName}`"></avatar>
                    </div>
                </td>
                <td>{{user.firstName}} {{user.lastName}}</td>
                <td>{{user.email}}</td>
                <td> {{user.roles.length > 0 ? user.roles[0].friendlyName : ""}}</td>
              </tr>
            </table>
            <div class="no-users-message" v-if="filteredList == 0">
              There are no users that meet your search criteria
            </div>
          </div>
          <div class="no-users-message" v-if="users.length == 0">
            There are no users in this company
          </div>
        </div>
      </section>
      <div slot="footer">
        <brand-primary-btn :disabled="!selectedUser.id || waitingForLogin" @click="loginInAs(selectedUser.id)">Log in</brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import companyService from '@/services/companyService';
import authService from '@/services/authService';
import Avatar from '@/components/shared/image/Avatar';
import { sortCompare } from '@/helpers/utils';
import loadingMixin from '@/mixins/loading';

export default {
  components: {
    CloudPlusCardModal,
    Avatar,
  },
  mixins: [loadingMixin],
  props: {
    showModal: {
      type: Boolean,
    },
    companyId: {
      type: Number,
    },
  },
  data() {
    return {
      selectedUser: {},
      users: [],
      search: '',
      orderBy: '',
      order: '',
      selectedItem: '',
      orderByItems: [
        {
          display: 'Order by name A-Z',
        },
        {
          display: 'Order by name Z-A',
        },
        {
          display: 'Order by Role',
        },
      ],
      waitingForLogin: false,
      isLoading: true,
    };
  },
  computed: {
    filteredList() {
      const filteredUsers = this.users.filter(user =>
        ((user.firstName && user.firstName.toLowerCase().includes(this.search.toLowerCase())) ||
          (user.lastName && user.lastName.toLowerCase().includes(this.search.toLowerCase())) ||
          (user.email && user.email.toLowerCase().includes(this.search.toLowerCase())) ||
          (user.roles.length > 0 &&
            user.roles[0].friendlyName.toLowerCase().includes(this.search.toLowerCase()))));
      return this.sort(filteredUsers);
    },
  },
  created() {
    this.getUsersByCompanyId(this.companyId);
    this.orderBy = 'firstName';
    this.order = 'asc';
  },
  methods: {
    selectRow(user) {
      this.selectedUser = user;
    },
    sort(users) {
      if (this.orderBy === 'firstName') {
        if (this.order === 'asc') {
          return users.sort((left, right) =>
            sortCompare(left[this.orderBy], right[this.orderBy], 'asc'));
        }
        return users.sort((left, right) =>
          sortCompare(left[this.orderBy], right[this.orderBy], 'desc'));
      }
      return users.sort((a, b) => {
        if (a.roles.length > 0 && b.roles.length > 0 &&
        a.roles[0].name.toLowerCase() > b.roles[0].name.toLowerCase()) {
          return 1;
        }
        return -1;
      });
    },
    sortBy(column) {
      this.selectedItem = column;
      if (column === 'Order by name A-Z') {
        this.orderBy = 'firstName';
        this.order = 'asc';
      } else if (column === 'Order by name Z-A') {
        this.orderBy = 'firstName';
        this.order = 'desc';
      } else {
        this.orderBy = 'roles';
      }
    },
    closeModal() {
      this.$emit('closeModal');
    },
    loginInAs(userId) {
      this.waitingForLogin = true;
      authService.impersonate(userId)
        .then({
        });
    },
    getUsersByCompanyId(companyId) {
      companyService.getUsersByCompanyId(companyId)
        .then(response => {
          this.isLoading = false;
          if (response.status === 200) {
            this.users = response.data.result;
          }
        });
    },
  },
};
</script>

<style scoped lang="scss">
.content {
  padding-top: 1.875rem;
  padding-bottom: 1rem;
}

section {
  min-height: 14.375rem;
}

.modalSearch {
  width: 12.5rem;
  margin-left: 1.5625rem;
}
table tr{
  cursor: pointer;
}
table {
  width: 100%;
}

table tr td {
  font-size: $secondary-font-size;
  color: $label-color;
}

table tr {
  border-collapse: collapse;
  border-bottom: $border-height solid #dbdbdb;
}

table tr:hover,
.row-selected {
  background-color: rgba(45, 138, 194, 0.10);
}

table td {
  vertical-align: middle
}

table tr {
  height: $tr-height;
}

table tr td {
  padding-right: 0.625rem;
}
.table-padding .fa-dot-circle-o {
  margin-left: 1.5rem;
  min-width: 2.1rem;
}

.table-padding .fa-circle-o {
    padding-right: 0.93rem;
    margin-left: 1.5rem;
}

.image-div {
  margin-top: 0.5rem;
  padding-bottom: 0.4375rem;
}

.title {
  text-align: center;
  color: $label-color;
  font-size: $font-size;
  padding-top: 1.875rem;
  padding-bottom: 1rem;
}

.subtitle {
  color: $subtitle-color;
  font-size: $secondary-font-size;
}

.input-control {
  padding-left: 1.6875rem;
}

.last-user {
  border-bottom: 0rem solid #dbdbdb;
}

.no-users-message {
  text-align: center;
  font-size: $secondary-font-size;
}
.dropdown-position {
  right: 2rem;
  position: absolute;
}
.checkbox-margin{
  margin: auto;
}
</style>
