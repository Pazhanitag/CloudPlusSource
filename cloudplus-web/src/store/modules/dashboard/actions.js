import dashboardService from '@/services/dashboardService';

export default {
  getSubscribedServices({ commit }, { companyId, userId }) {
    return new Promise(resolve => {
      dashboardService.getSubscribedServices(companyId, userId).then(response => {
        commit('SET_SUBSCRIBED_SERVICES', response.data.result);
        resolve();
      }).catch(e => {
        console.log(e);
      });
    });
  },
  saveSubscribedServices({ commit }, payload) {
    return new Promise(resolve => {
      dashboardService.saveSubscribedServices(payload).then(response => {
        commit('SET_SUBSCRIBED_SERVICES', response);
        resolve();
      }).catch(e => {
        console.log(e);
      });
    });
  },
  getCharts({ commit }, { userId, companyId }) {
    return new Promise(resolve => {
      dashboardService.getCharts(userId, companyId).then(response => {
        commit('SET_CHARTS', response.data.result);
        resolve();
      }).catch(e => {
        console.log(e);
      });
    });
  },
};

