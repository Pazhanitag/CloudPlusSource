import { mount } from 'vue-test-utils';
import { expect } from 'chai';
import CompanyDomains from '@/components/companies/createCompany/forms/CompanyDomains';

const wrapper = mount(CompanyDomains, {
  provide: {
    $validator: {},
  },
  data() {
    return {
      errors: {
        first(name) {
          return name;
        },
      },
    };
  },
  propsData: {
    value: [],
  },
});

describe('CompanyDomains.vue', () => {
  it('should add item to data array when link gets clicked', () => {
    const addLinkButton = wrapper.find('.domain-textfields > div:last-child');
    addLinkButton.trigger('click');

    expect(wrapper.vm.data.length).to.equal(2);
  });

  it('should remove item at passed in key when trash can gets clicked', () => {
    const domainToDelete = 'firstDomain';

    wrapper.setData({ data: [domainToDelete, 'secondDomain', 'thirdDomain'] });

    const trashIcon = wrapper.find('.trash-icon');
    trashIcon.trigger('click');

    expect(wrapper.vm.data.length).to.equal(2);
    expect(wrapper.vm.data[0]).to.not.equal(domainToDelete);
    expect(wrapper.vm.data).to.not.include(domainToDelete);
  });
});
