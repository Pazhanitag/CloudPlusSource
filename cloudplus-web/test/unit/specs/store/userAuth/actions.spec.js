// /* eslint-disable import/no-webpack-loader-syntax */
// import actionsInjector from 'inject-loader!@/store/modules/userAuth/actions';
// import chai from 'chai';
// import sinon from 'sinon';
// import sinonChai from 'sinon-chai';
// import * as mutationTypes from '@/store/mutation-types';

// chai.use(sinonChai);

// const assert = chai.assert;

// describe('The User Auth Actions', () => {
//   describe('Authentication flow', () => {
//     let authenticate;
//     let authUserData;
//     beforeEach(() => {
//       authenticate = actionsInjector({
//         '@/services/authService': {
//           authenticateUser() {
//             return new Promise(resolve => {
//               resolve(authUserData);
//             });
//           },
//         },
//       });

//       authUserData = {
//         access_toke: 'testbearertoken123',
//         profile: {
//           sub: 1,
//         },
//       };
//     });
//     it('should call mutation commit when resolved properly', async () => {
//       const commit = sinon.spy();

//       await authenticate.default.authenticate({ commit });

//       assert(commit.should.have.been.called, 'Commit to mutation has not been called');
//     });

//     it('should call mutation commit with auth user data', async () => {
//       const commit = sinon.stub();

//       await authenticate.default.authenticate({ commit });

//       assert(commit.should.have.been.calledWith(mutationTypes.SET_AUTHENTICATION_DATA,
//         authUserData));
//     });
//   });
//   describe('Fetching user data', () => {
//     let loadUserData;
//     let state;
//     let userData;

//     beforeEach(() => {
//       loadUserData = actionsInjector({
//         '@/services/userService': {
//           getUserData() {
//             return new Promise(resolve => {
//               resolve(userData);
//             });
//           },
//         },
//       });

//       userData = {
//         email: 'test@test.test',
//         firstName: 'John',
//       };

//       state = {
//         bearerToken: 'testbearertoken123',
//         userId: 1,
//         user: null,
//       };
//     });

//     it('should call mutation commit when resolved properly', async () => {
//       const commit = sinon.spy();

//       await loadUserData.default.loadUserData({ commit, state });

//       assert(commit.should.have.been.called, 'Commit to mutation has not been called');
//     });
//   });
// });
