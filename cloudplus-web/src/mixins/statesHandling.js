import { usStates, canadianStates } from '@/assets/constants/states';

export default {
  data() {
    return {
      usStates,
      canadianStates,
    };
  },
  computed: {
    rootObject() {
      if (this.company) return this.company;
      else if (this.user) return this.user;
      return {};
    },
    states() {
      if (this.rootObject.country === 'United States' || this.rootObject.country === 'US') {
        return this.mapStates(this.usStates);
      } else if (this.rootObject.country === 'Canada' || this.rootObject.country === 'CA') {
        return this.mapStates(this.canadianStates);
      }
      return [];
    },
    country() {
      return this.rootObject.country;
    },
    state() {
      return this.rootObject.state;
    },
    showStatesDropdown() {
      return ['United States', 'US', 'Canada', 'CA'].indexOf(this.rootObject.country) >= 0;
    },
  },
  watch: {
    country(newValue) {
      this.setFirstState(newValue);
    },
  },
  methods: {
    mapStates(states) {
      return states.map(state => ({
        name: state.name,
        value: state.abbreviation,
      }));
    },
    setFirstState(country) {
      let state;
      if (['United States', 'US'].indexOf(country) >= 0) {
        [state] = this.mapStates(this.usStates);
      } else if (['Canada', 'CA'].indexOf(country) >= 0) {
        [state] = this.mapStates(this.canadianStates);
      } else {
        [state] = [{ name: '', value: '' }];
      }
      this.setState(state.value);
    },
  },
  created() {
    if (!this.rootObject.state) {
      this.setFirstState(this.country);
    }
  },
};
