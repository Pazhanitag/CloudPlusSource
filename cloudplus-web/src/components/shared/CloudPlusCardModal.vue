<template>
  <div :class="{modal: true, 'is-active': showModalLocal}">
    <div class="modal-background"></div>
    <div class="modal-card" :class="{'min-height-setting' : !ignoreMinHeight}" :style="{width: width}">

        <header>
          <brand-background-with-text-color class="modal-card-head">
            <slot name="header"></slot>
          </brand-background-with-text-color>
        </header>

      <div v-if="hasTitle" class="modal-content" :style="{height: modalContentHeight, width: width}" >
        <slot name="content">
          <slot name="title">
            <h1></h1>
          </slot>
          <slot name="subtitle">
            <h2></h2>
          </slot>
        </slot>
      </div>

      <section class="modal-card-body">
        <slot name="section"></slot>
      </section>

      <footer class="modal-card-foot">
        <div class="level">
          <div class="level-right">
            <div class="level-item">
              <brand-reverse-primary-btn v-show="closeButtonVisible" @click="close" :disabled="disableCloseButton">{{closeButtonName}}</brand-reverse-primary-btn>
            </div>
            <div class="level-item">
              <slot name="footer"></slot>
            </div>
          </div>
        </div>
      </footer>
    </div>
  </div>
</template>
<script>

export default {
  props: {
    showModal: {
      type: Boolean,
    },
    modalContentHeight: {
      type: String,
      default: '11.875rem',
    },
    width: {
      type: String,
      default: '',
    },
    ignoreMinHeight: {
      type: Boolean,
      default: false,
    },
    closeButtonName: {
      type: String,
      default: 'Cancel',
    },
    disableCloseButton: {
      type: Boolean,
      default: false,
    },
    closeButtonVisible: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      showModalLocal: this.showModal,
    };
  },
  computed: {
    hasTitle() {
      return !!this.$slots.content;
    },
  },
  methods: {
    close() {
      this.showModalLocal = false;
      this.$emit('closeModal');
    },
  },
};
</script>

<style lang="scss" scoped>
.modal {
  z-index: 9999;
}

.modal-card {
  min-width: 34.37rem;
  margin: 0;
}

.min-height-setting {
  min-height: 35rem;
}

.modal-card-head {
  font-size: $font-size;
}

.modal-content {
  text-align: center;
  background-color: white;
  margin-right: 0rem;
  margin-left: 0rem;
  overflow: hidden;
  font-size: $primary-font-size;
  overflow: visible;
}

.modal-card-foot {
  justify-content: flex-end;
  padding-top: 1.25rem;
  padding-bottom: 1.25rem;
  border-bottom-width: 1.25rem;
  background-color: white;
}

@media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
  .modal-card-body section{
    overflow-y: scroll;
    overflow-x: hidden;
    max-height: 15rem;
  }
}

</style>
