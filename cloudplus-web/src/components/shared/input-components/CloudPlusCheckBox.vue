<template>
  <div class="level checkbox-container" @click="updateValue">
    <div class="level-left">
      <div class="level-item">
        <div class="checkbox-icon">
          <brand-input-icon>
            <input type="checkbox" class="checkbox-input" :disabled="disabled" :value="value">
            <i v-show="value" :style="{color: isDisabled()}" class="fa fa-check-square" aria-hidden="true"></i>
            <i v-show="!value" :style="{color: isDisabled()}" class="fa fa-square-o" aria-hidden="true"></i>
          </brand-input-icon>
        </div>
      </div>
      <div class="level-item label-allignment">
        <div>
          <label class="checkbox-label">
            <slot></slot>
          </label>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    value: {
      type: Boolean,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    updateValue() {
      if (!this.disabled) {
        this.$emit('input', !this.value);
      }
    },
    isDisabled() {
      return this.disabled ? '#989797' : '';
    },
  },
};
</script>

<style scoped lang="scss">
  .fa {
    padding-left: 0.125rem;
    font-size: 1rem;
  }

  .checkbox-container {
    display:table;
    width: 100%;
  }

  .checkbox-icon {
    width: 2rem;
    max-width: 2.81rem;
    line-height: $secondary-font-size;
  }

  .checkbox-input {
    display: none;
  }

  .checkbox-label {
    height: 1rem;
    font-size: $secondary-font-size;
    color: $label-color;
  }

  .label-allignment {
    margin-top: -5px;
  }
</style>

