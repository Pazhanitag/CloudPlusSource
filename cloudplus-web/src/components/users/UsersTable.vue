<template>
  <div>
    <loading-icon size="2" v-if="isLoading"></loading-icon>
    <div v-else>
      <div v-if="pagedUsers.results.length > 0">
        <table class="table" style="width: 100%;">
          <thead class="is-uppercase">
            <tr>
              <th></th>
              <th v-for="column in columns" :key="column.email">
                <a class="is-capitalized" @click="sortBy(column.value)" :class="{ 'has-text-weight-bold	has-text-black': orderBy === column.value }">
                  {{ column.name}}
                  <i class="fa fa-sort" aria-hidden="true"></i>
                </a>
              </th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, key) in pagedUsers.results" :key="key">
              <td>
                <avatar :src="user.profilePicture" :fullName="`${user.firstName} ${user.lastName}`"></avatar>
              </td>
              <td><span>{{user.firstName}}</span></td>
              <td><span>{{user.lastName}}</span></td>
              <td><span>{{user.email}}</span></td>
              <td>
                <span v-if="user.phoneNumber">{{user.phoneNumber}}</span>
                <span v-else class="not-provided">Not provided</span>
              </td>
              <td><span>{{user.role}}</span></td>
              <td>
              <span :class="{'tag is-active': user.userStatus === statusActive,'tag is-suspended': user.userStatus === statusSuspended }">{{user.userStatusDisplay}}</span>
              </td>
              <td v-if="!disableEditUser">
                <popper trigger="click" :options="{placement: 'bottom'}">
                  <div class="popper">
                    <div class="dropdown-content">
                      <brand-hover>
                        <span class="dropdown-item" @click="goToEditUserPage(user.id)">
                          Edit user details
                        </span>
                      </brand-hover>
                      <hr class="dropdown-divider">
                      <brand-hover>
                        <span class="dropdown-item" @click="openChangeUserPasswordModal(user.id, [user.email, user.alternativeEmail])">
                          Change user password
                        </span>
                      </brand-hover>
                      <div v-can-see="['DeleteUsers']" v-if="deleteEnabled(user)">
                        <hr class="dropdown-divider">
                        <brand-hover>
                          <span class="dropdown-item" @click="openDeleteUserModal(user.id)">
                            Delete User
                          </span>
                        </brand-hover>
                      </div>
                    </div>
                  </div>
                  <cloud-plus-cog slot="reference"></cloud-plus-cog>
                </popper>
              </td>
              <confirmation-modal
                v-if="showConfirmationModal" :showModal="showConfirmationModal"
                @cancel="closeDeleteUserModal()"
                @confirm = "deleteUser(user.id)"
                confirmText="Delete">
                  <p>Are you sure you want to delete this user?</p>
                  <br/>
                  <p><span class="has-text-weight-bold">NOTE:</span> All services assigned to the user will be deprovisioned. Service related data will be permanently lost.</p>
              </confirmation-modal>
            </tr>
          </tbody>
        </table>
        <cloud-plus-pagination v-if="pagedUsers.results.length > 0" @setPage="setPage" @setPageNext="setPageNext" @setPagePrevious="setPagePrevious" :usersLength="pagedUsers.results.length" :usersPerPage="usersPerPage" :currentPage="pagedUsers.pageNumber" :pageCount="pagedUsers.totalNumberOfPages" :userCount="pagedUsers.totalNumberOfRecords"></cloud-plus-pagination>
      </div>
      <div v-if="checkIfUsersExist(pagedUsers.results.length, !search)" class="no-users-warning">
        <div class="no-users-warning__title">Seems like you do not have any company users.</div>
        <div class="no-users-warning__subtitle">This is unexpected! Please contact us for support.</div>
        <router-link class="no-users-warning__subtitle" :to="parentDetailsPage" >
          Go to <span class="link"> Parent company </span>
        </router-link>
      </div>
      <div v-if="checkIfUsersExist(pagedUsers.results.length, search)" class="no-users-warning">
        <div class="no-users-warning__title">No results match your search criteria.</div>
      </div>
      <change-password-modal v-if="showModal" :showModal="showModal" :userId="selectedUserId" :emailList="EmailList" :isMultipleUser="true" @closeModal="closeModal"> </change-password-modal>
    </div>
  </div>
</template>

<script>
import debounce from 'lodash.debounce';
import Popper from '@/components/shared/popper/Popper';
import { userStatus } from '@/assets/constants/commonConstants';
import accountTypesConstants from '@/assets/constants/accountTypes';
import appConfig from 'appConfig';
import { mapActions, mapGetters } from 'vuex';
import sortByMixin from '@/mixins/sortBy';
import loadingMixin from '@/mixins/loading';
import toasterMixin from '@/mixins/toaster';
import Avatar from '@/components/shared/image/Avatar';
import CloudPlusCog from '@/components/shared/misc/CloudPlusCog';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import userService from '@/services/userService';
import CloudPlusPagination from '../../components/shared/pagination/CloudPlusPagination';
import ChangePasswordModal from './ChangePasswordModal';

