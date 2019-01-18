import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'users';
const rpcEndpoint = 'rpc/userutilities';
const profileEndpoint = 'profile';

export default {
  createUser(user) {
    return axios.post(`${Config.apiUrl}${endpoint}`, user);
  },
  updateUser(user) {
    return axios.put(`${Config.apiUrl}${endpoint}`, user);
  },
  getUsers(page, pageSize, orderBy, order, searchTerm) {
    return axios.get(`${Config.apiUrl}${endpoint}/${page}/${pageSize}/${orderBy}/${order}/${searchTerm}`);
  },
  getUser(userId) {
    return new Promise((resolve, reject) => {
      axios.get(`${Config.apiUrl}${endpoint}/${userId}`).then(response => {
        resolve(response.data.result);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },
  getUserProfile() {
    return axios.get(`${Config.apiUrl}${profileEndpoint}`);
  },
  updateUserProfile(user) {
    return axios.put(`${Config.apiUrl}${profileEndpoint}`, user);
  },
  getUserPermissions(userId) {
    return new Promise((resolve, reject) => {
      axios.get(`${Config.apiUrl}${endpoint}/${userId}/permissions`).then(response => {
        resolve(response.data.result);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },
  getParentProfileData() {
    return axios.get(`${Config.apiUrl}${rpcEndpoint}/getparentprofiledata`);
  },
  getUserServices(username, companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/${username}/services?companyId=${companyId}`);
  },
  deleteUser(userId) {
    return axios.delete(`${Config.apiUrl}${endpoint}/${userId}`);
  },
};

