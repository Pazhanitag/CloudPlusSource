import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'rpc/domainutilities';

export default {
  domainAvailable(name) {
    return axios.get(`${Config.apiUrl}${endpoint}/domainAvailable?name=${name}`);
  },
};
