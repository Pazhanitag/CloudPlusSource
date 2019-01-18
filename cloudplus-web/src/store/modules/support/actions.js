import supportService from '@/services/supportService';

export default {
  getSupportProductItems({ commit }, companyId) {
    return new Promise(resolve => {
      supportService.getSupportProductItems(companyId).then(response => {
        commit('SET_SUPPORT_PRODUCT_ITEMS', response.data.result);
        resolve();
      });
    }).catch(e => {
      commit('SET_SUPPORT_PRODUCT_ITEMS', []);
      console.log(e);
    });
  },
};

