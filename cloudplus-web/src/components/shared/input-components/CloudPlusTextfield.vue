<template>
  <div class="field">
    <label v-if="hasLabel" class="label has-text-weight-semibold">
      <slot></slot>
    </label>
    <div class="control" :class="{'is-loading': isLoading, 'has-icons-right' : hasIconRight, 'has-icons-left': hasIconLeft}">
      <brand-input>
        <input :type="type" ref="input" class="input is-size-7" :placeholder="placeholder" :value="value" @input="updateValue($event.target.value)" @focus="onFocus($event.target.value)" :disabled="disabled">
      </brand-input>
      <span v-if="hasIconRight" class="icon is-small is-right">
        <i class="fa" :class="icon"></i>
      </span>
      <span v-if="hasIconLeft" class="icon is-small is-left">
        <i class="fa" :class="icon"></i>
      </span>
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
    isLoading: {
      value: Boolean,
      default: false,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    hasIconLeft: {
      type: Boolean,
      default: false,
    },
    hasIconRight: {
      type: Boolean,
      default: false,
    },
    icon: {
      type: String,
      default: '',
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
  created() {
    this.$bus.on('changeFocus', this.changeFocus);
    this.$bus.on('fieldFocus', this.fieldFocus);
  },
  data() {
    return {
      focus: false,
    };
  },
  methods: {
    updateValue(inputValue) {
      this.$emit('input', inputValue);
    },
    onFocus(inputValue) {
      this.$emit('focus', inputValue);
    },
    fieldFocus() {
      if (this.focus && this.validationErrors) {
        if (this.$refs.input !== undefined) {
          this.$bus.emit('changeFocus', this.changeFocus);
          this.$refs.input.focus();
        }
      }
    },
    changeFocus() {
      this.focus = !this.focus;
    },
  },
};
</script>

<style lang="scss" scoped>
.input {
  height: $input-fields-height;
  border-radius: $border-radius;
  padding-left: 0.75rem;
  font-size: 0.8rem !important;
}

.icon.is-small.is-right {
  height: 100%;
}

</style>
