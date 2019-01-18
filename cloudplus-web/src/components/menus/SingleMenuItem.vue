<template>
  <div v-can-see="items.permissions">
    <li @click="toggleSubmenu" class="single-menu-item">
      <router-link v-if="items.items" :to="items.href" class="single-menu-item__link" :exact="items.exact">
          <i :class="['fa', 'fa-' + items.icon]" ></i>{{items.name}}<i :class="['fa', 'fa-' + submenuIconClass[collapsed], 'single-menu-item__link__chevron']"></i>
      </router-link>

      <brand-hover v-else>
        <router-link :to="items.href" class="single-menu-item__link" :exact="items.exact">
			    <brand-fa :class="['fa', 'fa-' + items.icon]" ></brand-fa>{{items.name}}
		    </router-link>
      </brand-hover>
    </li>
    <submenu v-show="items.items && !collapsed" :items="items.items"></submenu>
  </div>
</template>

<script>
import Submenu from './Submenu';

export default {
  props: ['items'],
  components: { Submenu },
  data() {
    return {
      collapsed: true,
      submenuIconClass: {
        true: 'angle-right',
        false: 'angle-down',
      },
    };
  },
  methods: {
    toggleSubmenu() {
      this.collapsed = !this.collapsed;
    },
  },
};
</script>

<style lang="scss">
.single-menu-item {
  color: #54667a;
  font-size: $font-size;
  &__link {
    display: block;
    padding: 0.625rem 1.25rem;
    font-size: $primary-font-size;
    &__chevron {
      float: right;
      font-size: $font-size;
    }
  }
  .fa {
    padding-right: 0.625rem;
  }
  &__link.router-link-active .fa,
  &__link:hover .fa {
    color: white;
  }
}
</style>
