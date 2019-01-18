export default {
  data() {
    return {
      orderBy: '',
      order: 'asc',
    };
  },
  computed: {
    ordChange() {
      return this.order + this.orderBy;
    },
  },
  methods: {
    sortBy(key) {
      if (key === this.orderBy) {
        if (this.order === 'asc') this.order = 'desc';
        else this.order = 'asc';
      } else {
        this.order = 'asc';
      }
      this.orderBy = key;
    },
  },
};
