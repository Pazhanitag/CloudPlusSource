import axios from 'axios';
import Config from 'appConfig';

export default {
  verifyRecaptcha(model) {
    return axios.post(`${Config.apiUrl}${Config.API_VerifyRecaptcha}`, model);
  },
};

