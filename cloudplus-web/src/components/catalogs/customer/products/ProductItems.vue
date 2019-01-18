<template>
  <div class="columns is-multiline product-items">
    <div class="column is-half product-item" v-for="(productItem, key) in mainProductItems" :key="key">
      <product-item-card :product="productItem"></product-item-card>
    </div>
  </div>
</template>

<script>
import { Tabs, TabPane } from '@/components/shared/tabs';
import ProductItemCard from './ProductItemCard';

export default {
  components: {
    Tabs,
    TabPane,
    ProductItemCard,
  },
  props: {
    product: {
      type: Object,
    },
  },
  computed: {
    mainProductItems() {
      return this.productItems(false);
    },
    addons() {
      return this.productItems(true);
    },
  },
  methods: {
    productItems(isAddon) {
      return this.product ? this.product.productItems.filter(item => item.isAddon === isAddon) : [];
    },
  },
};
</script>

<style lang="scss" scoped>
.product-items {
  margin: 0 1.3rem 0 1.3rem;
}
.product-item {
    padding-top: 2rem;

    .product-item-description {
      display: block;
      padding-bottom: 2rem;
      font-size: 0.93rem;
    }
  }
</style>
