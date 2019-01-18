<template>
  <div v-if="src && src !== ''" class="avatar" :style="initialsStyle">
  </div>
  <div v-else class="avatar initials" v-bind:style="initialsStyle">
    <span class="is-size-6 has-text-weight-light">{{initials}}</span>
  </div>
</template>

<script>
export default {
  props: {
    src: {
      type: String,
    },
    size: {
      type: Number,
      default: 35,
    },
    fullName: {
      type: String,
    },
    initialsLength: {
      type: Number,
      default: 2,
    },
  },
  computed: {
    initials() {
      return this.fullName.replace(/[^a-zA-Z- ]/g, '').match(/\b\w/g).slice(0, this.initialsLength).join('');
    },
    initialsStyle() {
      return {
        'background-image': `url(${this.src})`,
        width: `${this.size}px`,
        height: `${this.size}px`,
      };
    },
    avatarStyle() {
      return {
        width: `${this.size}px`,
        height: `${this.size}px`,
      };
    },
  },
  mounted() {
  },
};
</script>

<style scoped>
.avatar {
  display: inline-block;
  border-radius: 50%;
  background-repeat: no-repeat;
  background-position: center center;
  background-size: cover;
}

.initials {
  background-color: #d0dfe6;
  display: flex;
  justify-content: center;
  align-items: center;
}

.initials span {
  color: #756868;
}
</style>
