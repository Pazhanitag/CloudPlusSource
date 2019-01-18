<template>
  <div>
    <div class="text-wrapper">
      <span class="main-text" v-html="$props.value" v-if="!textTooLong"></span>
      <span class="main-text" v-html="excerpt" v-else></span>
      <brand-color-primary class="read-more-or-less"><span v-if="(textTooLong && !expanded)" @click="openModal">{{$props.readMoreText}}</span></brand-color-primary>
    </div>
    <cloud-plus-card-modal v-if="expanded" :showModal="expanded" closeButtonName="Close" @closeModal="closeModal">
      <p slot="header">{{title}}</p>
      <section class="main-modal-section" slot="section">
        <slot></slot>
        <span class="main-text" v-html="$props.value"></span>
      </section>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';

export default {
  components: {
    CloudPlusCardModal,
  },
  data() {
    return {
      expanded: false,
    };
  },
  props: {
    value: {
      type: String,
      default: '',
    },
    title: {
      type: String,
      default: 'More Info',
    },
    cutOffAtChar: {
      type: Number,
      default: 300,
    },
    readMoreText: {
      type: String,
      default: 'View more',
    },
  },
  computed: {
    textTooLong() {
      return this.value.length > this.$props.cutOffAtChar;
    },
    excerpt() {
      const fullText = this.value;
      return fullText.length < this.$props.cutOffAtChar ?
        fullText :
        `${fullText.substring(0, this.$props.cutOffAtChar)}...`;
    },
  },
  methods: {
    openModal() {
      this.expanded = true;
    },
    closeModal() {
      this.expanded = false;
    },
  },
};
</script>

<style scoped lang="scss">

.read-more-or-less {
  font-size: 0.875rem;
  font-weight: 400;
  cursor: pointer;
  text-decoration: underline;
  display: inline;
}

.main-modal-section {
  padding-left:10px;
}

.main-text {
  font-size: $secondary-font-size;
}  
</style>

