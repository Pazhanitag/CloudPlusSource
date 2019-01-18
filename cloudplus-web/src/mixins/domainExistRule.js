import { Validator } from 'vee-validate';
import DomainUtilitiesService from '@/services/domainUtilitiesService';

export default {
  data() {
    return {
      checkingDomainAvailability: [false],
    };
  },
  methods: {
    addDomainExistValidator() {
      Validator.extend('verify_domain_exists', {
        getMessage: field => `The ${field} already exists.`,
        validate: (value, [key]) => new Promise(resolve => {
          this.$set(this.checkingDomainAvailability, key, true);
          DomainUtilitiesService.domainAvailable(value).then(response => {
            this.$set(this.checkingDomainAvailability, key, false);
            if (response.data.result) {
              resolve({
                valid: true,
              });
            }
            resolve({
              valid: false,
            });
          }).catch(() => {
            this.$set(this.checkingDomainAvailability, key, false);
            resolve({
              valid: false,
            });
          });
        }),
      });
    },
  },
  created() {
    this.addDomainExistValidator();
  },
};
