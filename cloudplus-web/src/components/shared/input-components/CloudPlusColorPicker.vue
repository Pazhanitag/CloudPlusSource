<template>
  <div class="color-input">
    <div class="columns color-column">
      <div class="level-item">
        <div class="color-watch" @click="toggleColorPicker">
          <div class="color-watch__background" :style="{background:colors.hex}"></div>
        </div>
      </div>
      <div class="level-item">
        <div class="color-input__content">
          <div class="color-label">
            <slot name="label"></slot>
          </div>
          <div class="description">
            <slot name="description"></slot>
          </div>
        </div>
      </div>
    </div>
    <chrome-picker :disableAlpha="true" class="color-picker" v-if="visibility" v-model="colors" v-on-clickaway="closeColorPicker" @input="updateValue" />
  </div>
</template>

<script>
import { mixin as clickaway } from 'vue-clickaway';
import { Chrome } from 'vue-color';

export default {
  mixins: [clickaway],
  data() {
    return {
      colors: this.value,
      visibility: false,
    };
  },
  components: { 'chrome-picker': Chrome },
  props: ['value'],
  methods: {
    updateValue() {
      this.$emit('input', this.colors);
    },
    toggleColorPicker() {
      this.visibility = !this.visibility;
    },
    closeColorPicker() {
      this.visibility = false;
    },
  },
  watch: {
    value() {
      this.colors = this.value;
    },
  },
};
</script>

<style lang="scss" scoped>
.level-item {
  flex-shrink: inherit;
}
.color-input {
  margin-top: 1.875rem;
  margin-bottom: 1.25rem;
  position: relative;
  &__content {
    padding-left: 2.5rem;
    width: 100%;
  }
}

.color-column {
  margin-left: 0;
}

.color-watch {
  height: 40px;
  width: 40px;
  border-radius: 50%;
  border: 2px solid #898989;
  background-clip: content-box;
  padding: 3px;
  &__background {
    height: 30px;
    width: 30px;
    border-radius: 50%;
  }
}

.color-picker {
  position: absolute;
  z-index: 99;
  margin-top: -0.26rem;
  margin-left: -0.75rem;
}

.color-label {
  color: $label-color;
  font-weight: 600;
  font-size: $secondary-font-size;
  padding-bottom: 0.4375rem;
}

.description {
  font-size: $secondary-font-size;
  color: $subtitle-color;
}
</style>
