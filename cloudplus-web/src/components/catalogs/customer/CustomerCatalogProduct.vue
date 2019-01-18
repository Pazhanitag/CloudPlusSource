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
              <h2 class="is-size-7 has-text-weight-light"> For more details about this product, its packages and addons, <brand-color-primary class="link"><span v-on:click.stop="openProductDetails">click here</span></brand-color-primary></h2>
            </td>
            <td class="product-packages-count">
              <i class="fa fa-list" />
              <span>{{packagesCount}} Packages</span>
            </td>
            <td class="product-addons-count">
              <div>
                <i class="fa fa-level-up" />
                <span>{{addonsCount > 0 ? addonsCount : 'no' }} Addons</span>
              </div>
            </td>
            <td class="tri-td">
              <div class="tri-td-div" :class="[productAvailable ? 'with-product-available' : 'without-product-available']">
                <div>
                  <span v-if="!productAvailable">Inactive</span>
                  <span v-else>Activated</span>
                </div>
              <div class="product-activated-date" v-if="productAvailable">
                {{formatDate(product.activatedDate)}}
              </div>
              <div>
                <cloud-plus-switch
                  @input="toggleAvailability"
                  class="product-overview-switch-component"
                  v-on:click.stop
                  :class="{'disabled': (productAvailable || !productLoaded)}"
                  v-model="productAvailable"
                  :checked="productAvailable"
                  :disabled="productAvailable">
                </cloud-plus-switch>
              </div>
              </div>
            </td>
          </tr>
        </table>
      </div>
      <div class="product-items" v-show="showProductItems">
        <table class="table is-fullwidth">
          <tbody>
            <tr v-for="(productItem, index) in product.productItems" v-bind:key="index" v-bind:style="{'border-left': '4px solid ' + (productItem.isAddon ? '#e7995f' : '#2d8ac2')}">
              <td class="product-item-name">
                <span class="product-item-weight"> {{productItem.name}}<br></span>
                <div class="product-description">{{productDescription(productItem)}}... <brand-color-primary class="link"><span @click="openModal(productItem)">View more </span></brand-color-primary></div>
              </td>
              <td class="packages-padding">
                <span class="tag" :class="[productItem.isAddon ? 'is-addon' : 'is-info']">{{productItem.isAddon ? 'ADDON' : 'PACKAGE'}}</span>
              </td>
              <td>
                ${{productItem.cost | formatPrice}} per user
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <cloud-plus-card-modal v-if="expanded" :showModal="expanded" closeButtonName="Close" @closeModal="closeModal">
        <p slot="header">Detailed Product Info</p>
        <section class="main-modal-section" slot="section">
          <p class="margin-large bottom">
            <span class="subtitle">
              <strong>{{selectedProduct.name}}</strong>
            </span>
            <span class="tag is-package tag-margin" v-if="!selectedProduct.isAddon">
              PACKAGE
            </span>
            <span class="tag is-addon tag-margin" v-if="selectedProduct.isAddon">
              ADDON
            </span>
          </p>
          <span class="main-text" v-html="selectedProduct.description"></span>
        </section>
      </cloud-plus-card-modal>
      <confirmation-modal
        v-if="showProductActivationModal" :showModal="showProductActivationModal"
        @cancel="closeProductActivationModal"
        :cancelText="'Cancel'"
        :title="'Would you like to configure Office 365?'"
        @confirm = "confgureProduct"
        confirmText="Let’s do this!">You are about to activate Office 365 service for your organization. Before it will be available, there are a few things needed.
      </confirmation-modal>
    </div>
</template>

<script>
import { provisioningStatus } from '@/assets/constants/commonConstants';
import provisioningService from '@/services/provisioningService';
import moment from 'moment';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import CloudPlusSwitch from '@/components/shared/input-components/CloudPlusSwitch';
import { mapGetters } from 'vuex';

