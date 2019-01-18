import { Validator } from 'vee-validate';
import UserUtilitiesService from '@/services/userUtilitiesService';

export default {
  data() {
    return {
      checkingEmailAvailability: [false],
    };
  },
  methods: {
    addEmailAvailableValidator() {
      if (!this.disableEmail) {
        Validator.extend('verify_email_available', {
          getMessage: field => `The ${field} already exists.`,
          validate: () => new Promise(resolve => {
            this.checkingEmailAvailability = true;
            UserUtilitiesService.emailAvailable(this.user.emailAddress).then(result => {
              this.checkingEmailAvailability = false;
              resolve({
                valid: result.data.result,
              });
            });
          }),
        });
      }
    },
  },
  created() {
    this.addEmailAvailableValidator();
  },
};
