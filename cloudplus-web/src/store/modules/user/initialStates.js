import { userPasswordSetupMethod, userStatus } from '@/assets/constants/commonConstants';
import countries from '@/assets/constants/countries';

export default {
  userDefaultState() {
    return {
      passwordSetupMethod: userPasswordSetupMethod.GeneratePasswordViaLink,
      firstName: '',
      lastName: '',
      displayName: '',
      companyName: '',
      jobTitle: '',
      state: '',
      city: '',
      zipCode: '',
      streetAddress: '',
      alternativeEmail: null,
      phoneNumber: '',
      country: countries[229].name,
      countryCode: '',
      userName: '',
      domain: '',
      password: '',
      passwordRetyped: '',
      passwordSetupEmail: null,
      passwordSetupEmailRetyped: null,
      sendPlainPasswordViaEmail: false,
      roles: [],
      userStatus: userStatus.Active,
      sendWelcomeLetters: true,
      avatarBase64: '',
      profilePicture: '',
      emailAddress: '',
      companyId: -1,
      emailUsername: '',
      userStatusDisplay: '',
      roleToDisplay: '',
    };
  },
  userServicesDefaultState() {
    return {
      categoryId: 0,
      imgUrl: '',
      name: '',
      statusToDisplay: '',
      status: '',
      vendor: '',
      assignedLicense: '',
    };
  },
};
