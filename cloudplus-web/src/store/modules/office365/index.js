import getters from './getters';
import mutations from './mutations';
import actions from './actions';
import defaultStates from './initialStates';

export default {
  namespaced: true,
  state() {
    return {
      customer: defaultStates.customerDefaultState(),
      resendTxtRecordsModel: defaultStates.resendTxtRecordsModelDefaultState(),
      company: defaultStates.companyDefaultState(),
      selectedDomain: '',
      userLicence: defaultStates.userLicenceAssignmentDefaultState(),
      multiUserLicence: defaultStates.multiUserLicenceAssignmentDefaultState(),
      clearMultiuserLicenceForms: false,
      transitionData: defaultStates.transitionDataDefaultState(),
    };
  },
  getters,
  mutations,
  actions,
};
