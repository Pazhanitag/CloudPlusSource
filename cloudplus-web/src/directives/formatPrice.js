import Vue from 'vue';
import { formatPrice } from '@/helpers/utils';

const setFormattedPrice = (el, formattedPrice) => {
  el.value = formattedPrice;
  el.dispatchEvent(new Event('input'));
};
const formatPriceDirective = {
  bind(el) {
    setFormattedPrice(el, formatPrice(el.value));
    el.addEventListener('blur', () => {
      setFormattedPrice(el, formatPrice(el.value));
    });
  },
};

Vue.directive('format-price', formatPriceDirective);

export default formatPriceDirective;
