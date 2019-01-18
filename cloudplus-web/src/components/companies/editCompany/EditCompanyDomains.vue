<template>
  <div class="domain-textfields">
    <div v-for="(domain, key) in existingDomains" :key="key" class="columns">
      <div class="column domain-textfields__existing">
        <cloud-plus-textfield disabled v-model="domain.name">
          <span v-if="domain.isPrimary">Primary Domain</span>
          <span v-else>Additional Domain</span>
        </cloud-plus-textfield>
      </div>
    </div>
    <div class="columns" v-for="(domain, key) in newDomains" :key="key">
      <div class="column is-11">
        <cloud-plus-textfield @input="onInput()" v-model="domain.name" :name="'domain_'+key" v-validate="`url|verify_duplicate_domain|verify_domain_exists:${key}`" data-vv-delay="500" data-vv-as="Domain" :validationErrors="errors.first('domain_'+key)" :isLoading="checkingDomainAvailability[key]">
          <span>Additional Domain</span>
        </cloud-plus-textfield>
      </div>
      <div class="column is-1 icon-column">
        <brand-color-primary><i @click="removeAdditionalField(key)" class="fa fa-trash-o trash-icon"></i></brand-color-primary>
      </div>
    </div>
    <div @click="addAdditionalField()">
      <brand-color-primary class="additional-domain-link"><a>Add additional domains</a></brand-color-primary>
    </div>
  </div>
</template>

<script>
import { Validator } from 'vee-validate';
import domainExistRule from '@/mixins/domainExistRule';

export default {
  inject: {
    $validator: '$validator',
  },
  mixins: [domainExistRule],
  data() {
    return {
      newDomains: [],
    };
  },
  props: ['existingDomains', 'value'],
  methods: {
    addAdditionalField() {
      const lastDomainIndex = this.newDomains.length - 1;
      if (lastDomainIndex !== -1) {
        this.$validator.validate(`domain_${lastDomainIndex}`).then(result => {
          if (result && this.newDomains.filter(domain => domain.name === '').length === 0) {
            this.newDomains.push({ name: '', isPrimary: false });
            this.checkingDomainAvailability.push(false);
            this.onInput();
          }
        });
      } else {
        this.newDomains.push({ name: '', isPrimary: false });
        this.checkingDomainAvailability.push(false);
        this.onInput();
      }
    },
    removeAdditionalField(key) {
      this.newDomains.splice(key, 1);
      this.checkingDomainAvailability.splice(key, 1);
      this.onInput();
    },
    onInput() {
      this.$emit('input', this.newDomains);
    },
    addDuplicateDomainValidator() {
      Validator.extend('verify_duplicate_domain', {
        getMessage: field => `The ${field} has already been used.`,
        validate: value => this.newDomains.filter(domain => domain.name === value).length <= 1,
      });
    },
  },
  created() {
    this.addDuplicateDomainValidator();
  },
};
</script>

<style lang="scss" scoped>
.columns {
  margin-right: 0rem;
}

.icon-column {
  margin-top: 2.3125rem;
  height: $input-fields-height;
}

.trash-icon {
  font-size: 1.2rem;
  margin: -15% 0 0 -25%;
  &:hover {
    cursor: pointer;
  }
}

.domain-textfields{
  padding-bottom: 1.875rem;
  &__existing{
    padding-right: 0rem;
  }
}

.additional-domain-link{
  margin-top: -1.25rem;
  font-size: $small-font-size;
}
</style>
