import axios from 'axios';
import Oidc from 'oidc-client';
import Config from 'appConfig';

let mgr = new Oidc.UserManager({
  userStore: new Oidc.WebStorageStateStore(),
  authority: Config.authConfig.authority,
  client_id: Config.authConfig.clientId,
  redirect_uri: Config.authConfig.redurectUri,
  silent_redirect_uri: Config.authConfig.silent_redirect_uri,
  accessTokenExpiringNotificationTime: Config.authConfig.expiringNotificationTime,
  automaticSilentRenew: Config.authConfig.automaticSilentRenew,
  response_type: Config.authConfig.responseType,
  scope: Config.authConfig.scope,
  post_logout_redirect_uri: window.location.origin,
  loadUserInfo: true,
});

export default {
  signIn() {
    mgr.signinRedirect();
  },

  signOut() {
    mgr.signoutRedirect({ post_logout_redirect_uri: window.location.origin });
  },

  authenticateUser() {
    return new Promise((resolve, reject) => {
      mgr.events.addSilentRenewError(() => {
        this.signOut();
      });
      mgr.events.addUserLoaded(user => {
        axios.defaults.headers.common.Authorization = `Bearer ${user.access_token}`;
      });
      mgr.getUser().then(user => {
        if (user == null || user.expired) {
          this.signIn();
        } else {
          resolve(user);
        }
      }).catch(err => {
        reject(err);
      });
    });
  },
  impersonate(userId) {
    return new Promise((resolve, reject) => {
      axios.get(`${Config.apiUrl}impersonate/${userId}`).then(response => {
        mgr = new Oidc.UserManager({
          authority: Config.authConfig.authority,
          client_id: Config.authConfig.clientId,
          redirect_uri: Config.authConfig.redurectUri,
          response_type: Config.authConfig.responseType,
          scope: Config.authConfig.scope,
          post_logout_redirect_uri: Config.authConfig.postLogoutRedirectUrl,
          loadUserInfo: true,
          silent_redirect_uri: Config.authConfig.silent_redirect_uri,
          accessTokenExpiringNotificationTime: Config.authConfig.expiringNotificationTime,
          automaticSilentRenew: Config.authConfig.automaticSilentRenew,
          acr_values: `impersonate:${userId}`,
          prompt: 'login',
        });

        mgr.signinRedirect();
        resolve(response.data.result);
      }).catch(err => {
        reject(err.errorMessage);
      });
    });
  },

  revertImpersonate() {
    mgr = new Oidc.UserManager({
      authority: Config.authConfig.authority,
      client_id: Config.authConfig.clientId,
      redirect_uri: Config.authConfig.redurectUri,
      response_type: Config.authConfig.responseType,
      scope: Config.authConfig.scope,
      post_logout_redirect_uri: Config.authConfig.postLogoutRedirectUrl,
      loadUserInfo: true,
      silent_redirect_uri: Config.authConfig.silent_redirect_uri,
      accessTokenExpiringNotificationTime: Config.authConfig.expiringNotificationTime,
      automaticSilentRenew: Config.authConfig.automaticSilentRenew,
      prompt: 'login',
    });

    mgr.signinRedirect();
  },

  loadUserInfo() {
    return axios.get(`${Config.authConfig.authority}connect/userInfo`);
  },
};

