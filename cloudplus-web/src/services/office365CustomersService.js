import axios from 'axios';
import Config from 'appConfig';

const customersEndpoint = 'office365customers';
const customersDomainEndpoint = 'Office365Domains';
const customersUserEndpoint = 'Office365Users';
const customersRoleEndpoint = 'Office365Roles';
const customerTransitionsEndpoint = 'Office365Transitions';

export default {
  createCustomer(customer) {
    const url = `${Config.apiUrl}${customersEndpoint}`;
    return axios.post(url, customer);
  },
  addAdditionalDomain(domain) {
    const url = `${Config.apiUrl}${customersDomainEndpoint}`;
    return axios.post(url, domain);
  },
  assignLicence(userLicence) {
    const url = `${Config.apiUrl}${customersUserEndpoint}/AssignLicense`;
    return axios.post(url, userLicence);
  },
  getRoles() {
    return axios.get(`${Config.apiUrl}${customersRoleEndpoint}/GetAllRoles`);
  },
  restoreLicence(userLicence) {
    const url = `${Config.apiUrl}${customersUserEndpoint}/Restore`;
    return axios.post(url, userLicence);
  },
  removeLicence(userLicence) {
    const url = `${Config.apiUrl}${customersUserEndpoint}/RemoveLicense`;
    return axios.post(url, userLicence);
  },
  multiEdit(multiUserLicenceEdit) {
    const url = `${Config.apiUrl}${customersUserEndpoint}/MultiEdit`;
    return axios.post(url, multiUserLicenceEdit);
  },
  getAssignedLicences(username) {
    return axios.get(`${Config.apiUrl}${customersUserEndpoint}/${username}/AssignedLicenses`);
  },
  getAssignedRoles(username, companyId) {
    return axios.get(`${Config.apiUrl}${customersUserEndpoint}/${username}/AssignedRoles?companyId=${companyId}`);
  },
  changeLicence(changeUserLicenceModel) {
    const url = `${Config.apiUrl}${customersUserEndpoint}/ChangeLicense`;
    return axios.post(url, changeUserLicenceModel);
  },
  getTransitionData() {
    return axios.get(`${Config.apiUrl}${customerTransitionsEndpoint}/TransitionMatchingData`);
  },
  startTransition(transitionProcessModel) {
    const url = `${Config.apiUrl}${customerTransitionsEndpoint}/Transition`;
    return axios.post(url, transitionProcessModel);
  },
  checkAuthorization() {
    return axios.get(`${Config.apiUrl}${customerTransitionsEndpoint}/CheckAuthorization`);
  },
  isDomainFederated(domain) {
    return axios.get(`${Config.apiUrl}${customersDomainEndpoint}/IsDomainFederated?domain=${domain}`);
  },
  getCompanyUsersByDomain(domain, page, pageSize, orderBy, order, searchTerm) {
    return axios.get(`${Config.apiUrl}${customersUserEndpoint}/GetDomainUsers/${domain}/${page}/${pageSize}/${orderBy}/${order}/${searchTerm}`);
  },
};