export default {
  props: {
    productId: {
      required: true,
      type: Number,
    },
  },
  components: {
    CloudPlusCardModal,
    CloudPlusSwitch,
    ConfirmationModal,
  },
  data() {
    return {
      showProductItems: false,
      expanded: false,
      selectedProduct: '',
      provisioningStatus: {
        notProvisioned: provisioningStatus.NotProvisioned,
      },
      productLoaded: false,
      productAvailable: false,
      showProductActivationModal: false,
    };
  },
  computed: {
    ...mapGetters({
      customerProducts: 'catalog/customerProducts',
      companyId: 'userAuth/companyId',
    }),
    product() {
      return this.customerProducts.find(product => product.id === this.productId);
    },
    packagesCount() {
      return this.product.productItems.filter(productItem => !productItem.isAddon).length;
    },
    addonsCount() {
      return this.product.productItems.filter(productItem => productItem.isAddon).length;
    },
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
        path: `/catalogs/customer/products/${this.productId}`,
      });
    },
    closeModal() {
      this.expanded = false;
    },
    openModal(product) {
      this.selectedProduct = product;
      this.expanded = true;
    },
    productDescription(product) {
      const startChar = product.description.indexOf('main-description');
      let spaceChar = false;
      let counter = startChar + 120;
      if (product.name === 'Microsoft Exchange Online - Kiosk') {
        return product.description.slice(startChar + 23, startChar + 90);
      }
      while (!spaceChar) {
        if (product.description.charAt(counter) === ' ') {
          spaceChar = true;
        } else {
          counter += 1;
        }
      }
      return product.description.slice(startChar + 23, counter);
    },
    getProductAvailability() {
      provisioningService.getProductAvailability(this.companyId, this.productId)
        .then(response => {
          this.productAvailable = response.data.result !== this.provisioningStatus.notProvisioned;
          this.productLoaded = true;
        });
    },
    toggleAvailability(productAvailable) {
      this.productAvailable = productAvailable;
      if (this.productAvailable) {
        this.showProductActivationModal = true;
      }
    },
    confgureProduct() {
      provisioningService.setProductAvailability(
        this.companyId,
        this.productId,
        this.productAvailable,
      )
        .then(() => {
          this.$router.push({
            name: 'myServices',
            params: { showModalOnOpen: true },
          });
        });
    },
    closeProductActivationModal() {
      this.showProductActivationModal = false;
      this.productAvailable = false;
    },
  },
  mounted() {
    this.getProductAvailability();
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
td.product-packages-count,
td.product-addons-count,
td.product-tag,
td.product-lowest-price {
  width: 11%;
}
td.product-activated-date{
  width: 3%;
}
td.product-details {
  width: 2%;
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
.product-items table tr td.product-item-name {
  padding-left: 15rem;
  width: 72%;
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
.switch{
  padding: 1.5rem 1rem 1rem 1rem;
}
.product-overview-switch-component {
  display: inline-block;
  padding: 0 0.5rem;
  font-weight: 600;
}
.product-item-weight{
  font-weight: 400;
  color: #585858;
}

.main-text {
  font-size: $secondary-font-size;
}

.tri-td-div >div {
  display: inline-block;
  width: 80px;
  vertical-align: middle;
}

.tri-td-div >div:last-child {
  text-align: center
}

.tri-td-div.without-product-available >div{
  width: 120px;
}

@media screen and (max-width: 2000px) {
  .product-items table tr td.product-item-name {
    padding-left: 16.5rem;
  }
  .packages-padding{
    padding: 0rem 0rem 0rem 1.7rem;
  }
}
@media screen and (max-width: 1700px) {
  .product-items table tr td.product-item-name {
    padding-left: 14.5rem;
  }
  .packages-padding{
    padding: 0rem 0rem 0rem 0.5rem;
  }
}
@media screen and (max-width: 1366px) {
  .product-items table tr td.product-item-name {
    padding-left: 12.5rem;
  }
}
@media screen and (max-width: 1020px) {
  .product-items table tr td.product-item-name {
    padding-left: 9rem;
  }
}
@media screen and (max-width: 800px) {
  .product-items table tr td.product-item-name {
    padding-left: 7rem;
  }
}
</style>
