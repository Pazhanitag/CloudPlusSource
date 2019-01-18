import Vue from 'vue';

const scrolled = {
  bind(el) {
    el.addEventListener('scroll', () => {
      Vue.bus.emit('contentScrolled', el.scrollTop);
    });
  },
};

Vue.directive('scrolled', scrolled);

export default scrolled;
