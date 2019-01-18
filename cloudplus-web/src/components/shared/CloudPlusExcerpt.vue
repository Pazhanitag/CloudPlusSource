<template>
  <div class="text-wrapper">
    <!-- If there's value prop set, use that instead of text from the slot -->
    <div v-if="$props.value">
      <span v-html="$props.value" v-if="!textTooLong || (textTooLong && expanded)"></span>
      <span v-if="expanded" class="read-more-or-less" @click="toggleExpansion">{{$props.readLessText}}</span>

      <span v-if="(textTooLong && !expanded)">{{excerpt}}</span>
      <span v-if="(textTooLong && !expanded)" class="read-more-or-less" @click="toggleExpansion">{{$props.readMoreText}}</span>
    </div>

    <!-- If there's no value prop set, use text from the slot -->
    <div v-if="!$props.value">
      <slot v-if="!textTooLong || (textTooLong && expanded)"></slot>
      <span v-if="expanded" class="read-more-or-less" @click="toggleExpansion">{{$props.readLessText}}</span>

      <span v-if="(textTooLong && !expanded)">{{excerpt}}</span>
      <span v-if="(textTooLong && !expanded)" class="read-more-or-less" @click="toggleExpansion">{{$props.readMoreText}}</span>
    </div>
  </div>
</template>

<script>
export default {
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
    cutOffAtChar: {
      type: Number,
      default: 500,
    },
    readMoreText: {
      type: String,
      default: '...Read More',
    },
    readLessText: {
      type: String,
      default: '...Read Less',
    },
  },
  computed: {
    textTooLong() {
      if (this.value) {
        return this.value.length > this.$props.cutOffAtChar;
      }
      return this.$slots.default[0].text.length > this.$props.cutOffAtChar;
    },
    excerpt() {
      let fullText;
      if (this.value) {
        fullText = this.value;
      } else {
        fullText = this.$slots.default[0].text;
      }
      return fullText.length < this.$props.cutOffAtChar ?
        fullText :
        fullText.substring(0, this.$props.cutOffAtChar);
    },
  },
  methods: {
    toggleExpansion() {
      this.expanded = !this.expanded;
    },
  },
};
</script>

<style scoped lang="scss">

* {
  -webkit-touch-callout: none;
  -webkit-user-select: none;
  -khtml-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}

.read-more-or-less {
  font-size: 0.875rem;
  font-weight: 550;
  cursor: pointer;
}
</style>

