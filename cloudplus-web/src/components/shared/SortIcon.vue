<template>
  <div>
    <i class="fa fa-sort" aria-hidden="true" v-if="sortSymbol(orderByDefault, 1)"></i>
    <i class="fa fa-sort-asc" aria-hidden="true" v-if="sortSymbol(orderBy, 1)"></i>
    <i class="fa fa-sort-desc" aria-hidden="true" v-if="sortSymbol(orderBy, -1)"></i>
  </div>
</template>

<script>
export default {
  props: {
    orderByReference: {
      type: String,
      default: '',
      required: true,
    },
    orderReference: {
      type: Number,
      default: '',
      required: true,
    },
    orderBy: {
      type: String,
      default: '',
      required: true,
    },
    orderByDefault: {
      type: String,
      required: true,
    },
  },
  methods: {
    sortSymbol(orderBy, order) {
      return this.$props.orderByReference === orderBy && this.$props.orderReference === order;
    },
  },
};
</script>

<style lang="scss" scoped>
.fa-sort, .fa-sort-asc, .fa-sort-desc {
  margin-left: 50%;
}
</style>
