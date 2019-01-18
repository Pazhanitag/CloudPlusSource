import getters from './getters';
import mutations from './mutations';
import actions from './actions';
import defaultStates from './initialStates';

export default {
  namespaced: true,
  state() {
    return {
      reseller: defaultStates.resellerDefaultState(),
      company: defaultStates.companyDefaultState(),
      cscpUrl: 'my.[accountdomain.com]',
      cscpDetailsForConfiguration: {},
    };
  },
  getters,
  mutations,
  actions,
};
