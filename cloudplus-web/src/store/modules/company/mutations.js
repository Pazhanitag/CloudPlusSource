import Vue from 'vue';
import convert from 'color-convert';
import defaultStates from './initialStates';

function convertColorToColorPickerObject(color) {
  const colorHSL = convert.hex.hsl(color);
  const colorHSV = convert.hex.hsv(color);
  const colorRGB = convert.hex.rgb(color);

  const brandColor = {
    hex: color,
    hsl: {
      h: colorHSL[0],
      s: colorHSL[1],
      l: colorHSL[2],
    },
    hsv: {
      h: colorHSV[0],
      s: colorHSV[1],
      v: colorHSV[2],
    },
    rgba: {
      r: colorRGB[0],
      g: colorRGB[1],
      b: colorRGB[2],
    },
  };

  return brandColor;
}

export default {
  SET_COMPANIES(state, companies) {
    state.companies = companies;
  },
  SET_COMPANY(state, company) {
    state.company = company;

    Vue.set(state.company, 'primaryBrandColor', convertColorToColorPickerObject(company.brandColorPrimary));
    Vue.set(state.company, 'secondaryBrandColor', convertColorToColorPickerObject(company.brandColorSecondary));
    Vue.set(state.company, 'textColor', convertColorToColorPickerObject(company.brandColorText));
  },
  SET_COMPANY_DETAILS_DATA(state, companyDetails) {
    state.company.companyDetails = companyDetails;
  },
  UPDATE_COMPANY_PROPERTY(state, { key, value }) {
    state.company[key] = value;
  },
  SET_COMPANY_DOMAINS(state, domains) {
    state.company.domains = domains;
  },
  RESET_COMPANY_STATE(state) {
    state.company = defaultStates.companyDefaultState();
  },
  SET_PAGED_USERS(state, pagedUsers) {
    state.pagedUsers = pagedUsers;
  },
  SET_COMPANY_USERS(state, companyUsers) {
    state.companyUsers = companyUsers;
  },
  SET_COMPANY_BRANDING(state, company) {
    Vue.set(state.company, 'primaryBrandColor', convertColorToColorPickerObject(company.brandColorPrimary));
    Vue.set(state.company, 'secondaryBrandColor', convertColorToColorPickerObject(company.brandColorSecondary));
    Vue.set(state.company, 'textColor', convertColorToColorPickerObject(company.brandColorText));

    state.company.logoUrl = company.logoUrl;
  },
};

