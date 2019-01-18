<template>
  <div class="column">
    <div class="content">
      <cloud-plus-browse-image @imageUploaded="logoUploaded" :imgSource="company.logoUrl" title="Set company logo"></cloud-plus-browse-image>
    </div>
    <div class="columns">
      <cloud-plus-color-picker @input="onInput('primaryBrandColor', $event)" :value="company.primaryBrandColor">
        <span slot="label">Choose primary brand color</span>
        <span slot="description">Determines header/subhead and primary button/highlight colors.</span>
      </cloud-plus-color-picker>
    </div>
    <div class="columns">
      <cloud-plus-color-picker @input="onInput('secondaryBrandColor', $event)" :value="company.secondaryBrandColor">
        <span slot="label">Choose secondary brand color</span>
        <span slot="description">Determines accent colors.</span>
      </cloud-plus-color-picker>
    </div>
    <div class="columns">
      <cloud-plus-color-picker @input="onInput('textColor', $event)" :value="company.textColor">
        <span slot="label">Choose text color</span>
        <span slot="description">Determines color for text on secondary button.</span>
      </cloud-plus-color-picker>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import CloudPlusColorPicker from '@/components/shared/input-components/CloudPlusColorPicker';
import CloudPlusBrowseImage from '@/components/shared/input-components/CloudPlusBrowseImage';

export default {
  components: {
    CloudPlusColorPicker,
    CloudPlusBrowseImage,
  },
  computed: {
    ...mapGetters({
      company: 'company/brandingInformation',
    }),
  },
  methods: {
    ...mapMutations({
      updateCompanyProperty: 'company/UPDATE_COMPANY_PROPERTY',
    }),
    onInput(key, value) {
      this.updateCompanyProperty({
        key,
        value,
      });
    },
    logoUploaded(base64, logoSrc) {
      this.updateCompanyProperty({
        key: 'logoBase64',
        value: base64,
      });
      this.updateCompanyProperty({
        key: 'logoUrl',
        value: logoSrc,
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.content {
  margin-bottom: 5rem;
}
</style>
