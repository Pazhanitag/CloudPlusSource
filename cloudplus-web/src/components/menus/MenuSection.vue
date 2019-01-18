<template>
  <div class="menu-section" v-can-see="menuSectionData.permissions">
    <p class="menu-section__label">{{menuSectionData.section}}</p>
    <ul class="menu-section__list">
      <single-menu-item v-for="(items, key) in menuSectionData.items" :items="items" :key="key"></single-menu-item>
    </ul>
  </div>
</template>

<script>
  import SingleMenuItem from './SingleMenuItem';

  export default {
    props: ['menuSectionData'],
    components: { SingleMenuItem },
  };
</script>

<style lang="scss">
  .menu-section {
    &__label {
      margin: 2.5rem 1.25rem 0.625rem;
      text-transform: uppercase;
      color: #a6afbb;
      font-size: $small-font-size;
    }
  }
</style>
