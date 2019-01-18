<template>
  <div>
    <div>Please enter your primary or alternative email which you want to use for password recovery:</div>
    <div class="separator-30"></div>
    <div class="field">
      <label class="label">
        <slot></slot>
      </label>
      <div class="control has-icons-right">
        <cloud-plus-text-input v-model="email" @input="updateValue(email)"  data-vv-name="email" v-validate="'required|email|user_exists'" name="email" data-vv-as="Email" :validationErrors="errors.first('email')">
        </cloud-plus-text-input>
      </div>
      <p class="help is-danger">{{errorsParent}}</p>
    </div>
  </div>
</template>

<script>
import CloudPlusTextInput from '../shared/input-components/CloudPlusTextfield';

export default {
  inject: {
    $validator: '$validator',
  },
  props: ['value', 'errorsParent'],
  components: {
    CloudPlusTextInput,
  },
  data() {
    return {
      email: this.value,
    };
  },
  methods: {
    updateValue(inputValue) {
      this.$emit('input', inputValue);
    },
  },
};
</script>

<style scoped>

</style>
