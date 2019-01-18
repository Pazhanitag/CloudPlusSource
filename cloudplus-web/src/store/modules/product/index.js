import getters from './getters';
import mutations from './mutations';
import actions from './actions';

export default {
  namespaced: true,
  state() {
    return {
      search: '',
      selectedProduct: null,
      activeProductIndex: -1,
      products: [],
      categories: [],
      selectedCategory: '',
    };
  },
  getters,
  mutations,
  actions,
};
