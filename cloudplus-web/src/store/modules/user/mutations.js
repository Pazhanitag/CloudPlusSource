import { userServiceStatus } from '@/assets/constants/commonConstants';
import defaultStates from './initialStates';

export default {
  UPDATE_USER_PROPERTY(state, { key, value }) {
    state.user[key] = value;
  },
  SET_USER(state, user) {
    state.user.id = user.id;
    state.user.firstName = user.firstName;
    state.user.lastName = user.lastName;
    state.user.displayName = user.displayName;
    state.user.companyName = user.companyName;
    state.user.jobTitle = user.jobTitle;
    state.user.state = user.state;
    state.user.city = user.city;
    state.user.zipCode = user.zipCode;
    state.user.streetAddress = user.streetAddress;
    state.user.alternativeEmail = user.alternativeEmail;
    state.user.phoneNumber = user.phoneNumber;
    state.user.country = user.country !== undefined && user.country !== null ?
      user.country : defaultStates.userDefaultState().country;
    state.user.userName = user.userName;
    state.user.domain = user.domain;
    state.user.passwordSetupEmail = user.alernativeEmail;
    state.user.passwordSetupEmailRetyped = user.alernativeEmail;
    state.user.roles = (user.roles && user.roles.length > 0) ? [user.roles[0].id] : null;
    state.user.roleToDisplay = (user.roles && user.roles.length > 0) ? user.roles[0].name : null;
    state.user.profilePicture = user.profilePicture;
    state.user.userStatus = user.userStatus;
    state.user.userStatusDisplay = user.userStatusDisplay;
    state.user.companyId = user.companyId;
  },
  RESET_USER_STATE(state) {
    state.user = defaultStates.userDefaultState();
  },
  SET_USER_COMPANY(state, companyId) {
    state.user.companyId = companyId;
  },
  SET_SELECTED_USER_ID(state, userId) {
    state.selectedUserId = userId;
  },
  SET_USER_DOMAIN(state, domain) {
    state.user.domain = domain;
  },
  RESET_SECURITY_STATE(state) {
    state.user.password = '';
    state.user.passwordRetyped = '';
    state.user.sendPlainPasswordViaEmail = false;
    state.user.passwordSetupEmail = '';
    state.user.passwordSetupEmailRetyped = '';
  },
  SET_USER_SERVICES(state, userServices) {
    state.userServices = userServices;
  },
  SET_USER_SERVICE_STATUS_TO_IN_PROGRESS(state) {
    state.userServices.status = userServiceStatus.InProgress;
    state.userServices.statusToDisplay = 'InProgress';
  },
  RESET_USER_SERVICE_STATE(state) {
    state.userServices = defaultStates.userServicesDefaultState();
  },
  UNSAVED_USER_CHANGES_PRESENT(state) {
    state.unsavedUserChangesPresent = true;
  },
  RESET_UNSAVED_USER_CHANGES(state) {
    state.unsavedUserChangesPresent = false;
  },
  SET_USER_SERVICES_TIMEOUT(state, timeout) {
    state.userServicesTimeout = timeout;
  },
};
