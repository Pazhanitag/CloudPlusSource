import Vue from 'vue';

const visibleDirective = {
  bind(el, binding) {
    el.style.visibility = binding.value ? 'visible' : 'hidden';
  },
  update(el, binding) {
    el.style.visibility = binding.value ? 'visible' : 'hidden';
  },
};

Vue.directive('visible', visibleDirective);

export default visibleDirective;
