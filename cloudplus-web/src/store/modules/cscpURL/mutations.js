import { usStates } from '@/assets/constants/states';

export default {
  SET_COMPANY(state, company) {
    state.company.domains = company.domains;
    state.company.domain = company.domains[0].name;
    state.company.name = company.name;
    state.company.phoneNumber = company.phoneNumber;
    state.company.address1 = company.streetAddress;
    state.company.city = company.city;
    state.company.country = company.country;
    state.company.state = company.state;
    state.company.postalCode = company.zipCode;
    state.company.email = company.email;

    const companyState = usStates.find(s => s.name.toLowerCase() === company.state.toLowerCase() ||
        s.abbreviation.toLowerCase() === company.state.toLowerCase());

    if (companyState) {
      state.company.stateCode = companyState.abbreviation;
    }
  },
  SET_RESELLER(state, reseller) {
    state.reseller = reseller;
  },
  SET_CSCPURL(state, cscpurl) {
    state.cscpUrl = cscpurl;
  },
  UPDATE_RESELLER(state, { key, value }) {
    state.reseller[key] = value;
  },
  UPDATE_COMPANY(state, { key, value }) {
    state.company[key] = value;
  },
  RESET_CSCP_DETAILS_FOR_CONFIGURATION(state) {
    state.cscpDetailsForConfiguration = {};
  },
};
