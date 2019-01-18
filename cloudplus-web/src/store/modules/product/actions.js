import catalogService from '@/services/catalogService';

export default {
  getProduct({ commit }) {
    return new Promise(resolve => {
      catalogService.getCustomerProducts().then(response => {
        commit('SET_PRODUCT', response.data.result[0]);
        resolve();
      });
    }).catch(e => {
      commit('SET_PRODUCT', {});
      console.log(e);
    });
  },
  getAllProducts({ commit }) {
    return new Promise(resolve => {
      catalogService.getCustomerProducts().then(response => {
        commit('SET_PRODUCTS', response.data.result);
        commit('SET_PRODUCT', response.data.result[0]);
        resolve();
      });
    }).catch(e => {
      commit('SET_PRODUCTS', {});
      commit('SET_PRODUCT', {});
      console.log(e);
    });
  },
};
