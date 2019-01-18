import axios from 'axios';
import Config from 'appConfig';

const cscpEndpoint = 'CustomSecureControlPanel';

export default {
  createCscpUrl(createCscpUrlDetails) {
    return axios.put(`${Config.apiUrl}${cscpEndpoint}`, createCscpUrlDetails);
  },
  getSupportProductItems(comapanyId) {
    return axios.get(`${Config.apiUrl}${cscpEndpoint}/GetSupportProduct?companyId=${comapanyId}`);
  },
};
