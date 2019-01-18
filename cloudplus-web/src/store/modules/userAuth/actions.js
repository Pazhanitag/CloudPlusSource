import authService from '@/services/authService';
import userService from '@/services/userService';

export default {
  authenticate({ dispatch, commit }) {
    return new Promise(resolve => {
      authService.authenticateUser().then(response => {
        commit('SET_AUTHENTICATION_DATA', response);
        dispatch('reloadUserProfile').then(() => resolve());
      });
    });
  },
  loadPermissions({ commit }, userId) {
    return new Promise(resolve => {
      userService.getUserPermissions(userId).then(response => {
        commit('SET_USER_AUTHORIZATION_DATA', response);
        resolve();
      });
    });
  },
  getParentProfileData({ commit }) {
    return new Promise(resolve => {
      userService.getParentProfileData().then(response => {
        commit('SET_PARENT_PROFILE', response.data.result);
        resolve();
      });
    });
  },
  reloadUserProfile({ commit }) {
    return new Promise(resolve => {
      authService.loadUserInfo().then(response => {
        commit('SET_USER_PROFILE', response.data);
        resolve();
      });
    });
  },
};
