<template>
  <brand-switch class="switch">
    <input type="checkbox" :checked="checked" @click="updateValue">
    <span class="slider round"></span>
  </brand-switch>
</template>

<script>
export default {
  props: {
    checked: {
      type: Boolean,
    },
  },
  methods: {
    updateValue() {
      this.$emit('input', !this.checked);
    },
  },
};
</script>

<style scoped lang="scss">
.switch {
  position: relative;
  display: inline-block;
  width: 2.5rem;
  height: 1.34rem;
}

.switch input {display:none;}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  -webkit-transition: .4s;
  transition: .4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 1.1rem;
  width: 1.1rem;
  left: 0.13rem;
  bottom: 0.15rem;
  background-color: white;
  -webkit-transition: .4s;
  transition: .4s;
}
input:checked + .slider:before {
  -webkit-transform: translateX(1.1rem);
  -ms-transform: translateX(1.1rem);
  transform: translateX(1.1rem);
}

.slider.round {
  border-radius: 34px;
}

.slider.round:before {
  border-radius: 50%;
}
</style>

