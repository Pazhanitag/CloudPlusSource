import { userPasswordSetupMethod } from '@/assets/constants/commonConstants';

export default {
  userId: state => state.user.id,
  alternativeEmail: state => state.user.alternativeEmail,
  personalInformation: state => ({
    firstName: state.user.firstName,
    lastName: state.user.lastName,
    displayName: state.user.displayName,
    companyName: state.user.companyName,
    jobTitle: state.user.jobTitle,
  }),
  generalInformation: state => ({
    userName: state.user.userName,
    domain: state.user.domain,
    alternativeEmail: state.user.alternativeEmail,
    country: state.user.country,
    state: state.user.state,
    city: state.user.city,
    zipCode: state.user.zipCode,
    streetAddress: state.user.streetAddress,
    phoneNumber: state.user.phoneNumber,
    emailAddress: `${state.user.userName}@${state.user.domain}`,
  }),
  userSecurityInformation: state => ({
    passwordSetupMethod: state.user.passwordSetupMethod,
    password: state.user.password,
    passwordRetyped: state.user.passwordRetyped,
    passwordSetupEmail: state.user.passwordSetupEmail,
    passwordSetupEmailRetyped: state.user.passwordSetupEmailRetyped,
    sendPlainPasswordViaEmail: state.user.sendPlainPasswordViaEmail,
  }),
  userAccountOptions: state => ({
    userStatus: state.user.userStatus,
    sendWelcomeLetters: state.user.sendWelcomeLetters,
  }),
  userProfileImage: state => ({
    profilePicture: state.user.profilePicture,
    firstName: state.user.firstName,
  }),
  userRoles: state => ({
    roles: state.user.roles,
  }),
  userServices: state => state.userServices,
  createUser: state => ({
    companyId: state.user.companyId,
    firstName: state.user.firstName,
    lastName: state.user.lastName,
    displayName: state.user.displayName,
    companyName: state.user.companyName,
    jobTitle: state.user.jobTitle,
    userName: state.user.userName,
    domain: state.user.domain,
    alternativeEmail: state.user.alternativeEmail,
    country: state.user.country,
    countryCode: state.user.countryCode,
    state: state.user.state,
    city: state.user.city,
    zipCode: state.user.zipCode,
    streetAddress: state.user.streetAddress,
    phoneNumber: state.user.phoneNumber,
    emailAddress: `${state.user.userName}@${state.user.domain}`,
    passwordSetupMethod: state.user.passwordSetupMethod,
    password: state.user.password,
    passwordRetyped: state.user.passwordRetyped,
    passwordSetupEmail: state.user.passwordSetupEmail,
    passwordSetupEmailRetyped: state.user.passwordSetupEmailRetyped,
    sendPlainPasswordViaEmail: state.user.sendPlainPasswordViaEmail,
    userStatus: state.user.userStatus,
    sendWelcomeLetters: state.user.sendWelcomeLetters,
    avatarBase64: state.user.avatarBase64,
    roles: state.user.roles,
  }),
  updateUser: state => ({
    id: state.user.id,
    companyId: state.user.companyId,
    companyName: state.user.companyName,
    firstName: state.user.firstName,
    lastName: state.user.lastName,
    displayName: state.user.displayName,
    jobTitle: state.user.jobTitle,
    alternativeEmail: state.user.alternativeEmail,
    country: state.user.country,
    countryCode: state.user.countryCode,
    state: state.user.state,
    city: state.user.city,
    zipCode: state.user.zipCode,
    streetAddress: state.user.streetAddress,
    phoneNumber: state.user.phoneNumber,
    userStatus: state.user.userStatus,
    sendWelcomeLetters: state.user.sendWelcomeLetters,
    avatarBase64: state.user.avatarBase64,
    roles: state.user.roles,
  }),
  externalSignupForm: state => ({
    firstName: state.user.firstName,
    lastName: state.user.lastName,
    displayName: state.user.displayName,
    companyName: state.user.companyName,
    jobTitle: state.user.jobTitle,
    userName: state.user.userName,
    domain: state.user.domain,
    alternativeEmail: state.user.alternativeEmail,
    country: state.user.country,
    countryCode: state.user.countryCode,
    state: state.user.state,
    city: state.user.city,
    zipCode: state.user.zipCode,
    streetAddress: state.user.streetAddress,
    phoneNumber: state.user.phoneNumber,
    emailAddress: state.user.emailAddress,
    passwordSetupMethod: userPasswordSetupMethod.GeneratePasswordManually,
    password: state.user.password,
    passwordRetyped: state.user.passwordRetyped,
    passwordSetupEmail: '',
    passwordSetupEmailRetyped: '',
    userStatus: state.user.userStatus,
    sendWelcomeLetters: state.user.sendWelcomeLetters,
    avatarBase64: state.user.avatarBase64,
    roles: state.user.roles,
    emailUsername: state.user.emailUsername,
  }),
  changeUserPassword: state => ({
    userId: state.selectedUserId,
    passwordSetupMethod: state.user.passwordSetupMethod,
    password: state.user.password,
    passwordRetyped: state.user.passwordRetyped,
    passwordSetupEmail: state.user.passwordSetupEmail,
    passwordSetupEmailRetyped: state.user.passwordSetupEmailRetyped,
    sendPlainPasswordViaEmail: state.user.sendPlainPasswordViaEmail,
  }),
  updateUserProfile: (state, getters, rootState, rootGetters) => ({
    id: state.user.id,
    companyId: rootGetters['userAuth/userProfile'].companyId,
    companyName: state.user.companyName,
    firstName: state.user.firstName,
    lastName: state.user.lastName,
    displayName: state.user.displayName,
    jobTitle: state.user.jobTitle,
    alternativeEmail: state.user.alternativeEmail,
    country: state.user.country,
    countryCode: state.user.countryCode,
    state: state.user.state,
    city: state.user.city,
    zipCode: state.user.zipCode,
    streetAddress: state.user.streetAddress,
    phoneNumber: state.user.phoneNumber,
    userStatus: state.user.userStatus,
    sendWelcomeLetters: state.user.sendWelcomeLetters,
    avatarBase64: state.user.avatarBase64,
    roles: state.user.roles,
  }),
  basicUserInfo: state => ({
    id: state.user.id,
    profilePictureUrl: state.user.profilePicture,
    displayName: state.user.displayName !== undefined ? state.user.displayName : `${state.user.firstName} ${state.user.lastName}`,
    firstName: state.user.firstName,
    lastName: state.user.lastName,
    userRole: state.user.roleToDisplay,
    emailAddress: `${state.user.userName}@${state.user.domain}`,
    phoneNumber: state.user.phoneNumber,
    userStatus: state.user.userStatus,
    userStatusDisplay: state.user.userStatusDisplay,
    companyId: state.user.companyId,
  }),
  unsavedUserChangesPresent: state => state.unsavedUserChangesPresent,
  userServicesTimeout: state => state.userServicesTimeout,
};