import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'metrics/';

export default {
  getChartWithGrid(companyId, reportPeriod, appendString) {
    return axios.get(`${Config.apiUrl}${endpoint}${companyId}/${reportPeriod}/${appendString}`);
  },
  getGridByDate(companyId, reportPeriod, date, appendString) {
    return axios.get(`${Config.apiUrl}${endpoint}${companyId}/${reportPeriod}/${date}/${appendString}`);
  },
};

