import Axios from 'axios';

export default {
  SET_AUTHENTICATION_DATA(state, authData) {
    state.bearerToken = authData.access_token;
    Axios.defaults.headers.common.Authorization = `Bearer ${state.bearerToken}`;
  },
  SET_USER_AUTHORIZATION_DATA(state, permissions) {
    state.permissions = permissions;
  },
  SET_PARENT_PROFILE(state, parentProfile) {
    state.parentProfile = parentProfile;
  },
  SET_USER_PROFILE(state, profileData) {
    state.userProfile = profileData;
  },
};
