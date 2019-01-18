/* eslint quotes: ["error", "single", { "allowTemplateLiterals": true }] */
import Vue from 'vue';

// brand-buttons
import BrandButton from './brand-buttons/BrandButton';
import BrandUploadFileButton from './brand-buttons/BrandUploadFileButton';

// brand-wrappers
import BrandBackgroundWithTextColor from './brand-wrappers/BrandBackgroundWithTextColor';
import BrandHover from './brand-wrappers/BrandHover';
import BrandTextColor from './brand-wrappers/BrandTextColor';
import BrandSectionTitle from './brand-wrappers/BrandSectionTitle';
import BrandInputIcon from './brand-wrappers/BrandInputIcon';

// brand-messages
import BrandSuccessMessage from './brand-messages/BrandSuccessMessage';
import BrandErrorMessage from './brand-messages/BrandErrorMessage';

// brand-input
import BrandInput from './brand-input/BrandInput';
import BrandSelect from './brand-input/BrandSelect';

// brand-step-progressbar
import BrandStepProgressbar from './brand-step-progressbar/BrandStepProgressbar';
import BrandStep from './brand-step-progressbar/BrandStep';

// misc
import BrandSpinner from './brand-misc/BrandSpinner';
import BrandTabs from './brand-misc/BrandTabs';
import BrandSwitch from './brand-input/BrandSwitch';
import BrandFontAwesomeIcon from './brand-icon/BrandFontAwesomeIcon';

// define click event mixin
const clickMixin = {
  methods: {
    click() {
      this.$emit('click');
    },
  },
};

// brand-buttons
export const brandPrimaryButton = Vue.component('brand-primary-btn', {
  components: { BrandButton },
  mixins: [clickMixin],
  template: `<BrandButton @click.native="click" class="button" :color="$store.getters['branding/brand'].primaryColor" :textColor="$store.getters['branding/brand'].textColor"><slot></slot></BrandButton>`,
});

export const brandSecondaryButton = Vue.component('brand-secondary-btn', {
  components: { BrandButton },
  mixins: [clickMixin],
  template: `<BrandButton @click.native="click" class="button" :color="$store.getters['branding/brand'].secondaryColor" :textColor="$store.getters['branding/brand'].textColor"><slot></slot></BrandButton>`,
});

export const brandReversePrimaryButton = Vue.component('brand-reverse-primary-btn', {
  components: { BrandButton },
  mixins: [clickMixin],
  template: `<BrandButton @click.native="click" class="button" style="border: 1px solid #dbdbdb" color="white" :textColor="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandButton>`,
});

export const brandUploadFileButton = Vue.component('brand-upload-file-btn', {
  components: { BrandUploadFileButton },
  template: `<BrandUploadFileButton class="file-cta" :color="$store.getters['branding/brand'].primaryColor" :textColor="$store.getters['branding/brand'].textColor"><span style="margin:auto"><slot></slot></span></BrandUploadFileButton>`,
});

// brand-wrappers
export const brandBackgroundWithTextColor = Vue.component('brand-background-with-text-color', {
  components: { BrandBackgroundWithTextColor },
  template: `<BrandBackgroundWithTextColor :backgroundColor="$store.getters['branding/brand'].primaryColor" :textColor="$store.getters['branding/brand'].textColor"><slot></slot></BrandBackgroundWithTextColor>`,
});

export const brandHover = Vue.component('brand-hover', {
  components: { BrandHover },
  template: `<BrandHover :backgroundColor="$store.getters['branding/brand'].primaryColor" :textColor="$store.getters['branding/brand'].textColor"><slot></slot></BrandHover>`,
});

export const brandPrimaryColor = Vue.component('brand-color-primary', {
  components: { BrandTextColor },
  template: `<BrandTextColor :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandTextColor>`,
});

export const brandSectionTitle = Vue.component('brand-section-title', {
  components: { BrandSectionTitle },
  template: `<BrandSectionTitle :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandSectionTitle>`,
});

export const brandInputIcon = Vue.component('brand-input-icon', {
  components: { BrandInputIcon },
  template: `<BrandInputIcon class="btn-radio" :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandInputIcon>`,
});

// brand-messages
export const brandSuccessMessage = Vue.component('brand-success-message', {
  components: { BrandSuccessMessage },
  template: '<BrandSuccessMessage><i class="fa fa-check-circle" aria-hidden="true"></i><br><slot></slot></BrandSuccessMessage>',
});

export const brandErrorMessage = Vue.component('brand-error-message', {
  components: { BrandErrorMessage },
  template: '<BrandErrorMessage><i class="fa fa-exclamation-circle" aria-hidden="true"></i><br><slot></slot></BrandErrorMessage>',
});

// misc
export const brandSpinner = Vue.component('brand-spinner', {
  components: { BrandSpinner },
  template: `<BrandSpinner class="btn-radio" :color="$store.getters['branding/brand'].primaryColor"><i class="fa fa-spinner fa-spin"></i></BrandSpinner>`,
});

export const brandTabs = Vue.component('brand-tabs', {
  components: { BrandTabs },
  template: `<BrandTabs :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandTabs>`,

});

export const brandSwitch = Vue.component('brand-switch', {
  components: { BrandSwitch },
  template: `<BrandSwitch :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandSwitch>`,
});

export const brandFontAwesomeIcon = Vue.component('brand-fa', {
  components: { BrandFontAwesomeIcon },
  template: `<BrandFontAwesomeIcon :color="$store.getters['branding/brand'].primaryColor"></BrandFontAwesomeIcon>`,
});


// brand-step-progressbar
export const brandStepProgressbar = Vue.component('brand-step-progressbar', {
  components: { BrandStepProgressbar },
  props: ['numOfSteps'],
  template: `<BrandStepProgressbar :numOfSteps = "numOfSteps"><slot></slot></BrandStepProgressbar>`,
});

export const brandStep = Vue.component('brand-step', {
  components: { BrandStep },
  props: ['numOfSteps'],
  template: `<BrandStep :numOfSteps = "numOfSteps" :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandStep>`,
});

// brand-input
export const brandInput = Vue.component('brand-input', {
  components: { BrandInput },
  template: `<BrandInput :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandInput>`,
});
export const brandSelect = Vue.component('brand-select', {
  components: { BrandSelect },
  template: `<BrandSelect :color="$store.getters['branding/brand'].primaryColor"><slot></slot></BrandSelect>`,
});

