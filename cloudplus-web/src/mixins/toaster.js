import Vue from 'vue';

const generalOptions = {
  action: [{
    icon: 'times',
    onClick: (e, toastObject) => {
      toastObject.goAway(0);
    },
  }],
};
export default {
  methods: {
    sucessToaster(options) {
      Vue.toasted.show(options.text, { ...generalOptions, ...options, ...{ type: 'success' } });
    },
    errorToaster(options) {
      Vue.toasted.show(options.text, { ...generalOptions, ...options, ...{ type: 'error' } });
    },
    getToasterError() {
      return {
        text: 'You cannot proceed until you fill all mandatory fields',
        icon: 'exclamation-triangle',
        duration: 5000,
      };
    },
    validationErrorToaster() {
      this.errorToaster(this.getToasterError());
    },
  },
};
