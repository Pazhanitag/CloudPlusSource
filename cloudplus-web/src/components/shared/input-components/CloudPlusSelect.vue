<template>
  <div class="field" :class="{ 'field--stretch': stretch }">
    <label v-if="hasLabel" class="label has-text-weight-semibold">
      <slot></slot>
    </label>
    <div class="control  is-size-7" v-bind:class="{ 'has-icons-left': icon }">
      <div class="select" :class="computedSize">
        <brand-select>
          <select :disabled="disabled" @input="onChange($event.target.value)" :class="computedSize">
            <option :key="index" v-for="(option, index) in options" :value="option.value||option" :selected="selected == option.value || selected == option">{{option.name || option}}</option>
          </select>
        </brand-select>
      </div>
      <div v-if="icon" class="icon is-small is-left">
        <i class="fa" :class="icon"></i>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    options: {
      type: Array,
      required: true,
    },
    selected: {
    },
    icon: String,
    size: {
      type: String,
      default: '',
    },
    stretch: {
      type: Boolean,
      default: false,
    },
    hasWarning: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    hasLabel() {
      return !!this.$slots.default;
    },
    computedSize() {
      // eslint-disable-next-line
      const theClass = (this.$props.size) ? `is-size--${this.$props.size.toLowerCase()}` : '';
      return {
        [theClass]: true,
        warning: this.hasWarning,
      };
    },
  },
  methods: {
    onChange(value) {
      this.$emit('input', value);
    },
  },
};
</script>

<style lang="scss" scoped>
.select,
select {
  width: 100%;
  height: $input-fields-height;
  border-radius: $border-radius;
  font-size: 0.8rem !important;
}

select {
  padding-left: 0.75rem;
}

.is-size {
  &--s, &--small {
    height: 1.5rem;
  }

  &--n, &--normal, &--m, &--medium {
    height: $input-fields-height;
  }
  
  &--l, &--large {
    height: 4rem;
  }
}

.field{
  &--stretch {
    width: 100%;
  }
}

.control.has-icons-left .icon {
  height: $input-fields-height;
}

.select:not(.is-multiple)::after {
  border: $border-height solid grey;
  border-right: 0;
  border-top: 0;
}

.icon i {
  color: black;
}

.warning {
  border-color: red;
}
</style>
