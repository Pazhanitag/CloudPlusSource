// import chai from 'chai';
// import mutations from '@/store/modules/userAuth/mutations';
// import * as mutationTypes from '@/store/mutation-types';

// const assert = chai.assert;

// describe('The User Auth Mutations', () => {
//   let localStore;

//   beforeEach(() => {
//     localStore = {
//       bearerToken: '',
//       userId: -1,
//     };
//   });

//   it('should set bearer token when access_token value is present', () => {
//     const authData = {
//       access_token: 'testbearertoken123',
//     };
//     mutations[mutationTypes.SET_AUTHENTICATION_DATA](localStore, authData);
//     assert(localStore.bearerToken === authData.access_token, 'Bearer token not set properly');
//   });

//   it('should set userId when subject is presented in profile', () => {
//     const authData = {
//       profile: {
//         sub: 1,
//       },
//     };
//     mutations[mutationTypes.SET_AUTHENTICATION_DATA](localStore, authData);
//     assert(localStore.userId === authData.profile.sub, 'UserId not set properly');
//   });

//   it('should set userId and bearer token when access_token value is present and subject is presented in profile', () => {
//     const authData = {
//       profile: {
//         sub: 1,
//       },
//       access_token: 'testbearertoken123',
//     };
//     mutations[mutationTypes.SET_AUTHENTICATION_DATA](localStore, authData);
//     assert(localStore.userId === authData.profile.sub && localStore.bearerToken === authData.access_token, 'UserId or bearer token not set properly');
//   });

//   it('should set bearer token but not user id when profile is null', () => {
//     const authData = {
//       profile: null,
//       access_token: 'testbearertoken123',
//     };
//     mutations[mutationTypes.SET_AUTHENTICATION_DATA](localStore, authData);
//     assert(localStore.userId === -1 && localStore.bearerToken === authData.access_token);
//   });

//   it('should set user data when passed in mutation', () => {
//     const userData = {
//       userData: {
//         email: 'test@test.test',
//       },
//     };
//     mutations[mutationTypes.SET_USER_AUTHORIZATION_DATA](localStore, userData);
//     assert(localStore.user.email === userData.email, 'User data not set');
//   });
// });
