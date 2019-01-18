// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue';
import 'babel-polyfill';
import App from './App';
import { ConfigureVeeValidate, ConfigureVueToasted, ConfigureAxios } from './startup';
import router from './router/index';
import store from './store/index';
import './components/shared/styled-components';
import './components/shared';
import './directives';
import './filters/formatPrice';
import '../node_modules/ag-grid/dist/styles/ag-grid.css';
import '../node_modules/ag-grid/dist/styles/ag-theme-balham.css';

import { userHasPermissions } from './helpers/utils';

Vue.config.productionTip = false;

ConfigureVeeValidate();
ConfigureVueToasted(router);
ConfigureAxios();

const app = new Vue({
  store,
  router,
  template: '<App/>',
  components: { App },
});

if (app.$route.meta.requiresAuth) {
  store.dispatch('userAuth/authenticate').then(() => {
    const loadPermissions = store.dispatch(
      'userAuth/loadPermissions',
      store.getters['userAuth/userProfile'].id,
    );
    const loadBranding = store.dispatch('branding/getBranding');
    Promise.all([loadPermissions, loadBranding]).then(() => {
      store.dispatch('company/getCompany', store.getters['userAuth/userProfile'].companyId).then(response => {
        if (response.data.result.name !== undefined) {
          document.title = response.data.result.name;
        }
      });
      if (!userHasPermissions(store.getters['userAuth/permissions'], router.currentRoute.meta.permission)) {
        router.push('/companies/details');
      }
      app.$mount('#app');
      if (store.getters['userAuth/userProfile'].parentUserId) {
        store.dispatch('userAuth/getParentProfileData').then(() => { });
      }
    });
  });
} else {
  app.$mount('#app');
}

router.beforeEach((to, from, next) => {
  if (userHasPermissions(store.getters['userAuth/permissions'], to.meta.permission)) {
    next();
  } else {
    next('/companies/details');
  }
});
