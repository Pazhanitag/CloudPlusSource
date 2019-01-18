export default {
  bearerToken: state => state.bearerToken,
  userProfile: state => ({
    id: state.userProfile.sub,
    firstName: state.userProfile.given_name,
    lastName: state.userProfile.family_name,
    fullName: `${state.userProfile.given_name} ${state.userProfile.family_name}`,
    email: state.userProfile.email,
    userName: state.userProfile.prefered_username,
    companyId: state.userProfile.company_id,
    parentUserId: state.userProfile.parent_user_id,
    profilePicture: state.userProfile.picture,
    phoneNumber: state.userProfile.phone_number,
  }),
  companyId: state => state.userProfile.company_id,
  permissions: state => state.permissions,
  parentUserProfile: state => ({
    id: state.userProfile.sub,
    firstName: state.parentProfile.firstName,
    lastName: state.parentProfile.lastName,
    email: state.parentProfile.email,
    companyId: state.parentProfile.companyId,
    profilePicture: state.parentProfile.profilePicture,
  }),
  parentUserProfileRole: state => ({
    role: state.parentProfile.roles !== undefined ? state.parentProfile.roles[0].friendlyName : '',
  }),
  userProfileRole: state => ({
    role: state.userProfile.role !== undefined ? state.userProfile.role : '',
  }),
};
