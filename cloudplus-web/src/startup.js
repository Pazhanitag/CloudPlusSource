import Vue from 'vue';
import VeeValidate from 'vee-validate';
import Toasted from 'vue-toasted';
import Axios from 'axios';
import AuthService from '@/services/authService';
import installValidationRules from './validation/customRules';

export function ConfigureVeeValidate() {
  const validatorConfig = {
    fieldsBagName: 'fieldsBag', // changed because of conflict with color picker component
    inject: false,
    delay: 400,
  };
  Vue.use(VeeValidate, validatorConfig);
  installValidationRules(VeeValidate.Validator);
}

export function ConfigureVueToasted(router) {
  Vue.use(Toasted, {
    iconPack: 'fontawesome',
    duration: 3000,
    router,
  });
  Vue.toasted.register(
    'global_app_error', payload => {
      if (!payload.message) {
        return 'Oops.. Something Went Wrong..';
      }
      return payload.message;
    },
    {
      type: 'error',
      icon: 'exclamation-triangle',
    },
  );
}

export function ConfigureAxios() {
  Axios.interceptors.response.use(null, error => {
    if (error.response && (error.response.status === 401 || error.response.status === 403)) {
      AuthService.signOut();
    }
    Vue.toasted.global.global_app_error({
      message: error.message,
    });
    return Promise.reject(error);
  });
}
