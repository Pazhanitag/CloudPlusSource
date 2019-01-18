<template>
    <cloud-plus-card-modal 
      :showModal="showModal" 
      @closeModal="closeModal" 
      :disableCloseButton="disableCancelButton" 
      :closeButtonName="cancelText" 
      :ignoreMinHeight="true"
      :width="width">
      <p slot="header">{{title}}</p>
      <section slot="section">
        <div class="modal-message">
          <slot></slot>
        </div>
      </section>
      <div slot="footer">
        <brand-primary-btn @click="confirm" :disabled="disableConfirmButton">{{confirmText}}<loading-icon :inline="true" v-show="showLoadingIcon"></loading-icon></brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
</template>

<script>
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';

export default {
  components: {
    CloudPlusCardModal,
  },
  props: {
    showModal: {
      type: Boolean,
    },
    title: {
      type: String,
      default: '',
    },
    confirmText: {
      type: String,
      default: 'Yes',
    },
    cancelText: {
      type: String,
      default: 'Cancel',
    },
    showLoadingIcon: {
      type: Boolean,
      default: false,
    },
    disableConfirmButton: {
      type: Boolean,
      default: false,
    },
    disableCancelButton: {
      type: Boolean,
      default: false,
    },
    width: {
      type: String,
      default: '',
    },
  },
  methods: {
    closeModal() {
      this.$emit('cancel');
    },
    confirm() {
      this.$emit('confirm');
    },
  },
};
</script>

<style lang="scss" scoped>
  .modal-message {
    padding: 2rem;
    font-size: 0.875rem;
    text-align: center;
  }
</style>

