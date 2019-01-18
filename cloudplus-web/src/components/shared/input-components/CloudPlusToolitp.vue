<template>
    <div class="tooltip">
      <brand-fa :class="`fa fa-${tooltipIcon} ${iconSize}`"></brand-fa>
      <span class="tooltiptext">{{text}}</span>
    </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  props: {
    tooltipText: String,
    tooltipIcon: {
      type: String,
      default: 'question-circle',
    },
    iconSize: {
      type: String,
    },
  },
  computed: {
    ...mapGetters({
      branding: 'branding/brand',
    }),
  },
  data() {
    return {
      text: this.tooltipText,
    };
  },
};
</script>

<style lang="scss" scoped>

.tooltip {
  position: relative;
  display: inline-block;
  padding-left: 0.3rem;
  cursor: pointer;
}

.tooltip .tooltiptext {
  visibility: hidden;
  width: 21rem;
  background-color: #000000;
  color: #fff;
  text-align: left;
  border-radius: 0.3rem;
  padding: 1rem 1.3rem;
  position: absolute;
  z-index: 1;
  bottom: 100%;
  margin-left: -10rem;
  font-size: $small-font-size;
  font-weight: normal;
}
.tooltip:hover .tooltiptext {
  visibility: visible;
}

.fa-lg {
  font-size: 1.2rem;
}
table .tooltip .tooltiptext {
  left:10rem
}

</style>
