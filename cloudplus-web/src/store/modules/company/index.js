import getters from './getters';
import mutations from './mutations';
import actions from './actions';
import defaultStates from './initialStates';

export default {
  namespaced: true,
  state() {
    return {
      company: defaultStates.companyDefaultState(),
      companies: [],
      pagedUsers: defaultStates.pagedUsersDefaultState(),
      productCustomerCompany: defaultStates.productCustomerCompanyDefaultState(),
      companyUsers: [],
    };
  },
  getters,
  mutations,
  actions,
};
