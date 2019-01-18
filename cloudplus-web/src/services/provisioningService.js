import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'provisioning';
const cscpEndpoint = 'CustomSecureControlPanel';

export default {
  getProductAvailability(companyId, productId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${companyId}/${productId}`);
  },
  setProductAvailability(companyId, productId, shouldBeAvailable) {
    if (shouldBeAvailable) {
      return axios.post(`${Config.apiUrl}${endpoint}/${companyId}/${productId}`);
    }
    return axios.delete(`${Config.apiUrl}${endpoint}/${companyId}/${productId}`);
  },
  changeProvisioningStatus(companyId, productId, status) {
    return axios.put(`${Config.apiUrl}${endpoint}/${companyId}/${productId}/${status}`);
  },
  getCscpUrlAvailability(companyId) {
    return axios.get(`${Config.apiUrl}${cscpEndpoint}?companyId=${companyId}`);
  },
};
