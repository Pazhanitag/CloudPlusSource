<template>
  <div class="product-overview">
    <div class="product-overview-header" v-if="showProductLogo || showOverviewNameAndVendor || hasSwitch">
      <div class="product-overview-logo" v-if="showProductLogo">
        <img :src="product.imgUrl" alt="">
      </div>
      <div class="product-overview-name-and-vendor" v-if="showOverviewNameAndVendor">
        <div class="product-overview-name">{{product.name}}</div>
        <div class="product-overview-vendor">{{product.vendor}}</div>
      </div>
      <div v-if="hasSwitch" class="product-overview-switch is-pulled-right">
        <span class="product-overview-switch-component" v-if="!productAvailable">Activate Product</span>
        <span class="product-overview-switch-component" v-else>Activated</span>
        <cloud-plus-switch @input="toggleAvailability" v-on:click.stop class="product-overview-switch-component" :class="{'disabled': (productAvailable || !productLoaded)}" v-model="productAvailable" :checked="productAvailable" :disabled="productAvailable"></cloud-plus-switch>
      </div>
    </div>
    <div class="product-overview-description">
      <cloud-plus-excerpt v-if="showDescription" :cutOffAtChar="3000">{{product.description}}</cloud-plus-excerpt>
      <slot></slot>
    </div>
    <!-- <div v-if="showDescription" class="product-overview-tags">
      <div class="tag is-product">
        PRODUCT
      </div>
    </div> -->
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import { provisioningStatus } from '@/assets/constants/commonConstants';
import CloudPlusExcerpt from '@/components/shared/CloudPlusExcerpt';
import CloudPlusSwitch from '@/components/shared/input-components/CloudPlusSwitch';
import provisioningService from '@/services/provisioningService';

export default {
  data() {
    return {
      productAvailable: this.productAvailableSwitch,
      productLoadedAvailable: false,
      provisioningStatus: {
        notProvisioned: provisioningStatus.NotProvisioned,
      },
      productLoaded: false,
    };
  },
  mounted() {
    if (this.hasSwitch) {
      this.getProductAvailability();
    }
  },
  props: {
    hasSwitch: {
      type: Boolean,
      default: false,
    },
    product: {
      required: true,
    },
    showDescription: {
      type: Boolean,
      default: true,
    },
    productAvailableSwitch: {
      type: Boolean,
    },
    showProductLogo: {
      type: Boolean,
      default: true,
    },
    showOverviewNameAndVendor: {
      type: Boolean,
      default: true,
    },
  },
  watch: {
    productAvailableSwitch() {
      this.productAvailable = false;
    },
  },
  components: {
    CloudPlusExcerpt,
    CloudPlusSwitch,
  },
  methods: {
    toggleAvailability() {
      this.$emit('availibilityChanged', this.productAvailable);
    },
    getProductAvailability() {
      provisioningService.getProductAvailability(this.companyId, this.product.id)
        .then(response => {
          this.productAvailable = response.data.result !== this.provisioningStatus.notProvisioned;
          this.productLoaded = true;
        });
    },
  },
  computed: {
    ...mapGetters({
      companyId: 'userAuth/companyId',
    }),
  },
};
</script>

<style lang="scss" scoped>
  .product-overview {

    .product-overview-header {
      display: flex;
      padding-bottom: 2rem;
      align-items: center;

      .product-overview-logo {
        padding-right: 2rem;
        display: flex;
        align-items: center;

        img {
          max-height: 2.5rem;
        }
      }

      .product-overview-name-and-vendor {
        .product-overview-name {
          font-weight: 600;
          padding-bottom: 0;
          font-size: 1.2rem;
        }

        .product-overview-vendor {
          padding-top:0;
        }

        display: inline-block;
        width: 100%;
      }
    }

    .product-overview-switch {
      white-space: nowrap;
      display: flex;
      align-items: center;
      justify-content: center;

      .product-overview-switch-component {
        display: inline-block;
        padding: 0 0.5rem;
        font-weight: 600;
      }
    }

    .product-overview-description {
      display: block;
      padding-bottom: 2rem;
      font-size: $secondary-font-size;
    }

    .product-overview-tags {
      display: block;
      padding-bottom: 3rem;
    }

    .disabled {
      pointer-events: none;
    }
  }
</style>
