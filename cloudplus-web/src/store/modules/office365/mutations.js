import { usStates } from '@/assets/constants/states';
import defaultStates from './initialStates';

export default {
  SET_CUSTOMER(state, company) {
    state.customer.domains = company.domains;
    state.customer.domain = company.domains[0].name;
    state.customer.name = company.name;
    state.customer.phoneNumber = company.phoneNumber;
    state.customer.address1 = company.streetAddress;
    state.customer.city = company.city;
    state.customer.country = company.country;
    state.customer.state = company.state;
    state.customer.postalCode = company.zipCode;
    state.customer.email = company.email;

    const custState = usStates.find(s => s.name.toLowerCase() === company.state.toLowerCase() ||
        s.abbreviation.toLowerCase() === company.state.toLowerCase());

    if (custState) {
      state.customer.stateCode = custState.abbreviation;
    }
  },
  UPDATE_CUSTOMER_PROPERTY(state, { key, value }) {
    state.customer[key] = value;
  },
  RESET_CUSTOMER_STATE(state) {
    state.customer = defaultStates.customerDefaultState();
  },
  SET_RESEND_TXT_RECORDS_DOMAIN(state, domain) {
    state.resendTxtRecordsModel.domain = domain;
  },
  SET_RESEND_TXT_RECORDS_EMAIL(state, email) {
    state.resendTxtRecordsModel.email = email;
  },
  SET_COMPANY(state, company) {
    state.company = company;
  },
  SET_SELECTED_DOMAIN(state, domain) {
    state.selectedDomain = domain;
  },
  SET_DOMAIN_USERS(state, { users, index }) {
    state.company.domains[index].users = users;
  },
  SET_USER_LICENCE_PRODUCT_IDENTIFIER(state, productIdentifier) {
    state.userLicence.cloudPlusProductIdentifier = productIdentifier;
  },
  SET_USER_LICENCE_ROLES(state, roles) {
    state.userLicence.userRoles = roles;
  },
  SET_USER_LICENSE_PASSWORD(state, password) {
    state.userLicence.password = password;
  },
  SET_INITIALLY_ASSIGNED_PRODUCT_IDENTIFIER(state, productIdentifier) {
    state.userLicence.previouslyAssignedProductIdentifier = productIdentifier;
    state.userLicence.cloudPlusProductIdentifier = productIdentifier;
  },
  SET_MULTI_USER_LICENCE_PRODUCT_IDENTIFIER(state, productIdentifier) {
    state.multiUserLicence.cloudPlusProductIdentifier = productIdentifier;
  },
  SET_MULTI_USER_LICENCE_ROLES(state, roles) {
    state.multiUserLicence.userRoles = roles;
  },
  SET_MULTI_USER_LICENCE_USERS(state, users) {
    state.multiUserLicence.users = users;
  },
  SET_MULTI_USER_PRODUCT_NAME(state, productName) {
    state.multiUserLicence.cloudPlusProductName = productName;
  },
  RESET_MULTI_USER_LICENCE(state) {
    state.multiUserLicence = defaultStates.multiUserLicenceAssignmentDefaultState();
    state.clearMultiuserLicenceForms = true;
    setTimeout(() => {
      state.clearMultiuserLicenceForms = false;
    }, 1000);
  },
  UPDATE_MULTI_USER_LICENSE_PASSWORD(state, payload) {
    const user = state.multiUserLicence.users
      .find(u => u.userPrincipalName === payload.username);
    user.password = payload.password;
  },
  RESET_USER_LICENCE_STATE(state) {
    state.userLicence = defaultStates.userLicenceAssignmentDefaultState();
  },
  SET_TRANSITION_DATA(state, transitionData) {
    state.transitionData = transitionData;
  },
  SET_TRANSITION_DATA_PRODUCT_ID(state, productId) {
    state.transitionData.productId = productId;
  },
  RESET_TRANSITION_DATA(state) {
    state.transitionData = defaultStates.transitionDataDefaultState();
  },
  UPDATE_TRANSITION_USER_PASSWORD(state, payload) {
    const user = state.transitionData.productItems
      .find(u => u.userPrincipalName === payload.username);
    user.password = payload.password;
  },
  UPDATE_TRANSITION_RECOMENDED_PRODUCT_ITEM(state, payload) {
    const user = state.transitionData.productItems
      .find(u => u.userPrincipalName === payload.username);
    // clear previous state
    user.delete = false;
    user.removeLicenses = false;
    user.keepLicenses = false;
    user.admin = false;
    user.recommendedProductItem.name = payload.product.name;
    user.recommendedProductItem.cloudPlusProductIdentifier = payload.product.identifier;
  },
  SET_TRANSITION_USER_PROPERTY_TO_TRUE(state, payload) {
    const user = state.transitionData.productItems
      .find(u => u.userPrincipalName === payload.username);
    // clear previous state
    user.delete = false;
    user.removeLicenses = false;
    user.keepLicenses = false;
    user.admin = false;

    // set new selected option to true
    user[payload.property] = true;
  },
};
