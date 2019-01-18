import { mount } from 'vue-test-utils';
import { expect } from 'chai';
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';

const wrapper = mount(CloudPlusCardModal, {
  propsData: {
    showModal: true,
  },
});

describe('CloudPlusCardModal.vue', () => {
  it('should show modal', () => {
    const p = wrapper.findAll('div').at(0);
    expect(p.hasClass('is-active')).to.equals(true);
  });
  it('simulate close modal click', () => {
    const addLinkButton = wrapper.find('.cancel');
    expect(wrapper.vm.showModalLocal).to.equal(true);
    addLinkButton.trigger('click');
    expect(wrapper.vm.showModalLocal).to.equal(false);
  });
});
