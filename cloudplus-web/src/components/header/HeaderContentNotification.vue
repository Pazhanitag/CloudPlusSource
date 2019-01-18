<template>
  <div class="header-content-notification">
    <i :class="['fa', 'fa-' + icon, 'header-content-notification__icon']" ></i>
    <span class="header-content-notification__count">{{count}}</span>
  </div>
</template>

<script>
  export default {
    props: ['icon', 'count'],
  };
</script>

<style lang="scss">
  .header-content-notification{
    margin-right: 0.625rem;
    position: relative;
    &__icon {
      font-size: $big-font-size;
      padding-right: 0.625rem;
    }
    &__count {
      background-color: #ff2b71;
      border-radius: 50%;
      color: #ffffff;
      display: flex;
      font-size: $notification-font-size;
      justify-content: center;
      line-height: 1;
      padding: 0.125rem;
      position: absolute;
      right: 0.3125rem;
      top: -0.3125rem;
      width: 0.875rem;
    }
  }
</style>
