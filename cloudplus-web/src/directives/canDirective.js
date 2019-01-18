import Vue from 'vue';
import store from '@/store/index';

const canDirective = {
  bind(el, binding) {
    const userPermissions = store.getters['userAuth/permissions'] || [];
    if (!userPermissions || !binding.value) return;
    let can = false;
    binding.value.forEach(permission => {
      if (!can) {
        can = userPermissions.find(p => p.name === permission) !== undefined;
      }
    });
    if (!can) {
      el.style.display = 'none';
    }
  },
};

Vue.directive('can-see', canDirective);

export default canDirective;
