import Vue from 'vue';
import Vuex from 'vuex';
import VueCroppie from 'vue-croppie';
import VueBus from 'vue-bus';
import userAuth from './modules/userAuth';
import branding from './modules/branding';
import company from './modules/company';
import router from './modules/router';
import user from './modules/user';
import product from './modules/product';
import office365 from './modules/office365';
import catalog from './modules/catalog';
import dashboard from './modules/dashboard';
import chart from './modules/chart';
import cscpURL from './modules/cscpURL';
import support from './modules/support';

Vue.use(Vuex);
Vue.use(VueCroppie);
Vue.use(VueBus);

// initial state, whole reseller portal
const state = {
};
// mutations
const mutations = {
};

// getters
const getters = {
};

export default new Vuex.Store({
  state,
  mutations,
  getters,
  modules: {
    userAuth,
    branding,
    company,
    router,
    user,
    product,
    office365,
    catalog,
    dashboard,
    chart,
    cscpURL,
    support,
  },
});
