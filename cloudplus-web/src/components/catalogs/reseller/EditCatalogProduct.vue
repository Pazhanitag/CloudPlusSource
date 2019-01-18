<template>
  <div>
    <div class="product-container">
      <div class="product-header">
        <table class="table is-fullwidth" @click="toggleProductItemsVisibility">
          <tr>
            <td class="caret">
              <i class="fa" :class="[showProductItems ? 'fa-angle-down' : 'fa-angle-right']" aria-hidden="true"></i>
            </td>
            <td class="product-image">
              <img :src="product.img" class="product-image"/>
            </td>
            <td>
              <h1 class="is-size-6 has-text-weight-semibold">{{product.name}}</h1>
              <h2 class="is-size-7 has-text-weight-light">{{product.vendor}}</h2>
            </td>
            <td class="product-status">
            </td>
          </tr>
        </table>
      </div>
      <div class="product-items" v-show="showProductItems">
        <table class="table is-fullwidth">
          <thead>
          <th></th>
          <th class="has-text-weight-light">Cost</th>
          <th class="has-text-weight-light">Reseller</th>
          <th class="has-text-weight-light">Customer</th>
          <th class="has-text-weight-light" v-can-see="['SetMsrpFixed']">MSRP</th>
          <th class="has-text-weight-light">Availability</th>
          </thead>
          <tbody>
          <tr v-for="(productItem, index) in filteredProductItems" v-bind:key="index"
              v-bind:style="{'border-left': '4px solid ' + (productItem.isAddon ? '#e7995f' : '#2d8ac2')}">
            <td class="product-item-name">
              {{productItem.name}}
            </td>
            <td>
              <input type="text" class="input has-text-weight-semibold" :value="productItem.cost | formatPrice" disabled>
              <span class="has-text-weight-light currency">$</span>
            </td>
            <td>
              <input type="text" class="input has-text-weight-semibold"
                     v-format-price
                     :value="productItem.resellerPrice"
                     @input="resellerPriceChange($event.target.value, productItem.productItemId)"
                     :disabled="!productItem.available"
                     data-vv-delay="0"
                     v-validate="'required|decimal|min_value:0'"
                     :data-vv-name="'resellerPrice' + productItem.productItemId"
                     data-vv-as="reseller price"
                     name="resellerPrice"
                     :class="{'is-danger': errors.first('resellerPrice' + productItem.productItemId), 'is-warning': showWarning(productItem)}"
                     :title="errors.first('resellerPrice' + productItem.productItemId)">

              <span class="has-text-weight-light currency">$</span>

              <cloud-plus-tooltip
                class="pice-warning"
                tooltipIcon="exclamation-triangle"
                :tooltipText="getWarningMessage(productItem)"
                v-visible="showWarning(productItem) && !errors.first('resellerPrice' + productItem.productItemId)"> </cloud-plus-tooltip>
            </td>
            <td>
              <input type="text" class="input has-text-weight-semibold"
                     v-format-price
                     :value="productItem.retailPrice"
                     @input="retailPriceChange($event.target.value, productItem.productItemId)"
                     :disabled="retailPriceDisabled(productItem)"
                     data-vv-delay="0"
                     v-validate="'required|decimal||min_value:0'"
                     :data-vv-name="'retailPrice' + productItem.productItemId"
                     data-vv-as="retail price"
                     name="retailPrice"
                     :class="{'is-danger': errors.first('retailPrice' + productItem.productItemId)}"
                     :title="errors.first('retailPrice' + productItem.productItemId)"> <span
              class="has-text-weight-light currency">$</span>
            </td>
            <td v-can-see="['SetMsrpFixed']">
              <cloud-plus-check-box :value="productItem.fixedRetailPrice"
                                    @input="fixedRetailPriceChange($event, productItem.productItemId)"
                                    :disabled="!productItem.available"></cloud-plus-check-box>
            </td>
            <td>
              <cloud-plus-switch :checked="productItem.available"
                                 @input="productItemAvailabilityChange($event, productItem.productItemId)"></cloud-plus-switch>
            </td>
          </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
  import { mapGetters, mapMutations } from 'vuex';
  import CloudPlusSwitch from '@/components/shared/input-components/CloudPlusSwitch';
  import CloudPlusCheckbox from '@/components/shared/input-components/CloudPlusCheckBox';

  export default {
    inject: {
      $validator: '$validator',
    },
    props: {
      productId: {
        required: true,
        type: Number,
      },
      search: {
        type: String,
      },
    },
    data() {
      return {
        showProductItems: false,
      };
    },
    components: {
      CloudPlusSwitch,
      CloudPlusCheckbox,
    },
    computed: {
      ...mapGetters({
        resellerCatalog: 'catalog/resellerCatalog',
        permissions: 'userAuth/permissions',
      }),
      canEditRetailPrice() {
        return this.permissions.filter(p => p.name === 'SetMsrpFixed').length > 0;
      },
      product() {
        return this.resellerCatalog.products.find(product => product.id === this.productId);
      },
      filteredProductItems() {
        return this.product.productItems.filter(productItem =>
          productItem.name.toLowerCase().includes(this.search.toLowerCase()));
      },
    },
    methods: {
      ...mapMutations({
        updateResellerPrice: 'catalog/UPDATE_RESELLER_PRICE',
        updateRetailPrice: 'catalog/UPDATE_RERATIL_PRICE',
        updateFixedRetailPrice: 'catalog/UPDATE_FIXED_RETAIL_PRICE',
        updateProductItemAvailability: 'catalog/UPDATE_PRODUCT_ITEM_AVAILABILITY',
      }),
      toggleProductItemsVisibility() {
        this.showProductItems = !this.showProductItems;
      },
      resellerPriceChange(newResellerPrice, productItemId) {
        this.updateResellerPrice({
          productItemId,
          newResellerPrice,
        });
      },
      retailPriceChange(newRetailPrice, productItemId) {
        this.updateRetailPrice({ productItemId, newRetailPrice });
      },
      fixedRetailPriceChange(fixedRetailPrice, productItemId) {
        this.updateFixedRetailPrice({ productItemId, fixedRetailPrice });
      },
      productItemAvailabilityChange(available, productItemId) {
        this.updateProductItemAvailability({ productItemId, available });
      },
      retailPriceDisabled(productItem) {
        return !productItem.available || (productItem.fixedRetailPrice && !this.canEditRetailPrice);
      },
      showWarning(productItem) {
        return (parseFloat(productItem.cost) > parseFloat(productItem.resellerPrice)
          || parseFloat(productItem.cost) > parseFloat(productItem.retailPrice)
          || (parseFloat(productItem.resellerPrice) > parseFloat(productItem.retailPrice)));
      },
      getWarningMessage(productItem) {
        let message = '';
        if (parseFloat(productItem.cost) > parseFloat(productItem.resellerPrice)) {
          message = 'Product cost is greater than reseller price.';
        } else if (parseFloat(productItem.cost) > parseFloat(productItem.retailPrice)) {
          message = 'Product cost is greater than retail price.';
        } else if (parseFloat(productItem.resellerPrice) > parseFloat(productItem.retailPrice)) {
          message = 'Product reseler price is greater than retail price.';
        }
        return message;
      },
    },
  };
</script>

<style scoped="true">
  .product-header table {
    border: 1px solid #d6e4f0 !important;
    background-color: #f2f7f8;
    cursor: pointer;
    height: 4.3rem;
  }

  .product-header table tr td.caret {
    padding: 0 1.5rem;
    font-size: 1.5rem;
    width: 6rem;
  }

  td.product-status {
    width: 13rem;
  }

  td.product-image {
    width: 11rem;
  }

  .product-image {
    height: 3rem;
    padding: 0.6rem;
  }

  .product-items table tr {
    height: 4rem;
  }

  .product-items table tr td.product-item-name {
    width: 60%;
    padding-left: 15rem;
  }

  .product-items table tr td input[type='text'] {
    width: 60%;
  }

  .product-items table tr td input[type='text'] {
    width: 60%;
  }

  .product-items table tr td input[type='text']:focus,
  .product-items table tr td input[type='text'].is-focused,
  .product-items table tr td input[type='text']:active,
  .product-items table tr td input[type='text'].is-active {
    border-color: #b5b5b5;
    -webkit-box-shadow: none;
    -moz-box-shadow: none;
    box-shadow: none;
  }

  span.currency,
  .pice-warning {
    vertical-align: -webkit-baseline-middle;
  }
</style>
