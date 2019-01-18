<template>
  <div>
    <component-sticky-header title="Product Transition">
      <div class="is-4 is-offset-2">
        <brand-secondary-btn class="top-button" @click="exportData()">Export List</brand-secondary-btn>
        <brand-primary-btn @click="reviewLicenses" class="top-button">Start Transition</brand-primary-btn>
      </div>
    </component-sticky-header>
    <div class="component-main">
      <div class="component-main__white">
        <div v-if="isLoading" class="loading-message">
          <loading-icon :inline="true" size="1"></loading-icon>
          Loading users and licenses for transfer
        </div>
        <div v-else>
          <product-overview v-if="product" :product="product" :showDescription="false">
            <div class="has-text-weight-bold is-uppercase margin-medium bottom">Users and Licenses</div>
            <div class="margin-medium bottom">
              Below is a list of users and licenses that have been transferred.
              Please confirm, edit or delete the items needed.
            </div>
            <div class="margin-medium bottom">
              <div>
                <span class="has-text-weight-bold is-uppercase">Current O365 License</span>
                This is the current license active in O365 Portal.
              </div>
              <div>
                <span class="has-text-weight-bold is-uppercase">Associated O365 License</span>
                A license has been associated to your users. You can update these licenses via the drop down.
              </div>
            </div>
            <div>
              It is possible that a current Office365 license is not present in the drop down.
              <ul>
                <li>
                  If you have a user that has a license that does not allign with these packages
                  we have recommended and defaulted the best fit in this dropdown.
                  <span class="is-uppercase">These user's licenses are highlighted.</span>
                  <div class="discuss-license">
                    * If you'd like to retain their original license please select DISCUSS LICENSE OPT
                    or if you'd like a different license, please choose your preffered option.
                  </div>
                </li>
                <li>
                  If you agree to update the current package with the new package from this drop dow
                  all previous data will be deleted.
                </li>
                <li>
                  By selecting <span class="has-text-weight-bold is-uppercase">Discuss License Opt</span>, we will work with you to provide an alternative for this
                  user(s) after saving changes.
                </li>
                <li>
                  By selecting <span class="has-text-weight-bold is-uppercase">Remove License</span> you are agreeing to add this user as a license-less user within
                  your Control Panel. License will be removed on the O365 portal.
                </li>
                <li>
                  By selecting <span class="has-text-weight-bold is-uppercase">delete</span>, you are agreeing to remove user from BOTH this control panel and
                  the Microsoft Office 365 Portal. This includes all services and data associated with the user.
                  This is not recoverable.
                </li>
                <li>
                  If a user does not have an associated package from the O365 portal, we have defaulted
                  these users as an <span class="has-text-weight-bold is-uppercase">Admin - No License</span>.
                </li>
              </ul>
            </div>
          </product-overview>
          <div v-if="unsuportedDomainsExist" class="unsuported-domain-warning">
            <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
            <span class="unsuported-domain-warning__text"> You have one or more Office365 domains which are not added to our portal yet. The users belonging
            to those domains can not be transitioned. You can add the missing domains to your account and come back to this screen to proceed with transitioning.</span>
          </div>
          <table v-if="transitionData" class="table">
            <thead>
              <tr>
                <th></th>
                <th v-for="(column, key) in columns" :key="key">
                  <a class="is-capitalized has-text-weight-normal" @click="sortBy(column.value)" :class="{ 'has-text-weight-bold	has-text-black': orderBy === column.value }">
                    {{ column.name}}
                    <i v-if="column.value !== 'password'" class="fa fa-sort" aria-hidden="true"></i>
                  </a>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(user, key) in sortedUsers" :key="key">
                <td>
                  <cloud-plus-tooltip
                    v-if="isUserUnsuported(user.userPrincipalName)"
                    tooltipIcon="exclamation-triangle"
                    :tooltipText="'The domain of this user has not been added to our portal yet'">
                  </cloud-plus-tooltip>
                  <cloud-plus-tooltip
                    v-if="user.unsupportedLicensesFound"
                    tooltipIcon="exclamation-triangle"
                    :tooltipText="'Unsupported license found'">
                  </cloud-plus-tooltip>
                </td>
                <td>{{user.userPrincipalName}}</td>
                <td>{{user.currentProductItemName}}</td>
                <td>
                  <cloud-plus-select
                  @input = "changeRecomendedProductItem($event, user.userPrincipalName)"
                  :selected="user.recommendedProductItem.cloudPlusProductIdentifier"
                  :options="assignLicenceOptions"
                  :hasWarning="user.unsupportedLicensesFound">
                  </cloud-plus-select>
                </td>
                <td>{{user.displayName}}</td>
                <td>
                  <cloud-plus-textfield @input="changeUserPassword($event, user.userPrincipalName)" :value="user.password"></cloud-plus-textfield>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <confirmation-modal
      v-if="showReviewLicensesModal" :showModal="showReviewLicensesModal"
      @cancel="closeReviewLicensesModal"
      @confirm = "startTransitionProcess"
      title="Review Licenses Match"
      confirmText="Confirm">
      <div class="license-review">
        <div class="has-text-weight-bold margin-medium bottom">By accepting you are agreeing to:</div>
        <ol>
          <li>
            Delete any users and any previous data marked as <span class="has-text-weight-bold is-uppercase">delete</span> from both your Control Panel and the Office365 Portal.
          </li>
          <li>
            Change any current Office 365 licenses to the associated license in the drop down. By doing so all previous data and features no longer exist.
          </li>
          <li>
            Remove any previous licenses from the users marked as Remove License. They will be added to the portal as a license-less user.
          </li>
        </ol>
        <div>
          Finally, we will reach out directly to discuss any users marked as Discussion. If you have any reservation or you see an error, please cancel this confirmation
          and update your user list. If you have any questions, please contact your support team.
        </div>
      </div>
    </confirmation-modal>
  </div>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from 'vuex';
