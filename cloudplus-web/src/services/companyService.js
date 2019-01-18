import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'companies';
const rpcEndpoint = 'rpc/companyutilities';

export default {
  createCompany(company) {
    const url = `${Config.apiUrl}${endpoint}`;
    return new Promise((resolve, reject) => {
      axios.post(url, company).then(response => {
        resolve(response);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },
  createCompanyFromExternalSignupForm(company) {
    const url = `${Config.apiUrl}${rpcEndpoint}/createcompany`;
    return new Promise((resolve, reject) => {
      axios.post(url, company).then(response => {
        resolve(response);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },
  getCompanies(companyId, accountType) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/${endpoint}/${accountType}`);
  },
  getCompaniesBySearch(companyId, accountType, searchTerm) {
    return axios.get(`${Config.apiUrl}${endpoint}/FilterCompaniesByType?companyId=${companyId}&companyType=${accountType}&search=${searchTerm}`);
  },
  getCompanyChildrenForPriceCatalog(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/${endpoint}/pricecatalog`);
  },
  getCompany(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}`);
  },
  getUsersByCompanyId(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/users`);
  },
  getCompanyDomains(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/domains`);
  },
  getCompanyBranding() {
    return axios.get(`${Config.apiUrl}${endpoint}/branding`);
  },
  getCompanyBrandingForUser(userEmail) {
    return axios.get(`${Config.apiUrl}${rpcEndpoint}/getbrandingforusercompany?email=${userEmail}`);
  },
  updateCompany(company) {
    return axios.put(`${Config.apiUrl}${endpoint}`, company);
  },
  getCompanyUsers(companyId, page, pageSize, orderBy, order, searchTerm) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/users/${page}/${pageSize}/${orderBy}/${order}/${searchTerm}`);
  },
  getParentCompanyByCompanyId(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/parentcompany`);
  },
  getCompanyOffice365Domains(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/office365domains`);
  },
};

