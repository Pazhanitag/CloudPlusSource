<template>
  <div class="field">
    <label v-if="hasLabel" class="label has-text-weight-semibold">
      <slot></slot>
    </label>
    <div class="control">
      <brand-input>
        <textarea :type="type" class="textarea"  :rows="rows" :placeholder="placeholder" :value="value" @input="updateValue($event.target.value)" @focus="onFocus($event.target.value)" :disabled="disabled"></textarea>
      </brand-input>
    </div>
    <p class="help is-danger">{{validationErrors}}</p>
  </div>
</template>

<script>
export default {
  props: {
    value: String,
    type: {
      default: 'text',
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    rows: {
      default: 10,
      type: Number,
    },
    validationErrors: String,
    placeholder: {
      type: String,
      default: '',
    },
  },
  computed: {
    hasLabel() {
      return !!this.$slots.default;
    },
  },
  methods: {
    updateValue(inputValue) {
      this.$emit('input', inputValue);
    },
    onFocus(inputValue) {
      this.$emit('focus', inputValue);
    },
  },
};
</script>

<style lang="scss" scoped>
.input {
  border-radius: $border-radius;
}
@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .textarea{
    min-height: 10rem;
  }
}
</style>
