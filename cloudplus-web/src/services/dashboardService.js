import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'metrics/';

export default {
  getCharts(userId, companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}${userId}/${companyId}/7/Dashboard`);
  },
  getSubscribedServices(companyId, userId) {
    return axios.get(`${Config.apiUrl}${endpoint}${companyId}/${userId}/CustomerWidgets`);
  },
  saveSubscribedServices(payload) {
    const url = `${Config.apiUrl}${endpoint}UpdateCustomerWidgets`;
    return axios.post(url, payload);
  },
  getAllCompanies() {
    return axios.get(`${Config.apiUrl}companies/AllCompanies`);
  },
  getCompanyWidgets(companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}${companyId}/CompanyWidgets`);
  },
  updateCompanyWidgets(payload) {
    return axios.post(`${Config.apiUrl}${endpoint}UpdateCompanyWidgets`, payload);
  },
  getAllWidgets() {
    return axios.get(`${Config.apiUrl}${endpoint}AllWidgets`);
  },
  updateAllWidgets(payload) {
    return axios.post(`${Config.apiUrl}${endpoint}UpdateVendorMetrics`, payload);
  },
  saveReport(payload) {
    return axios.post(`${Config.apiUrl}${endpoint}SaveVendorMetricsReportConfigs`, payload);
  },
  getReports(userId) {
    return axios.get(`${Config.apiUrl}${endpoint}${userId}/GetReportConfig`);
  },
  getReport(id) {
    return axios.get(`${Config.apiUrl}${endpoint}${id}/GetReportConfigBasedOnId`);
  },
  resetAlltoDefault() {
    return axios.post(`${Config.apiUrl}${endpoint}ResetAllCompanies`);
  },
};

