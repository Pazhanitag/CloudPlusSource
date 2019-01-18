export default {
  customerDefaultState() {
    return {
      companyId: 0,
      domains: [],
      domain: '',
      name: '',
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: '',
      address1: '',
      address2: '',
      city: '',
      country: '',
      countryCode: '',
      state: '',
      stateCode: '',
      postalCode: '',
    };
  },
  resendTxtRecordsModelDefaultState() {
    return {
      email: '',
      domain: '',
    };
  },
  companyDefaultState() {
    return {
      companyId: 0,
      email: '',
      domains: [],
      office365CustomerId: '',
    };
  },
  userLicenceAssignmentDefaultState() {
    return {
      cloudPlusProductIdentifier: '',
      previouslyAssignedProductIdentifier: 0,
      usageLocation: 'US',
      userRoles: [],
      password: '',
    };
  },
  multiUserLicenceAssignmentDefaultState() {
    return {
      cloudPlusProductIdentifier: '',
      cloudPlusProductName: '',
      userRoles: [],
      users: [],
    };
  },
  transitionDataDefaultState() {
    return {
      office365CustomerId: '',
      companyId: 0,
      productId: 0,
      domains: [],
      productItems: [],
    };
  },
};
