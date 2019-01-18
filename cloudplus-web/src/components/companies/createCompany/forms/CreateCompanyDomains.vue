<template>
  <div class="domain-textfields">
    <div class="columns" v-for="(field, key) in domains" :key="key">
      <div class="column" :class = "{'is-11': !field.isPrimary, 'domain-textfields__primary': field.isPrimary}">
        <cloud-plus-textfield v-if="field.isPrimary" @input="onInput()" :name="'domain_'+key" v-validate="`required|verify_duplicate_domain|verify_domain_exists:${key}|verify_domain:${key}`" data-vv-delay="500" data-vv-as="Domain" :validationErrors="errors.first('domain_'+key)"  v-model="field.name" :isLoading="checkingDomainAvailability[key]">
          <span >Primary Domain</span>
        </cloud-plus-textfield>
        <cloud-plus-textfield v-else @input="onInput()" :name="'domain_'+key" v-validate="`verify_duplicate_domain|verify_domain_exists:${key}|verify_domain:${key}`" data-vv-delay="500" data-vv-as="Domain" :validationErrors="errors.first('domain_'+key)"  v-model="field.name" :isLoading="checkingDomainAvailability[key]">
          <span>Additional Domain</span>
        </cloud-plus-textfield>
      </div>
      <div v-if="!field.isPrimary" class="column is-1 icon-column">
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
      domains: this.value,
    };
  },
  props: ['value', 'sameAsPrimaryDomain'],
  methods: {
    addAdditionalField() {
      const lastDomainIndex = this.domains.length - 1;
      this.$validator.validate(`domain_${lastDomainIndex}`).then(result => {
        if (result && this.domains.filter(domain => domain.name === '').length === 0) {
          this.domains.push({ name: '', isPrimary: false });
          this.checkingDomainAvailability.push(false);
          this.onInput();
        }
      });
    },
    removeAdditionalField(key) {
      this.domains.splice(key, 1);
      this.checkingDomainAvailability.splice(key, 1);
      this.onInput();
    },
    onInput() {
      if (this.sameAsPrimaryDomain) {
        this.$emit('sameAsPrimary', this.domains);
      }
      this.$emit('setSupportSite', `${this.domains.find(domain => domain.isPrimary).name}`);
      this.$emit('input', this.domains);
    },
    addDuplicateDomainValidator() {
      Validator.extend('verify_duplicate_domain', {
        getMessage: field => `The ${field} has already been used.`,
        validate: value => this.domains.filter(domain => domain.name === value).length <= 1,
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
  &__primary{
    padding-right: 0rem;
  }
}

.additional-domain-link{
  margin-top: -1.25rem;
  font-size: $small-font-size;
}
</style>