export default {
  props: {
    search: {
      type: String,
    },
    companyId: {
      required: true,
    },
    disableEditUser: {
      type: Boolean,
      default: false,
    },
  },
  mixins: [sortByMixin, loadingMixin, toasterMixin],
  components: {
    CloudPlusPagination,
    Avatar,
    Popper,
    ChangePasswordModal,
    CloudPlusCog,
    ConfirmationModal,
  },
  data() {
    return {
      usersExist: false,
      showModal: false,
      showConfirmationModal: false,
      selectedUserId: 0,
      EmailList: {
        primaryEmailAddress: '',
        alternativeEmailAddress: '',
      },
      usersPerPage: appConfig.usersPerPage,
      statusActive: userStatus.Active,
      statusSuspended: userStatus.Suspended,
      accountTypes: accountTypesConstants,
      columns: [
        {
          name: 'First Name',
          value: 'FirstName',
        },
        {
          name: 'Last Name',
          value: 'LastName',
        },
        {
          name: 'Email',
          value: 'Email',
        },
        {
          name: 'Phone',
          value: 'PhoneNumber',
        },
        {
          name: 'Role',
          value: 'Role',
        },
        {
          name: 'Status',
          value: 'UserStatus',
        },
      ],
      parentDetailsPage: '/companies/details?companyLevel=1',
    };
  },
  computed: {
    ...mapGetters({
      pagedUsers: 'company/pagedUsers',
      company: 'company/generalInformation',
      userProfile: 'userAuth/userProfile',
      loggedUserProfileRole: 'userAuth/userProfileRole',
    }),
    userCount() {
      return this.pagedUsers.totalNumberOfRecords;
    },
  },
  watch: {
    search: debounce(
      // eslint-disable-next-line
      function() {
        this.getUsersAtPage(1);
      },
      500,
    ),
    ordChange() {
      this.getUsersAtPage(1);
    },
  },
  mounted() {
    this.orderBy = 'Id';
    this.getCompany(this.companyId).then(() => {
      this.getUsersAtPage(1).then(() => {
        this.isLoading = false;
        if (this.pagedUsers.totalNumberOfRecords !== 0) {
          this.usersExist = true;
        }
      });
    });
  },
  methods: {
    ...mapActions({
      getPagedUsers: 'company/getPagedUsers',
      getCompany: 'company/getCompany',
    }),
    addNewUser() {
      this.$router.push({
        path: appConfig.createNewUserRoute,
        query: {},
      });
    },
    setPage(pageNumber) {
      this.getUsersAtPage(pageNumber);
    },
    setPagePrevious(pageNumber) {
      this.setPage(pageNumber);
    },
    setPageNext(pageNumber) {
      this.setPage(pageNumber);
    },
    goToEditUserPage(userId) {
      this.$router.push({
        path: `/${appConfig.editUserRoute}/${userId}`,
      });
    },
    openChangeUserPasswordModal(userId, emailList) {
      this.selectedUserId = userId;
      [this.EmailList.primaryEmailAddress, this.EmailList.alternativeEmailAddress] = emailList;
      this.showModal = true;
    },
    openDeleteUserModal(userId) {
      this.selectedUserId = userId;
      this.showConfirmationModal = true;
    },
    closeDeleteUserModal() {
      this.showConfirmationModal = false;
    },
    closeModal() {
      this.showModal = false;
    },
    deleteUser() {
      userService.deleteUser(this.selectedUserId).then(() => {
        this.sucessToaster({
          text: 'Your user will be deleted shortly.',
          icon: 'user',
          duration: 5000,
        });
        this.showConfirmationModal = false;
        this.getUsersAtPage(1);
      });
    },
    roleHighEnough(user) {
      if (!user.roles.length) return true;

      switch (user.roles[0].name) {
        case appConfig.masterRole:
          return this.loggedUserProfileRole.role === appConfig.masterRole;
        case appConfig.resellerRole:
          return this.loggedUserProfileRole.role === appConfig.masterRole
          || this.loggedUserProfileRole.role === appConfig.resellerRole;
        case appConfig.customerRole:
          return this.loggedUserProfileRole.role === appConfig.masterRole
          || this.loggedUserProfileRole.role === appConfig.resellerRole
          || this.loggedUserProfileRole.role === appConfig.customerRole;
        default:
          return true;
      }
    },
    deleteEnabled(user) {
      if (!(Number(this.userProfile.id) === user.id)
      && this.roleHighEnough(user)) {
        return true;
      }
      return false;
    },
    getUsersAtPage(page) {
      return this.getPagedUsers({
        companyId: this.companyId,
        pageNumber: page,
        pageSize: this.usersPerPage,
        orderBy: this.orderBy,
        order: this.order,
        searchTerm: this.search,
      });
    },
    checkIfUsersExist(usersLength, searchTerm) {
      return usersLength === 0 && searchTerm;
    },
  },
};
</script>

<style lang="scss" scoped>
  .no-users-warning {
    text-align: center;
    &__title {
      color: $label-color;
      font-size: $big-font-size;
    }
    &__subtitle {
      color: $subtitle-color;
      font-size: $primary-font-size;
    }
  }
  tr th:nth-child(5) {
    min-width: 6rem;
  }
  .link{
    color: #6262ff;
    text-decoration: underline;
  }
  .tag{
    font-size: 0.75rem;
  }
</style>
