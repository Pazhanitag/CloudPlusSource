<template>
  <ul class="submenu">
    <li v-for="(item, key) in items" :key="key" class="submenu__item">
      <brand-hover>
        <router-link :to="item.href" class="submenu__item__link" exact>
          <span v-if="item.icon" :class="['fa', 'fa-' + item.icon]" ></span>{{item.name}}
        </router-link>
      </brand-hover>
    </li>
  </ul>
</template>

<script>
  export default {
    props: ['items'],
  };
</script>

<style lang="scss">
  .submenu {
    overflow: hidden;
    padding-bottom: 1.25rem;
    margin-top: 0rem;
    &__item {
      &__link{
        display: block;
        padding: 0.5rem 3.125rem;
        color: #54667a;
        font-size: $secondary-font-size;
      }
    }
    .fa {
      padding-right: 0.625rem;
    }
  }

</style>
