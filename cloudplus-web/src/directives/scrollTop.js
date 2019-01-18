import Vue from 'vue';

const scrollTop = {
  bind() {
    document.getElementsByClassName('main-content')[0].scrollTop = 0;
  },
};

Vue.directive('scroll-top', scrollTop);
