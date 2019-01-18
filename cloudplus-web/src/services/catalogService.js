import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'catalogs';
const utilitiesEndpoint = 'rpc/catalogutilities';

export default {
  getResellerCatalogs() {
    return axios.get(`${Config.apiUrl}${endpoint}/reseller`);
  },
  getResellerCatalog(catalogId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${catalogId}/reseller`);
  },
  updateResellerCatalogPrices(catalogId, catalog) {
    return axios.put(`${Config.apiUrl}${endpoint}/${catalogId}/reseller`, catalog);
  },
  getResellerCompaniesAssignedCatalogs() {
    return axios.get(`${Config.apiUrl}${endpoint}/reseller/companies`);
  },
  createNewCatalog(newCatalog) {
    return axios.post(`${Config.apiUrl}${endpoint}/reseller`, newCatalog);
  },
  deleteCatalog(catalogId) {
    return axios.delete(`${Config.apiUrl}${endpoint}/${catalogId}/reseller`);
  },
  downloadCatalog(catalog) {
    return axios.post(`${Config.apiUrl}${endpoint}/SendCatalogEmail`, catalog);
  },
  rawDownload(catalogId, companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/DownloadProductasExcel?catalogId=${catalogId}&companyId=${companyId}`, { responseType: 'arraybuffer', headers: { 'Content-Type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' } });
  },
  getCustomerProducts() {
    return axios.get(`${Config.apiUrl}${endpoint}/customer`);
  },
  changeDefaultCatalog(newDefaultCatalogId) {
    return axios.post(`${Config.apiUrl}${utilitiesEndpoint}/changedefaultresellercatalog/${newDefaultCatalogId}`);
  },
};

