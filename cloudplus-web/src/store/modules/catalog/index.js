import getters from './getters';
import mutations from './mutations';
import actions from './actions';
import defaultStates from './initialStates';

export default {
  namespaced: true,
  state() {
    return {
      resellerCatalogs: defaultStates.resellerCatalogsDefaultState(),
      resellerCatalog: defaultStates.resellerCatalogDefaultState(),
      companiesAssignedCatalogs: defaultStates.companiesAssignedCatalogsDefaultState(),
      customerProducts: {

      },
    };
  },
  getters,
  mutations,
  actions,
};
