import { mount } from 'vue-test-utils';
import { expect } from 'chai';
import CloupPlusPasswordTextfield from '@/components/shared/input-components/CloudPlusPasswordTextfield';

const wrapper = mount(CloupPlusPasswordTextfield, {
  propsData: {
    validationErrors: 'Validation error test',
    value: 'password',
  },
});

describe('CloupPlusPasswordTextfield.vue', () => {
  it('should display errors passed from parent', () => {
    expect(wrapper.vm.$el.textContent).to.contain('Validation error test');
  });

  it('should emit input event when password gets typed in', () => {
    let eventFired = false;
    let inputLoad = '';

    wrapper.vm.$on('input', load => {
      eventFired = true;
      inputLoad = load;
    });

    wrapper.vm.updateValue('newPasswordValue');

    expect(eventFired).to.equal(true);
    expect(inputLoad).to.equal('newPasswordValue');
  });

  it('should toggle visibility when eye icon gets clicked', () => {
    const eyeIcon = wrapper.findAll('.icon').at(0);
    eyeIcon.trigger('click');

    expect(wrapper.vm.visibility).to.equal(true);

    const eyeSlashIcon = wrapper.findAll('.icon').at(1);
    eyeSlashIcon.trigger('click');

    expect(wrapper.vm.visibility).to.equal(false);
  });
});
