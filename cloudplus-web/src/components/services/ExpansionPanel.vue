<template>
  <div class="product-container">
    <div class="product-header">
      <table class="table is-fullwidth" @click="toggleProductItemsVisibility">
        <tr>
          <td class="caret">
            <i class="fa" :class="[showProductItems ? 'fa-angle-down' : 'fa-angle-right']" aria-hidden="true"></i>
          </td>
          <td class="product-image">
            <img :src="product.imgUrl" class="product-image"/>
          </td>
          <td class="product-name">
            <h1 class="is-size-6 has-text-weight-semibold">{{product.name}}</h1>
            <!-- <h2 class="is-size-7 has-text-weight-light">
              For more details about this product, its packages and addons,
              <brand-color-primary class="link"><span v-on:click.stop="openProductDetails">click here</span></brand-color-primary>
            </h2> -->
          </td>
          <!-- <td class="product-packages-count">
            <i class="fa fa-list" />
            <span>{{packagesCount}} Packages</span>
          </td>
          <td class="product-addons-count" v-if="addonsCount > 0">
            <div>
              <i class="fa fa-level-up" />
              <span>{{addonsCount}} Addons</span>
            </div>
          </td> -->
          <td class="product-activated-date">
            {{formatDate(product.activatedDate)}}
          </td>
          <td class="product-lowest-price">
            <template v-if="product.id === productIds.office365">
              <span v-show="product.statusIcon === 'fa fa-exclamation-triangle'">Actions required</span>
              <span v-show="product.statusIcon === 'fa fa-hourglass'">Pending</span>
              <span v-show="product.statusIcon === 'fa fa-check'">Completed</span>
            </template>
            <span v-if="product.name === productItemIds.customSecureControlPanelURL">{{product.customControlPanel[0].status.statusValue}}</span>
          </td>
          <td class="status-icon">
            <span v-if="product.id === productIds.office365">
              <i :class="product.statusIcon"/>
            </span>
            <span v-if="product.name === productItemIds.customSecureControlPanelURL">
              <i :class="product.customControlPanel[0].status.statusIcon"/>
            </span>
          </td>
        </tr>
      </table>
    </div>
    <div class="product-items" v-if="showProductItems">
      <table class="table is-fullwidth">
        <tbody>
          <slot></slot>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import moment from 'moment';
import { catalogIdConstants, productIdConstants } from '@/assets/constants/commonConstants';
// import { mapGetters } from 'vuex';

export default {
  props: {
    product: {
      required: true,
    },
  },
  data() {
    return {
      showProductItems: false,
      productIds: catalogIdConstants,
      productItemIds: productIdConstants,
    };
  },
  computed: {
    /* ...mapGetters({
      products: 'product/allProducts',
    }),
     product() {
      return this.products.find(product => product.id === this.productId);
    },
     packagesCount() {
      return this.product.productItems.filter(productItem => !productItem.isAddon).length;
    },
    addonsCount() {
      return this.product.productItems.filter(productItem => productItem.isAddon).length;
    }, */
  },
  methods: {
    formatDate(date) {
      if (date) {
        return moment(date).format('MM/DD/YYYY');
      }
      return '';
    },
    toggleProductItemsVisibility() {
      this.showProductItems = !this.showProductItems;
    },
    openProductDetails() {
      this.$router.push({
        path: `/catalogs/customer/products/${this.product.id}`,
      });
    },
  },
  mounted() {
  },
};
</script>

<style scoped="true" lang="scss">
.product-header table {
  border: 1px solid #d6e4f0 !important;
  background-color: #f2f7f8;
  cursor: pointer;
  height: 4.3rem;
}

.product-header table tr td.caret {
  font-size: 1.5rem;
  text-align: center;
  width: 4%;
}
td.product-name {
  width: 35%;
}

td.product-name h1 {
  max-width: 410px;
  margin: 0 auto
}

td.product-packages-count,
td.product-addons-count,
td.product-tag,
td.product-lowest-price {
  width: 11%;
}
td.product-activated-date{
  width: 11%;
}
td.status-icon {
  width: 4%;
}
td.product-image {
  width: 13%;
}
.product-image {
  height: 3rem;
  padding: 0.6rem;
}
.product-items table tr {
  height: 4rem;
}

tag.is-addon {
  background-color: #e7995f;
}

.link{
  text-decoration: underline;
  font-weight: bold;
  display: inline;
  cursor: pointer;
}
.tag-margin{
  margin-left: 0.5rem;
  padding-bottom: 0rem;
  padding-top: 0.1rem;
  margin-top: 0.15rem;
}
.bottom{
  margin-bottom: 0.4rem;
  display: flex;
}
.main-modal-section{
  padding: 1rem 1rem 0rem 1rem;
}
.product-description{
  color: $subtitle-color;
  font-size: $small-font-size;
}
.product-item-weight{
  font-weight: 400;
  color: #585858;
}

.fa-hourglass {
  color: #54667a;
}

.main-text {
  font-size: $secondary-font-size;
}

@media screen and (max-width: 2000px) {
  .product-items table .support-item-name {
    max-width: calc(100% / 2 + 300px);
  }
  .packages-padding{
    padding: 0rem 0rem 0rem 1.7rem;
  }
}
@media screen and (max-width: 1700px) {
  .product-items table .support-item-name {
    max-width: calc(100% / 2 + 285px);
  }
  .packages-padding{
    padding: 0rem 0rem 0rem 0.5rem;
  }
}
@media screen and (max-width: 1366px) {
  .product-items table .support-item-name {
    max-width: calc(100% / 2 + 270px);
  }
}
@media screen and (max-width: 1020px) {
  .product-items table .support-item-name {
    max-width: calc(100% / 2 + 205px);
  }
}
@media screen and (max-width: 800px) {
  .product-items table .support-item-name {
    max-width: calc(100% / 2 + 185px);
  }
}
</style>
