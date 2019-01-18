import chartService from '@/services/chartService';

export default {
  getChartWithGrid({ commit }, { companyId, reportPeriod, urlString }) {
    return new Promise(resolve => {
      chartService.getChartWithGrid(companyId, reportPeriod, urlString).then(response => {
        commit('SET_CHART', response.data.result.count);
        resolve(response.data.result.details);
      }).catch(e => {
        console.log(e);
      });
    });
  },
  getGridByDate(commit, {
    companyId,
    reportPeriod,
    date,
    urlString,
  }) {
    return new Promise(resolve => {
      chartService.getGridByDate(companyId, reportPeriod, date, urlString).then(response => {
        resolve(response.data.result);
      }).catch(e => {
        console.log(e);
      });
    });
  },
};
