<template>
  <div class="sticky" :class="{shadow: showShadow}">
    <div class="columns">
      <div class="column">
        <div class="title-container">
          <brand-color-primary>
            <span class="header-title has-text-weight-semibold">{{title}}</span>
          </brand-color-primary>
        </div>
        <div class="is-pulled-right" :class="[{ 'slotContentsFlex': flexTheContent }, { 'slotContentsStretch': stretchTheContent }]">
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      default: '',
    },
    flexTheContent: {
      type: Boolean,
      default: false,
    },
    stretchTheContent: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      showShadow: false,
    };
  },
  created() {
    this.$bus.on('contentScrolled', this.toggleShowShadow);
  },
  methods: {
    toggleShowShadow(contentScrollTop) {
      this.showShadow = contentScrollTop > 0;
    },
  },
};
</script>

<style scoped lang="scss">
.columns {
  padding: 1rem 1.5rem 1rem 1.2rem;
}

.title-container {
  float: left;
  display: inline;
  margin: 0.4rem 0.93rem;
}

.header-title {
  font-size: 1.1rem;
}

.sticky {
  width: calc(100% - #{$menu-width});
  background-color: #f2f7f8;
  position: fixed;
  z-index: 100;
  height: $sticky-header-height;
  transition: all 0.3s ease-in-out;
}

.sticky.shadow {
    box-shadow: 0 3px 2px -2px rgba(0, 0, 0, 0.1);
    -webkit-box-shadow: 0 4px 2px -2px rgba(0, 0, 0, 0.2);
}

.slotContentsFlex {
  display: flex;
}

.slotContentsStretch {
  width: 100%;
  justify-content: flex-end;
}
</style>
