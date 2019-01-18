import companyService from '@/services/companyService';

export default {
  createCompany({ getters }) {
    return companyService.createCompany(getters.createCompany);
  },
  createCompanyFromExternalSignupForm({ getters }) {
    return new Promise(resolve => {
      companyService.createCompanyFromExternalSignupForm(getters.externalSignupForm)
        .then(() => {
          resolve();
        }).catch(() => {
          resolve();
        });
    });
  },
  getCompanyDomains({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getCompanyDomains(companyId).then(response => {
        commit('SET_COMPANY_DOMAINS', response.data.result);
        resolve();
      }).catch(() => {
        resolve();
      });
    });
  },
  getCompanies({ commit }, payload) {
    return new Promise(resolve => {
      companyService.getCompanies(payload.companyId, payload.accountType).then(response => {
        commit('SET_COMPANIES', response.data.result);
        resolve();
      }).catch(() => {
        resolve();
      });
    });
  },
  getCompany({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getCompany(companyId).then(result => {
        commit('SET_COMPANY', result.data.result);
        resolve(result);
      });
    });
  },
  getParentCompanyByCompanyId({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getParentCompanyByCompanyId(companyId).then(result => {
        commit('SET_COMPANY', result.data.result);
        resolve(result);
      });
    });
  },
  updateCompany({ getters }) {
    return companyService.updateCompany(getters.updateCompany);
  },
  getPagedUsers({ commit }, params) {
    return new Promise(resolve => {
      companyService.getCompanyUsers(
        params.companyId,
        params.pageNumber,
        params.pageSize,
        params.orderBy,
        params.order,
        params.searchTerm,
      ).then(response => {
        commit('SET_PAGED_USERS', response.data.result);
        resolve();
      });
    });
  },
  getAllCompanyUsers({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getUsersByCompanyId(companyId).then(response => {
        commit('SET_COMPANY_USERS', response.data.result);
        resolve();
      });
    });
  },
  getParentBranding({ commit, rootGetters }) {
    return new Promise((resolve, reject) => {
      companyService.getCompany(rootGetters['userAuth/userProfile'].companyId).then(result => {
        commit('SET_COMPANY_BRANDING', result.data.result);
        resolve(result);
      }).catch(() => {
        reject();
      });
    });
  },
};
