import getters from './getters';
import mutations from './mutations';
import actions from './actions';
import defaultStates from './initialStates';

export default {
  namespaced: true,
  state() {
    return {
      branding: defaultStates.brandingDefaultState(),
    };
  },
  getters,
  mutations,
  actions,
};
