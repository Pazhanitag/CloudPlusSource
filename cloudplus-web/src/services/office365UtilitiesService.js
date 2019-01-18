import axios from 'axios';
import Config from 'appConfig';

const rpcEndpoint = 'rpc/office365utilities';

export default {
  validateCustomerAddress(address) {
    const url = `${Config.apiUrl}${rpcEndpoint}/validatecustomeraddress`;
    return axios.post(url, address);
  },
  getProvisioningStatus(companyId) {
    return axios.get(`${Config.apiUrl}${rpcEndpoint}/getprovisioningstatus/${companyId}`);
  },
  resendTxtRecords(model) {
    const url = `${Config.apiUrl}${rpcEndpoint}/resendtxtrecords`;
    return axios.post(url, model);
  },
  verifyDomain(domain) {
    const url = `${Config.apiUrl}${rpcEndpoint}/verifycustomerdomain?domainName=${domain}`;
    return axios.get(url);
  },
  federateDomain(domain) {
    const url = `${Config.apiUrl}${rpcEndpoint}/federatecustomerdomain?domainName=${domain}`;
    return axios.post(url);
  },
};
