<template>
  <div v-if="companies.length > 0">
    <span>{{visibleCompaniesFormatted}}</span>
    <popper v-if="showPopper" trigger="hover" :appendToBody="true" :options="{placement: 'bottom-end'}">
      <div class="popper" :class="{'account-overflow' : companiesShownInTooltip.length > 3}">
          <div class="dropdown-content">
            <div v-for="(company, index) in companiesShownInTooltip" :key="index">
              <span class="dropdown-item">
                {{company.companyName}}
              </span>
              <hr v-if="index < companiesShownInTooltip.length - 1" class="dropdown-divider">
            </div>
          </div>
      </div>
      <a slot="reference" class="has-text-weight-bold	" aria-label="more options">
        + {{companies.length -2}} more
      </a>
    </popper>
  </div>
</template>

<script>
import Popper from '@/components/shared/popper/Popper';

export default {
  components: { Popper },
  props: {
    companies: {
      required: true,
      type: Array,
    },
  },
  computed: {
    showPopper() {
      return this.companies.length > 2;
    },
    visibleCompaniesFormatted() {
      if (this.showPopper) {
        return this.companies.map(c => c.companyName).slice(0, 2).join(', ');
      }
      return this.companies.map(c => c.companyName).join(', ');
    },
    companiesShownInTooltip() {
      return this.companies.slice(2, this.companies.length);
    },
  },
};
</script>

<style scoped lang="scss">
.account-name:not(:last-child) {
    padding: 0rem;
}
.dropdown-divider{
  margin: 2px;
}
.account-overflow {
  overflow-y: scroll;
}
</style>
