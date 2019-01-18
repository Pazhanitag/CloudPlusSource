import { mount } from 'vue-test-utils';
import { expect } from 'chai';
import CloudPlusPagination from '@/components/shared/pagination/CloudPlusPagination';

const wrapper = mount(CloudPlusPagination, {
  propsData: {
    usersLength: 5,
    usersPerPage: 5,
    currentPage: 1,
    pageCount: 7,
    userCount: 31,
  },
});

describe('CloudPlusPagination.vue', () => {
  it('should show disabled next or back button depending on whether the user is on the first or the last page', () => {
    expect(wrapper.vm.ifPreviousButtonShouldBeDisabled).to.equal(true);
    expect(wrapper.vm.ifNextButtonShouldBeDisabled).to.equal(false);
    wrapper.setProps({ currentPage: 7 });
    expect(wrapper.vm.ifPreviousButtonShouldBeDisabled).to.equal(false);
    expect(wrapper.vm.ifNextButtonShouldBeDisabled).to.equal(true);
  });
  it('should show ellipsis or not depending on current page position', () => {
    wrapper.setProps({ currentPage: 7 });
    const pageNumberFirst = 1;
    expect(wrapper.vm.shouldShowEllipsis(pageNumberFirst, true)).to.equal(false);
    expect(wrapper.vm.shouldShowEllipsis(pageNumberFirst + 1, true)).to.equal(true);
    wrapper.setProps({ currentPage: 1 });
    const pageNumberLast = 5;
    expect(wrapper.vm.shouldShowEllipsis(pageNumberLast, false)).to.equal(false);
    expect(wrapper.vm.shouldShowEllipsis(pageNumberLast + 1, false)).to.equal(true);
  });
  it('should show page number or not depending on current page position', () => {
    wrapper.setProps({ currentPage: 7 });
    const pageNumberFirst = 1;
    expect(wrapper.vm.shouldShowPageNumber(pageNumberFirst)).to.equal(true);
    const pageNumberMiddle = 3;
    expect(wrapper.vm.shouldShowPageNumber(pageNumberMiddle)).to.equal(false);
  });
  it('should show correct number of users', () => {
    wrapper.setProps({ usersPerPage: 50 });
    const p = wrapper.findAll('span').at(3);
    expect(p.html()).to.equals('<span>31</span>');
  });
});
