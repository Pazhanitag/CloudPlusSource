<template>
  <div>
    <div class="container">
      <brand-step-progressbar :numOfSteps="numOfSteps">
        <brand-step :numOfSteps="numOfSteps" :class="{ 'active' : key + 1 === activeStep }" v-for="(step, key) in steps" :key="key">
          <div>{{step}}</div>
          <div class="step-status" v-if="key + 1 <= activeStep">
            <div class="step-status__complete" v-if="key + 1 < activeStep">Completed</div>
            <div class="step-status__inProgress" v-if="key + 1 === activeStep">In progress</div>
          </div>
        </brand-step>
      </brand-step-progressbar>
    </div>
    <div class="step-details">
      <div v-if="key + 1 === activeStep" v-for="(step, key) in steps" :key="key">
        <slot :name="key+1"></slot>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    activeStep: {
      required: true,
    },
    steps: {
      required: true,
    },
  },
  computed: {
    numOfSteps() {
      return this.steps.length;
    },
  },
};
</script>

<style lang="scss" scoped>
.step-details {
  margin-top: 5.625rem;
  padding-top: 1.875rem;
}

.container {
  width: 100%;
  padding-top: 2.187rem;
}

.step-status {
  text-align: center;
  font-size: $small-font-size;
  &__complete {
    color: #23d160;
  }
  &__inProgress {
    color: #ffc000;
  }
}
</style>
