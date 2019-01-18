import userService from '@/services/userService';
import userUtilitiesService from '@/services/userUtilitiesService';

export default {
  getUser({ commit }, userId) {
    return new Promise(resolve => {
      userService.getUser(userId).then(response => {
        commit('SET_USER', response);
        resolve(response);
      });
    });
  },
  createUser({ getters }) {
    return userService.createUser(getters.createUser);
  },
  updateUser({ getters }) {
    return userService.updateUser(getters.updateUser);
  },
  changePassword({ getters }) {
    return userUtilitiesService.changePassword(getters.changeUserPassword);
  },
  getUserProfile({ commit }) {
    return new Promise(resolve => {
      userService.getUserProfile().then(response => {
        commit('SET_USER', response.data.result);
        resolve(response);
      });
    });
  },
  updateUserProfile({ getters }) {
    return userService.updateUserProfile(getters.updateUserProfile);
  },
  getUserServices({ commit }, { username, company }) {
    return new Promise(resolve => {
      userService.getUserServices(username, company).then(response => {
        commit('SET_USER_SERVICES', response.data.result);
        resolve(response);
      });
    });
  },
};
