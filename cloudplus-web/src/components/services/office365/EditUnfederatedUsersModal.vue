<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal" :ignoreMinHeight="true" :width="'70rem'">
      <p slot="header">Edit Unfederated Users</p>
      <section slot="section">
        <div class="set-password-prompt">Please set passwords for listed users to proceed with assigning Office 365 service</div>
        <div v-if="passwordValidationFailed" class="has-text-danger">
          You must choose a strong password that contains 8 to 16 characters, a combination of letters, and at least one number. Choose another password and try again.
        </div>
        <table class="table">
          <thead>
            <tr>
              <th></th>
              <th v-for="(column, key) in columns" :key="key">
                <a v-if="column.value !== 'password'" class="is-capitalized has-text-weight-normal" @click="sortBy(column.value)" :class="{ 'has-text-weight-bold	has-text-black': orderBy === column.value }">
                  {{ column.name}}
                  <i class="fa fa-sort" aria-hidden="true"></i>
                </a>
                <div v-else class="is-capitalized has-text-weight-normal">
                  {{column.name}}
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, key) in sortedUsers" :key="key">
              <td><avatar :fullName="user.displayName" :src="user.profilePicture"></avatar></td>
              <td>{{user.displayName}}</td>
              <td>{{user.userPrincipalName}}</td>
              <td>
                <cloud-plus-textfield @input="changeUserPassword($event, user.userPrincipalName)" :value="user.password" data-vv-name="password" v-validate="'required|min:8|max:16|office365_password'"></cloud-plus-textfield>
              </td>
            </tr>
          </tbody>
        </table>
      </section>
      <div slot="footer">
        <brand-primary-btn @click="saveMultiUserChanges" :disabled="passwordValidationFailed">Proceed</brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex';
import sortByMixin from '@/mixins/sortBy';
import { sortCompare } from '@/helpers/utils';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import Avatar from '@/components/shared/image/Avatar';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  components: {
    CloudPlusCardModal,
    Avatar,
  },
  props: {
    showModal: {
      type: Boolean,
    },
  },
  mixins: [sortByMixin],
  data() {
    return {
      columns: [
        {
          name: 'Name',
          value: 'displayName',
        },
        {
          name: 'Email',
          value: 'userPrincipalName',
        },
        {
          name: 'Password',
          value: 'password',
        },
      ],
      passwordValidationFailed: false,
    };
  },
  computed: {
    ...mapGetters({
      allSelectedUsers: 'office365/allSelectedLicenseUsers',
    }),
    unfederatedUsers() {
      return this.allSelectedUsers.filter(user => !user.isProvisioned);
    },
    sortedUsers() {
      return this.sort(this.unfederatedUsers);
    },
  },
  methods: {
    sort(users) {
      if (this.order === 'asc') {
        return users.sort((left, right) =>
          sortCompare(left[this.orderBy], right[this.orderBy], 'asc'));
      }
      return users.sort((left, right) =>
        sortCompare(left[this.orderBy], right[this.orderBy], 'desc'));
    },
    changeUserPassword(newPassword, userPrincipalName) {
      this.$validator.validate('password', newPassword).then(validated => {
        this.passwordValidationFailed = !validated;
      });
      this.updateUserPassword({
        password: newPassword,
        username: userPrincipalName,
      });
    },
    ...mapMutations({
      updateUserPassword: 'office365/UPDATE_MULTI_USER_LICENSE_PASSWORD',
    }),
    closeModal() {
      this.$emit('closeModal');
    },
    saveMultiUserChanges() {
      this.$emit('usersEdited');
      this.closeModal();
    },
  },
  created() {
    this.orderBy = 'displayName';
  },
};
</script>

<style scoped lang="scss">
  section {
    padding: 0rem 1.25rem;
  }
  .table {
    margin-top: 1rem;
    width: 100%;
  }
  .set-password-prompt {
    font-size: 0.875rem;
    margin: 2rem 0 3rem 0;
  }
</style>