import sortByMixin from '@/mixins/sortBy';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import { sortCompare, convertArrayOfObjectsToCSV } from '@/helpers/utils';
import companyService from '@/services/companyService';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import ProductOverview from '@/components/catalogs/customer/products/ProductOverview';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';

export default {
  mixins: [sortByMixin, toasterMixin, loadingMixin],
  data() {
    return {
      columns: [
        {
          name: 'Email',
          value: 'userPrincipalName',
        },
        {
          name: 'Current License',
          value: 'currentProductItemName',
        },
        {
          name: 'Associated License',
          value: 'associatedLicense',
        },
        {
          name: 'Display Name',
          value: 'displayName',
        },
        {
          name: 'Password',
          value: 'password',
        },
      ],
      unsuportedDomains: [],
      showReviewLicensesModal: false,
      discussLicenseOptionIndex: null,
    };
  },
  components: {
    ComponentStickyHeader,
    ProductOverview,
    ConfirmationModal,
  },
  computed: {
    ...mapGetters({
      product: 'product/selectedProduct',
      transitionData: 'office365/transitionData',
      companyId: 'userAuth/companyId',
    }),
    unsuportedDomainsExist() {
      return this.unsuportedDomains.length > 0;
    },
    sortedUsers() {
      return this.sort(this.transitionData.productItems);
    },
    assignLicenceOptions() {
      const additionalOptions = [
        {
          value: 'delete',
          name: 'Delete',
        },
        {
          value: 'removeLicenses',
          name: 'Remove Licenses',
        },
        {
          value: 'keepLicenses',
          name: 'Discuss License Option',
        },
        {
          value: 'admin',
          name: 'Admin',
        },
      ];
      if (this.product !== null) {
        const availableProducts = this.product.productItems.filter(p => !p.isAddon);
        const availableOptions = availableProducts.map(product =>
          ({
            value: product.identifier,
            name: product.name,
          }));
        this.discussLicenseOptionIndex = availableOptions.concat(additionalOptions).findIndex(options => options.value === 'keepLicenses');
        return availableOptions.concat(additionalOptions);
      }
      this.discussLicenseOptionIndex = additionalOptions.findIndex(options => options.value === 'keepLicenses');
      return additionalOptions;
    },
  },
  methods: {
    ...mapActions({
      getProduct: 'product/getProduct',
      getTransitionData: 'office365/getTransitionData',
      startTransition: 'office365/startTransition',
    }),
    ...mapMutations({
      updateUserPassword: 'office365/UPDATE_TRANSITION_USER_PASSWORD',
      updateRecomendedProductItem: 'office365/UPDATE_TRANSITION_RECOMENDED_PRODUCT_ITEM',
      setUserPropertyToTrue: 'office365/SET_TRANSITION_USER_PROPERTY_TO_TRUE',
    }),
    sort(users) {
      if (users !== undefined) {
        if (this.order === 'asc') {
          if (this.orderBy === 'associatedLicense') {
            return users.sort((left, right) =>
              sortCompare(
                left.recommendedProductItem.name,
                right.recommendedProductItem.name, 'asc',
              ));
          }
          return users.sort((left, right) =>
            sortCompare(left[this.orderBy], right[this.orderBy], 'asc'));
        }
        if (this.orderBy === 'associatedLicense') {
          return users.sort((left, right) =>
            sortCompare(
              left.recommendedProductItem.name,
              right.recommendedProductItem.name, 'desc',
            ));
        }
        return users.sort((left, right) =>
          sortCompare(left[this.orderBy], right[this.orderBy], 'desc'));
      }
      return users;
    },
    changeUserPassword(newPassword, userPrincipalName) {
      this.updateUserPassword({
        password: newPassword,
        username: userPrincipalName,
      });
    },
    changeRecomendedProductItem(selectedOption, userPrincipalName) {
      if (selectedOption.length < 32) {
        this.updateRecomendedProductItem({
          product: {
            name: '',
            identifier: selectedOption,
          },
          username: userPrincipalName,
        });
        this.setUserPropertyToTrue({
          property: selectedOption,
          username: userPrincipalName,
        });
      } else {
        const productName = this.product.productItems
          .find(p => p.identifier === selectedOption).name;
        this.updateRecomendedProductItem({
          product: {
            name: productName,
            identifier: selectedOption,
          },
          username: userPrincipalName,
        });
      }
    },
    compareOfficeAndPortalDomains() {
      companyService.getCompanyDomains(this.companyId).then(response => {
        const portalDomains = response.data.result.map(d => d.name.toLowerCase());
        this.unsuportedDomains = this.transitionData.domains
          .filter(domain => !portalDomains.includes(domain.toLowerCase()));
      });
    },
    isUserUnsuported(username) {
      const domainStartIndex = username.indexOf('@');
      const userDomain = username.slice(domainStartIndex + 1);
      return this.unsuportedDomains.includes(userDomain);
    },
    closeReviewLicensesModal() {
      this.showReviewLicensesModal = false;
    },
    initializeGettingTransitionData() {
      this.getTransitionData().then(() => {
        this.isLoading = false;
        this.transitionData.productItems.forEach(transitionUser => {
          if (transitionUser.recommendedProductItem.cloudPlusProductIdentifier === undefined) {
            this.changeRecomendedProductItem(
              this.assignLicenceOptions[this.discussLicenseOptionIndex].value,
              transitionUser.userPrincipalName,
            );
          }
        });
        if (this.transitionData !== undefined) {
          this.compareOfficeAndPortalDomains();
        }
      });
    },
    startTransitionProcess() {
      this.startTransition().then(() => {
        this.sucessToaster({
          text: 'The transition process has been started.',
          icon: 'briefcase',
        });
        this.closeReviewLicensesModal();
        this.$router.push({
          path: '/users',
        });
      });
    },
    reviewLicenses() {
      this.showReviewLicensesModal = true;
    },
    exportData() {
      const filename = 'User-and-Licence-Matches.csv';
      const array = ([]);
      for (let i = 0; i < this.sortedUsers.length; i += 1) {
        array.push({
          Email: this.sortedUsers[i].userPrincipalName,
          License: this.sortedUsers[i].recommendedProductItem.name !== '' ? this.sortedUsers[i].recommendedProductItem.name : this.sortedUsers[i].recommendedProductItem.cloudPlusProductIdentifier,
          'Display Name': this.sortedUsers[i].displayName,
          Password: this.sortedUsers[i].password !== undefined ? this.sortedUsers[i].password : '',
        });
      }
      const csv = convertArrayOfObjectsToCSV({
        data: array,
      });
      if (csv == null) return;

      const blob = new Blob([csv], {
        type: 'text/csv;charset=utf-8;',
      });
      if (navigator.msSaveBlob) {
        navigator.msSaveBlob(blob, filename);
      } else {
        const link = document.createElement('a');
        if (link.download !== undefined) {
          const url = URL.createObjectURL(blob);
          link.setAttribute('href', url);
          link.setAttribute('download', filename);
          link.style = 'visibility:hidden';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }
      }
    },
  },
  created() {
    this.isLoading = true;
    // TODO: add proper method which does not use a hard coded id to get office365 details
    if (this.product === null) {
      this.getProduct(1).then(() => {
        this.orderBy = 'userPrincipalName';
        this.initializeGettingTransitionData();
      });
    } else {
      this.orderBy = 'userPrincipalName';
      this.initializeGettingTransitionData();
    }
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

ul {
    list-style-type: circle;
    padding-left: 1rem;
}

ol {
  padding-left: 1rem;
  li {
    padding-bottom: 1rem;
  }
}

.unsuported-domain-warning {
  font-size: $secondary-font-size;
  margin-bottom: 2rem;
  &__text{
    padding-left: 0.5rem;
  }
}

.unsuported-domain-user {
  background: #ff849d;
}

.discuss-license {
  padding-left: 2rem;
}

.license-review {
  text-align: left;
  font-size: 0.875rem;
}

.loading-message {
  text-align: center;
  font-size: 0.875rem;
}
</style>
