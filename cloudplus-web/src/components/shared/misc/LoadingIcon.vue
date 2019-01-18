<template>
  <component
    :is="this.rootElementType"
    class="has-text-centered">
    <i
      class="fa fa-spin fa-spinner"
      :class="cssClass"
      aria-hidden="true"
      :aria-label="label">
    </i>
  </component>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: false,
      default: '',
    },
    size: {
      type: String,
      required: false,
      default: '1',
    },
    inline: {
      type: Boolean,
      required: false,
      default: false,
    },
  },
  computed: {
    cssClass() {
      return `fa-${this.size}x`;
    },
    rootElementType() {
      return this.inline ? 'span' : 'div';
    },
  },
};
</script>

<style scoped>
span {
  margin: 0 0.2rem;
}
</style>
