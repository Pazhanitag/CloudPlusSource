import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'products';

export default {
  getProduct(productId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${productId}`);
  },
};

