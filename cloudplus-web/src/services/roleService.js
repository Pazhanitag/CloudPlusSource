import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'roles';
const rpcEndpoint = 'rpc/rolesutilities';

export default {
  getAllRoles() {
    return axios.get(`${Config.apiUrl}${endpoint}`);
  },
  getAllRolesRPC() {
    return axios.get(`${Config.apiUrl}${rpcEndpoint}/getallroles`);
  },
};
