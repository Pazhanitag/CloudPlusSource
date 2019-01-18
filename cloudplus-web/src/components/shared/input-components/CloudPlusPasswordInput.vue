<template>
  <div>
    <brand-input>
      <input class="input is-size-7"
            :class="{'is-danger': hasErrors}"
            :type="inputType"
            :value="value"
             ref="input"
             @input="updateValue($event.target.value)">
    </brand-input>
  </div>
</template>

<script>
export default {
  props: ['value', 'inputType', 'hasErrors', 'validationErrors'],
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
}
</style>
