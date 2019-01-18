// /* eslint-disable no-underscore-dangle */
// import UserList from '@/components/users/UserList';
// import Vue from 'vue';

// describe('UserList.vue', () => {
//   it('should display paging and "Add New User" button', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();
//     expect(component.$el.textContent).to.contain('Showing results');
//     expect(component.$el.textContent).to.contain('Add New User');
//   });

//   it('should have user id set, when selectUser called', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();

//     component.selectUser(1);

//     expect(component.selectedUser).to.equal(1);
//   });

//   it('should have current page number set, when setPage called', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();

//     component.setPage(1);

//     expect(component.currentPage).to.equal(1);
//   });

//   it('should change order of items if being order by the same key', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();

//     component.order = 'asc';
//     component.orderBy = 'email';

//     component.sortBy('email');

//     expect(component.order).to.equal('desc');
//     expect(component.orderBy).to.equal('email');
//   });

//   it('should change order key, but leave the order intact when sorting by different column', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();

//     component.order = 'asc';
//     component.orderBy = 'firstName';

//     component.sortBy('email');

//     expect(component.order).to.equal('asc');
//     expect(component.orderBy).to.equal('email');
//   });

//   it('should set popoverVisible variable to null', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();

//     component.closePopovers();

//     expect(component.popoverVisible).to.equal(null);
//   });

//   it('should set popoverVisible user id passed as parameter', () => {
//     const Constructor = Vue.extend(UserList);
//     const component = new Constructor().$mount();

//     component.popoverVisible = null;
//     component.showPopover(1);

//     expect(component.popoverVisible).to.equal(1);
//   });
// });
