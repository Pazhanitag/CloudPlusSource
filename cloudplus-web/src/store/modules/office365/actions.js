import office365UtilitiesService from '@/services/office365UtilitiesService';
import office365CustomersService from '@/services/office365CustomersService';
import companyService from '@/services/companyService';
import provisioningService from '@/services/provisioningService';

export default {
  validateCustomerAddress({ getters }) {
    return office365UtilitiesService.validateCustomerAddress(getters.customerAddress);
  },
  createCustomer({ getters }) {
    return office365CustomersService.createCustomer(getters.createCustomerCompany);
  },
  getCompany({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getCompany(companyId).then(result => {
        commit('SET_CUSTOMER', result.data.result);
        resolve(result);
      });
    });
  },
  resendTxtRecords({ getters }) {
    return office365UtilitiesService.resendTxtRecords(getters.resendTxtRecordsModel);
  },
  getCompanyOffice365Domains({ commit }, companyId) {
    return new Promise(resolve => {
      companyService.getCompanyOffice365Domains(companyId).then(result => {
        commit('SET_COMPANY', result.data.result);
        resolve(result);
      });
    });
  },
  addAdditionalDomain({ getters }) {
    return office365CustomersService.addAdditionalDomain(getters.additionalDomain);
  },
  getDomainUsers({ commit }, params) {
    return new Promise(resolve => {
      office365CustomersService.getCompanyUsersByDomain(
        params.domain,
        params.pageNumber,
        params.pageSize,
        params.orderBy,
        params.order,
        params.searchTerm,
      ).then(result => {
        const pagedUsers = result.data.result;
        commit('SET_DOMAIN_USERS', {
          users: pagedUsers,
          index: params.index,
        });
        resolve(result);
      });
    });
  },
  assignOffice365ToUser({ getters }) {
    return office365CustomersService.assignLicence(getters.userLicenceAssignment);
  },
  restoreUserService({ getters }) {
    return office365CustomersService.restoreLicence(getters.userLicenceRestore);
  },
  removeUserService({ getters }) {
    return office365CustomersService.removeLicence(getters.userLicenceRemove);
  },
  editMultiUser({ getters }) {
    return office365CustomersService.multiEdit(getters.multiUserLicenceEdit);
  },
  getAssignedLicences({ getters }) {
    return office365CustomersService
      .getAssignedLicences(getters.userLicenceAssignment.userPrincipalName);
  },
  getAssignedRoles({ getters }) {
    return office365CustomersService
      .getAssignedRoles(
        getters.userLicenceAssignment.userPrincipalName,
        getters.userLicenceAssignment.companyId,
      );
  },
  changeLicence({ getters }) {
    return office365CustomersService.changeLicence(getters.changeUserLicence);
  },
  getTransitionData({ commit, rootGetters }) {
    return new Promise(resolve => {
      office365CustomersService.getTransitionData().then(result => {
        const transitionData = result.data.result;
        commit('SET_TRANSITION_DATA', transitionData);
        commit('SET_TRANSITION_DATA_PRODUCT_ID', rootGetters['product/selectedProduct'].id);
        resolve(transitionData);
      });
    });
  },
  startTransition({ getters }) {
    return office365CustomersService.startTransition(getters.transitionData);
  },
  setProvisioningStatusToInTransition({ rootGetters }) {
    return provisioningService.changeProvisioningStatus(rootGetters['userAuth/companyId'], rootGetters['product/selectedProduct'].id, 2);
  },
  setProvisioningStatusToActive({ rootGetters }) {
    return provisioningService.changeProvisioningStatus(rootGetters['userAuth/companyId'], rootGetters['product/selectedProduct'].id, 1);
  },
  federateDomain(context, domain) {
    return office365UtilitiesService.federateDomain(domain);
  },
};
