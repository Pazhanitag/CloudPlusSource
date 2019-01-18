<template>
  <div class="level-item ">
     <p class="level-item-padding" @click="sortBy(orderValue.value)" v-bind:class="{'has-text-weight-semibold' : (orderBy == orderValue)}" aria-hidden="true">
       <a class="padding-right" :style="{color: isSortBy(orderValue.value)}">
         <span class="order-item-title" :class="{'has-text-weight-semibold': isSortBy(orderValue.value)}">Order by {{orderValue.display}}</span>
       </a>
      <i class="fa fa-angle-up padding-right-tab" :class="{'has-text-weight-semibold': isSortBy(orderValue.value)}" :style="{color: isSortBy(orderValue.value)}" v-if="isOrderByDesc(orderValue.value)" aria-hidden="true"></i>
    <i class="fa fa-angle-down padding-right-tab" :class="{'has-text-weight-semibold': isSortBy(orderValue.value)}" :style="{color: isSortBy(orderValue.value)}" v-else></i>
   </p>
  </div>
</template>
<script>
export default {
  props: ['orderBy', 'order', 'orderValue'],
  data() {
    return {};
  },
  methods: {
    sortBy(orderBy) {
      this.$emit('sortBy', orderBy);
    },
    isOrderByDesc(orderByAttribute) {
      return this.orderBy === orderByAttribute && this.order !== 'asc';
    },
    isSortBy(orderBy) {
      return this.orderBy === orderBy ? this.$store.getters['branding/brand'].primaryColor : '';
    },
  },
};
</script>
<style scoped>
.padding-right {
  padding-right: 0.3125rem;
}
.padding-right-tab {
  padding-right: 1.5rem;
}
.level-item-padding{
  margin-bottom: 0.600rem;
  margin-top: 0.800rem;
}
.order-item-title {
  font-size: 0.8rem;
}
</style>
