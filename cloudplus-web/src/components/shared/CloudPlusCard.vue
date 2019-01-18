<template>
  <section class="hero is-fullheight">
    <div class="hero-body">
      <div class="container">
        <div class="columns is-centered">
          <div class="column is-4 is-narrow card-container">
            <div class="card">
              <div class="card-content">
                <div class="card-title">
                  <img v-if="branding.logoUrl" :src="branding.logoUrl" alt="Company Logo">
                  <div v-if="!branding.logoUrl">
                    <h1 class="cp-card-title">
                      <slot name="title">Welcome to ControlPanel</slot>
                    </h1>
                  </div>
                </div>
                <slot></slot>
              </div>
            </div>
            <div class="cp-card-footer">
              <slot name="footer"></slot>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  props: {
    email: {
      default: '',
    },
  },
  watch: {
    $route() {
      this.getBrandingByEmail();
    },
  },
  methods: {
    ...mapActions({
      getBranding: 'branding/getBrandingForUser',
    }),
    getBrandingByEmail() {
      if (this.email) {
        this.getBranding(this.email);
      }
    },
  },
  computed: {
    ...mapGetters({
      branding: 'branding/brand',
    }),
  },
  created() {
    this.getBrandingByEmail();
  },
};
</script>

<style scoped lang="scss">
body {
  background-color: #f2f8f8;
}

a {
  color: #2f88c1;
}

.hero.is-fullheight {
  background-color: #f2f8f8;
}

.card-container {
  min-width: 30rem;
}

@media only screen and (max-width: 515px) {
  .card-container {
    min-width: 6.25rem;
  }
}

.card {
  border-top: 0.125rem solid #2F88C1;
  border-radius: 0.125rem;
  box-shadow: 0.3125rem 0.3125rem 0.3125rem 0 #D2D8D8, -0.3125rem 0 0.3125rem -0.3125rem #D2D8D8;
}

.card:hover {
  border: none;
  border-top: 0.125rem solid #2F88C1;
}

.card-content {
  overflow: auto;
  padding: 4.375rem 3.125rem 2.187rem 3.125rem;
}

.card-title {
  padding-bottom: 3.125rem;
  text-align: center;
}

.input {
  background-color: #f9fafc;
}

.cp-card-title {
  font-size: 1.5rem;
  color: #56575B;
  text-align: center;
}

.cp-card-subtitle {
  font-size: $secondary-font-size;
  padding: 1.875rem 1.25rem 0 1.25rem;
  color: #CCCCCC;
  text-align: center;
}

.cp-card-footer {
  font-size:  $secondary-font-size;
  color: #CCCCCC;
  text-align: center;
  padding: 1.25rem 3.75rem 0rem 3.75rem;
}
</style>
