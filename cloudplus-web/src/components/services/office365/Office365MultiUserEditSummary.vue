<template>
  <div class="multi-user-summary">
    <product-overview v-if="selectedProductItem" :product="product" :showDescription="false"></product-overview>
    <div class="columns">
      <div class="column is-7">
        <div class="selected-feature-title">Selected Users:</div>
        <table class="table">
          <tbody>
            <tr v-for="(user, key) in selectedUsers" :key="key">
              <td><avatar :fullName="user.displayName" :src="user.profilePicture"></avatar></td>
              <td>{{user.displayName}}</td>
              <td>{{user.userPrincipalName}}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="column is-5">
        <div class="selected-feature-title">Selected Roles:</div>
        <div v-if="selectedRoles.length > 0" class="selected-roles">
          <div v-for="role in selectedRoles" :key="role" class="selected-roles__role"><i class="fa fa-check"></i>{{role}}</div>
        </div>
        <div v-else class="not-provided">No roles selected</div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import Avatar from '@/components/shared/image/Avatar';
import ProductOverview from '@/components/catalogs/customer/products/ProductOverview';

export default {
  components: {
    Avatar,
    ProductOverview,
  },
  computed: {
    ...mapGetters({
      selectedUsers: 'office365/allSelectedLicenseUsers',
      selectedRoles: 'office365/selectedOffice365Roles',
      selectedProductItem: 'office365/selectedOffice365ProductName',
      selectedProduct: 'product/selectedProduct',
    }),
    product() {
      return {
        imgUrl: this.selectedProduct.imgUrl,
        name: this.selectedProductItem,
        vendor: this.selectedProduct.vendor,
      };
    },
  },
};
</script>

<style <style lang="scss" scoped>
.table {
  width: 100%;
}
.column {
  text-align: left;
}
.selected-feature-title {
  font-weight: bold;
  margin-bottom: 1rem;
}
.selected-roles {
  border: 1px solid #f2f2f2;
    &__role {
    border-bottom: 1px solid #f2f2f2;
    padding: 1rem 1rem 1rem 1rem;
    height: $tr-height;
    line-height: 2rem;
  }
}
.fa-check {
  margin-right: 1rem;
}
.multi-user-summary {
  text-align: left;
}
</style>
