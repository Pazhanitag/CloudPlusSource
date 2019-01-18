<template>
  <div class="paging-info ">
    <p v-if="usersLength > 0">
      <span>Showing results </span>
      <span>{{pageRangeDisplayedStart}} - </span>
      <span v-if="currentPage < pageCount">{{pageRangeDisplayedEnd}} of </span>
      <span v-else>{{userCount}} of </span>
      <span>{{userCount}}</span>
    </p>

    <div class="level center" v-if="usersLength > 0">
      <div class="level-item pagination-bottom">
        <nav class="pagination is-small center" role="navigation" aria-label="pagination">
          <ul class="pagination-list">
            <a class="pagination-previous" @click="setPagePrevious(currentPage - 1)" :disabled="ifPreviousButtonShouldBeDisabled">
              <i class="fa fa-caret-left"></i>
            </a>
            <li v-for="pageNumber in pageCount" :key="pageNumber">
              <span v-if="shouldShowEllipsis(pageNumber, true)" class="pagination-ellipsis">&hellip;</span>
              <a @click="setPage(pageNumber)" :class="['pagination-link no-margins', {'is-current current': currentPage === pageNumber}]" v-if="shouldShowPageNumber(pageNumber)">{{ pageNumber }}</a>
              <span v-if="shouldShowEllipsis(pageNumber, false)" class="pagination-ellipsis">&hellip;</span>
            </li>
            <a class="pagination-next" @click="setPageNext(currentPage + 1)" :disabled="ifNextButtonShouldBeDisabled">
              <i class="fa fa-caret-right"></i>
            </a>
          </ul>
        </nav>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: ['usersLength', 'usersPerPage', 'currentPage', 'pageCount', 'userCount'],
  data() {
    return {};
  },
  computed: {
    pageRangeDisplayedStart() {
      return ((this.currentPage * this.usersPerPage) - this.usersPerPage) + 1;
    },
    pageRangeDisplayedEnd() {
      return this.currentPage * this.usersPerPage;
    },
    ifPreviousButtonShouldBeDisabled() {
      return this.currentPage === 1;
    },
    ifNextButtonShouldBeDisabled() {
      return this.currentPage === this.pageCount;
    },
  },
  methods: {
    shouldShowEllipsis(pageNumber, isFirst) {
      if (isFirst) {
        return pageNumber === 2 && Math.abs(pageNumber - this.currentPage) > 3;
      }
      return pageNumber === this.pageCount - 1 && Math.abs(pageNumber - this.currentPage) > 3;
    },
    shouldShowPageNumber(pageNumber) {
      return Math.abs(pageNumber - this.currentPage) < 3
        || pageNumber === this.pageCount
        || pageNumber === 1;
    },
    setPage(pageNumber) {
      this.$emit('setPage', pageNumber);
    },
    setPagePrevious(pageNumber) {
      if (!this.ifPreviousButtonShouldBeDisabled) {
        this.$emit('setPagePrevious', pageNumber);
      }
    },
    setPageNext(pageNumber) {
      if (!this.ifNextButtonShouldBeDisabled) {
        this.$emit('setPageNext', pageNumber);
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.showingResults {
  padding-top: 0.3125rem;
  padding-bottom: 0.3125rem;
  font-size: $secondary-font-size;
}

.paginationLevel {
  padding-bottom: 0.625rem;
  background-color: white;
  margin-bottom: 3.125rem;
  padding-top: 0.625rem;
}

.pagination-custom {
  padding: 0;
  margin: 0.5rem 0;
}

.pagination-custom li {
  display: inline-block;
  border: $border-height solid #ddd;
  padding: 0.5rem 0.375rem;
  text-decoration: none;
  font-size: $small-font-size;
}

.pagination-custom a {
  color: #bdbdbd;
  padding: 0.25rem 0.25rem;
  text-decoration: none;
}

.pagination-previous {
  -webkit-box-ordinal-group: 3;
  -ms-flex-order: 2;
  order: 0;
}

.pagination-link.is-current {
  background-color: #ffffff;
  border-color: #dbdbdb;
  color: #000000;
}

.paging-info {
  font-size: $secondary-font-size;
  color: $label-color;
}

a.current {
  font-weight: bold;
  color: #000000;
}

.no-margins {
  margin: 0rem;
  color: #bdbdbd;
}
.margin-left {
  margin-left: 40px;
}
.pagination-bottom {
    padding-bottom: 40px;
}

// zero padding for My Services Expansion Panel -> compact view
.pagination-zero-bottom .level .level-item.pagination-bottom{
  padding-bottom: 0px;
}

</style>
