<template>
  <div class="level" @click="updateValue">
    <div class="level-left">
      <div class="level-item">
        <brand-input-icon>
          <input type="radio" class="radio-input" :name="name" :value="checkedValue" :checked="checked">
          <i v-show="this.checked" class="fa fa-dot-circle-o" aria-hidden="true"></i>
          <i v-show="!this.checked" class="fa fa-circle-o" aria-hidden="true"></i>
        </brand-input-icon>
      </div>
      <div class="level-item">
        <div>
          <label class="radio has-text-weight-semibold">
            <slot></slot>
          </label>
          <p class="is-size-7 has-text-weight-light" v-if="subtitle">{{subtitle}}</p>
        </div>
      </div>
      </div>
  </div>
</template>

<script>
export default {
  props: ['name', 'checkedValue', 'checked', 'subtitle'],
  methods: {
    updateValue() {
      this.$emit('input', this.checkedValue);
    },
  },
};
</script>

<style scoped lang="scss">
.radio {
  height: 1rem;
  font-size: $secondary-font-size;
  color: $label-color;
}

.radio-input {
  display: none;
}

p {
  overflow: hidden;
  text-overflow: ellipsis;
  width: 28rem;
  cursor: pointer;
}
</style>
