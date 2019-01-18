import companyService from '@/services/companyService';
import supportService from '@/services/supportService';

export default {
  getCompany({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getCompany(companyId).then(result => {
        commit('SET_COMPANY', result.data.result);
        resolve(result);
      });
    });
  },
  createCscpUrl({ getters }) {
    return supportService.createCscpUrl(getters.createCscpUrlDetails);
  },
};
