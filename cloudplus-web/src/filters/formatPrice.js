import Vue from 'vue';
import { formatPrice } from '@/helpers/utils';

const formatPriceFilter = value => formatPrice(value);

Vue.filter('formatPrice', formatPriceFilter);

export default formatPriceFilter;
