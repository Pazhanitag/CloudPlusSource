import companyService from '@/services/companyService';

export default {
  getBranding({ commit }) {
    return new Promise(resolve => {
      companyService.getCompanyBranding().then(response => {
        commit('SET_BRANDING', response.data.result);
        resolve();
      }).catch(() => {
        resolve();
      });
    });
  },
  getBrandingForUser({ commit }, userEmail) {
    return new Promise(resolve => {
      companyService.getCompanyBrandingForUser(userEmail).then(response => {
        commit('SET_BRANDING', response.data.result);
        resolve();
      }).catch(() => {
        resolve();
      });
    });
  },
};
