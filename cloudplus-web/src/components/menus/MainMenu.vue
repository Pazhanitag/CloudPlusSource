<template>
  <div class="side-menu">
    <MenuSection v-for="(menuSectionData, key) in menuSectionsData" :menuSectionData="menuSectionData" :key="key"></MenuSection>
  </div>
</template>

<script>
import menu from '@/assets/constants/menu';
import MenuSection from './MenuSection';

export default {
  props: ['notificationVisible', 'visibility'],
  components: { MenuSection },
  data() {
    return {
      menuSectionsData: menu.menu,
    };
  },
};
</script>

<style lang="scss">
.side-menu {
  width: $menu-width;
  height: 100%;
  overflow: auto;
  background-color: white;
  float: left;
}
</style>
