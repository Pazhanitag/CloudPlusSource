import catalogService from '@/services/catalogService';

export default {
  getResellerCatalogs({ commit }) {
    return new Promise((resolve, reject) => {
      catalogService.getResellerCatalogs().then(response => {
        commit('SET_RESELLER_CATALOGS', response.data.result);
        resolve(response);
      }).catch(() => {
        reject();
      });
    });
  },
  getResellerCatalog({ commit }, catalogId) {
    return new Promise(resolve => {
      catalogService.getResellerCatalog(catalogId).then(response => {
        commit('SET_RESELLER_CATALOG', response.data.result);
        resolve(response);
      });
    });
  },
  updateResellerCatalogPrices({ commit }, { catalogId, catalog }) {
    return new Promise((resolve, reject) => {
      catalogService.updateResellerCatalogPrices(catalogId, catalog).then(response => {
        commit('SET_RESELLER_CATALOG', response.data.result);
        resolve();
      }).catch(() => {
        reject();
      });
    });
  },
  getResellerCompaniesAssignedCatalogs({ commit }) {
    return new Promise((resolve, reject) => {
      catalogService.getResellerCompaniesAssignedCatalogs().then(response => {
        commit('SET_COMPANIES_ASSIGNED_CATALOGS', response.data.result);
        resolve();
      }).catch(() => {
        reject();
      });
    });
  },
  createCatalog({ commit }, newCatalog) {
    return new Promise((resolve, reject) => {
      catalogService.createNewCatalog(newCatalog).then(response => {
        commit('SET_RESELLER_CATALOGS', response.data.result);
        resolve();
      }).catch(() => {
        reject();
      });
    });
  },
  deleteCatalog({ commit }, catalogId) {
    return new Promise((resolve, reject) => {
      catalogService.deleteCatalog(catalogId).then(response => {
        commit('SET_RESELLER_CATALOGS', response.data.result);
        resolve();
      }).catch(() => {
        reject();
      });
    });
  },
  getCustomerProducts({ commit }) {
    return new Promise(resolve => {
      catalogService.getCustomerProducts().then(response => {
        commit('SET_CUSTOMER_PRODUCTS', response.data.result);
        resolve();
      });
    });
  },
  changeDefaultCatalog({ commit }, newDefaultCatalogId) {
    return new Promise((resolve, reject) => {
      catalogService.changeDefaultCatalog(newDefaultCatalogId).then(response => {
        commit('SET_RESELLER_CATALOGS', response.data.result);
        resolve();
      }).catch(() => {
        reject();
      });
    });
  },
};
