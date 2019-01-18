import getters from './getters';
import mutations from './mutations';
import actions from './actions';
import defaultStates from './initialStates';

export default {
  namespaced: true,
  state() {
    return {
      user: defaultStates.userDefaultState(),
      selectedUserId: 0,
      userServices: defaultStates.userServicesDefaultState(),
      userServicesTimeout: 0,
      unsavedUserChangesPresent: false,
    };
  },
  getters,
  mutations,
  actions,
};
