import getters from './getters';
import mutations from './mutations';
import actions from './actions';

export default {
  namespaced: true,
  state() {
    return {
      userProfile: {},
      parentProfile: {},
      permissions: [],
      bearerToken: '',
    };
  },
  getters,
  mutations,
  actions,
};
