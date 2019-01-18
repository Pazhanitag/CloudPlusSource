<template>
  <div class="field">
    <label class="label has-text-weight-semibold">
      <slot></slot>
    </label>
    <div class="control has-icons-right">
      <cloud-plus-password-input :value="value" @input="updateValue" :inputType="inputType[visibility]" :validationErrors="validationErrors">
      </cloud-plus-password-input>
      <span class="icon is-small is-right" v-show="!visibility" @click="toggleVisibility">
        <i class="fa fa-eye"></i>
      </span>
      <span class="icon is-small is-right" v-show="visibility" @click="toggleVisibility">
        <i class="fa fa-eye-slash"></i>
      </span>
    </div>
    <p class="help is-danger">{{validationErrors}}</p>
  </div>
</template>

<script>
import CloudPlusPasswordInput from './CloudPlusPasswordInput';

export default {
  props: ['value', 'validationErrors'],
  components: {
    CloudPlusPasswordInput,
  },
  data() {
    return {
      password: this.value,
      visibility: false,
      inputType: {
        true: 'text',
        false: 'password',
      },
    };
  },
  methods: {
    updateValue(inputValue) {
      this.$emit('input', inputValue);
    },
    toggleVisibility() {
      this.visibility = !this.visibility;
    },

  },
};
</script>

<style scoped lang="scss">
.control.has-icons-right .icon.is-right {
  pointer-events: inherit;
  cursor: pointer;
}

.icon.is-small.is-right {
  height: 100%;
}
</style>
