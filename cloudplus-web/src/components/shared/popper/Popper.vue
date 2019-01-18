<template>
  <span>
    <transition :name="transition" :enter-active-class="enterActiveClass" :leave-active-class="leaveActiveClass" @after-leave="doDestroy">
      <span
        ref="popper"
        v-show="!disabled && showPopper">
        <slot>{{ content }}</slot>
      </span>
    </transition>
    <slot name="reference"></slot>
  </span>
</template>

<script>
  import Popper from 'popper.js';

  function on(element, event, handler) {
    if (element && event && handler) {
      if (document.addEventListener) {
        element.addEventListener(event, handler, false);
        return;
      }
      element.attachEvent(`on${event}`, handler);
    }
  }
  function off(element, event, handler) {
    if (element && event) {
      if (document.removeEventListener) {
        element.removeEventListener(event, handler, false);
        return;
      }
      element.detachEvent(`on${event}`, handler);
    }
  }
  export default {
    props: {
      trigger: {
        type: String,
        default: 'hover',
        validator: value => ['click', 'hover'].indexOf(value) > -1,
      },
      delayOnMouseOut: {
        type: Number,
        default: 10,
      },
      disabled: {
        type: Boolean,
        default: false,
      },
      content: String,
      enterActiveClass: String,
      leaveActiveClass: String,
      boundariesSelector: String,
      reference: {},
      forceShow: {
        type: Boolean,
        default: false,
      },
      appendToBody: {
        type: Boolean,
        default: false,
      },
      visibleArrow: {
        type: Boolean,
        default: true,
      },
      transition: {
        type: String,
        default: '',
      },
      options: {
        type: Object,
        default() {
          return {};
        },
      },
    },
    data() {
      return {
        referenceElm: null,
        popperJS: null,
        showPopper: false,
        currentPlacement: '',
        popperOptions: {
          placement: 'bottom',
          gpuAcceleration: false,
        },
      };
    },
    watch: {
      showPopper(value) {
        if (value) {
          this.$emit('show');
          this.updatePopper();
        } else {
          this.$emit('hide');
        }
      },
      forceShow: {
        handler(value) {
          this[value ? 'doShow' : 'doClose']();
        },
        immediate: true,
      },
    },
    created() {
      this.popperOptions = Object.assign(this.popperOptions, this.options);
    },
    mounted() {
      this.referenceElm = this.reference || this.$slots.reference[0].elm;
      this.popper = this.$slots.default[0].elm;
      switch (this.trigger) {
        case 'click':
          on(this.referenceElm, 'click', this.doToggle);
          on(document, 'click', this.handleDocumentClick);
          break;
        case 'hover':
          on(this.referenceElm, 'mouseover', this.onMouseOver);
          on(this.popper, 'mouseover', this.onMouseOver);
          on(this.referenceElm, 'mouseout', this.onMouseOut);
          on(this.popper, 'mouseout', this.onMouseOut);
          break;
        default:
          break;
      }
      this.createPopper();
    },
    methods: {
      doToggle() {
        if (!this.forceShow) {
          this.showPopper = !this.showPopper;
        }
      },
      doShow() {
        this.showPopper = true;
      },
      doClose() {
        this.showPopper = false;
      },
      doDestroy() {
        if (this.showPopper || !this.popperJS) {
          return;
        }
        this.popperJS.destroy();
        this.popperJS = null;
      },
      createPopper() {
        this.$nextTick(() => {
          if (this.visibleArrow) {
            this.appendArrow(this.popper);
          }
          if (this.appendToBody) {
            document.body.appendChild(this.popper.parentElement);
          }
          if (this.popperJS && this.popperJS.destroy) {
            this.popperJS.destroy();
          }
          this.popperOptions.preventOverflow = {
            boundariesElement: this.popper,
          };
          this.popperOptions.modifiers = {
            preventOverflow: {
              enabled: false,
            },
            hide: {
              enabled: false,
            },
            computeStyle: {
              gpuAcceleration: false,
            },
          };
          this.popperOptions.onCreate = () => {
            this.$emit('created', this);
            this.$nextTick(this.updatePopper);
          };
          this.popperJS = new Popper(this.referenceElm, this.popper, this.popperOptions);
        });
      },
      destroyPopper() {
        off(this.referenceElm, 'click', this.doToggle);
        off(this.referenceElm, 'mouseup', this.doClose);
        off(this.referenceElm, 'mousedown', this.doShow);
        off(this.referenceElm, 'focus', this.doShow);
        off(this.referenceElm, 'blur', this.doClose);
        off(this.referenceElm, 'mouseout', this.onMouseOut);
        off(this.referenceElm, 'mouseover', this.onMouseOver);
        off(document, 'click', this.handleDocumentClick);
        this.popperJS = null;
        if (this.appendToBody) {
          document.body.removeChild(this.popper.parentElement);
        }
      },
      appendArrow(element) {
        if (this.appended) {
          return;
        }
        this.appended = true;
        const arrow = document.createElement('div');
        arrow.setAttribute('x-arrow', '');
        arrow.className = 'popper__arrow';
        element.appendChild(arrow);
      },
      updatePopper() {
        if (this.popperJS) {
          this.popperJS.scheduleUpdate();
          return;
        }
        this.createPopper();
      },
      onMouseOver() {
        this.showPopper = true;
        clearTimeout(this.mouseOutTimer);
      },
      onMouseOut() {
        this.mouseOutTimer = setTimeout(() => {
          this.showPopper = false;
        }, this.delayOnMouseOut);
      },
      handleDocumentClick(e) {
        if (!this.$el || !this.referenceElm ||
          this.$el.contains(e.target) ||
          this.referenceElm.contains(e.target)
        ) {
          return;
        }
        this.$emit('documentClick');
        if (this.forceShow) {
          return;
        }
        this.showPopper = false;
      },
    },
    destroyed() {
      this.destroyPopper();
    },
  };
</script>

<style>

</style>
