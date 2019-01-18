<template>
  <div class="product-item-card">
    <div class="columns">
      <div class="column is-8 product-card-header">
          <div class="subtitle product-name">
            <strong>{{product.name}}</strong>
          </div>
          <div class="tag is-package" v-if="!product.isAddon">
            PACKAGE
          </div>
          <div class="tag is-addon" v-if="product.isAddon">
            ADDON
          </div>
      </div>
      <div class="product-cost column is-4 is-pulled-left">
        <span><span>${{product.cost | formatPrice}}</span> per user</span>
      </div>
    </div>
    <div>
      <cloud-plus-excerpt-modal :cutOffAtChar="getCutOffLength(product.description)" title="Detailed Product Info" :value="product.description">
        <div class="margin-large product-card-header product-card-header__modal">
          <div class="subtitle product-name">
            <strong>{{product.name}}</strong>
          </div>
          <div class="tag is-package" v-if="!product.isAddon">
            PACKAGE
          </div>
          <div class="tag is-addon" v-if="product.isAddon">
            ADDON
          </div>
        </div>
      </cloud-plus-excerpt-modal>
    </div>
    <div class="add-on-info">
      Currently, there are no add ons available for this product.
    </div>
  </div>
</template>

<script>
import CloudPlusExcerptModal from '@/components/shared/modals/CloudPlusExcerptModal';

export default {
  components: {
    CloudPlusExcerptModal,
  },
  props: {
    product: {
      required: true,
    },
  },
  methods: {
    iconsLength(description) {
      return description.indexOf('main-description');
    },
    getCutOffLength(description) {
      return 230 + this.iconsLength(description);
    },
  },
};
</script>

<style lang="scss" scoped>
.product-item-card {
  width: 100%;
  height: 100%;
  background-color: white;
  padding: 2rem;
  min-height: 22rem;
  position: relative;
}

@media only screen and (max-width: 1820px) {
  .product-item-card {
    min-height: 24rem;
  }
}

@media only screen and (max-width: 1600px) {
  .product-item-card {
    min-height: 22rem;
  }
}

.add-on-info {
  position: absolute;
  bottom: 1.75rem;
  font-size: $secondary-font-size;
}
.product-cost {
  text-align: right;
  padding-left: 0rem;
  padding-right: 0rem;
}
.product-cost span span {
  font-size: 1.25rem;
  font-weight: 700;
}
.product-name {
  margin-bottom: 0;
}
.tag {
  margin-left: 0.5rem;
}
.product-card-header {
    display: flex;
    align-items: center;
    &__modal {
      margin-top: 1.5rem;
      margin-bottom: 2rem;
    }
}
</style>

