export default {
  apiUrl: 'http://localhost:63145/api/',
  authConfig: {
    authority: 'http://localhost:57161/cloudplus/',
    clientId: 'cloudplusAdminPortal',
    redurectUri: `${window.location.origin}/static/callback.html`,
    silent_redirect_uri: 'http://localhost:8080/static/silent.html',
    expiringNotificationTime: 10,
    automaticSilentRenew: true,
    responseType: 'id_token token',
    scope: 'openid read',
    postLogoutRedirectUri: window.location.origin,
    forgotPasswordRoute: 'forgotPassword',
    changePasswordRoute: 'changePassword',
    activeDirectoryEndpoint: 'http://192.168.73.175:8585/',
  },

  // Users List Form Settings
  usersPerPage: 10,
  editUserRoute: 'users/edit',
  createNewUserRoute: 'users/create',
  externalResellerSignupForm: 'externalResellerSignupForm',
  externalCustomerSignupForm: 'externalCustomerSignupForm',
  imagePlaceholder: 'http://localhost:63145/Static/Images/Placeholders/128x128.png',
  imagePlaceholderCircle: 'http://localhost:63145/Static/Images/Placeholders/128x128circle.png',
  // Products Forms Settings
  productDetails: 'product-details',
  googleRecaptchaSiteKey: '6LfmTTMUAAAAAFdQzcGOC49mzUUpolN6WM3HhfGG',
  customerRole: 'CustomerAdmin',
  resellerRole: 'ResellerAdmin',
  masterRole: 'MasterAdmin',
  userRole: 'User',
  companyLogoUrl: 'https://cpstgcp.cloudplusstaging.review/CloudPlus.Api/Static/Images/CompanyLogo/',
};
