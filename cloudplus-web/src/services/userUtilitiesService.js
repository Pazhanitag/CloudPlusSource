import axios from 'axios';
import Config from 'appConfig';

const endpoint = 'rpc/userutilities';

export default {
  emailAvailable(email) {
    return axios.get(`${Config.apiUrl}${endpoint}/emailAvailable?email=${email}`);
  },
  displayNameAvailable(displayName, userId, companyId) {
    return axios.get(`${Config.apiUrl}${endpoint}/displayNameAvailable?displayName=${displayName}&userId=${userId}&companyId=${companyId}`);
  },
  getUserEmails(email) {
    return new Promise((resolve, reject) => {
      axios.get(`${Config.apiUrl}${endpoint}/getUserEmails?email=${email}`).then(response => {
        resolve(response.data.result);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },
  sendForgotPasswordEmail(id, email, username) {
    const requestData = { id, email, username };
    const url = `${Config.apiUrl}${endpoint}/sendforgotpasswordemail`;
    return axios.post(url, requestData);
  },
  isEmailValied(email) {
    return new Promise((resolve, reject) => {
      axios.get(`${Config.apiUrl}${endpoint}/isemailvalid?email=${email}`).then(response => {
        resolve(response.data.result);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },
  updatePassword(requestData) {
    const url = `${Config.apiUrl}${endpoint}/updatepassword`;
    return axios.put(url, requestData);
  },
  changePassword(requestData) {
    const url = `${Config.apiUrl}${endpoint}/changepassword`;
    return axios.put(url, requestData);
  },
};
