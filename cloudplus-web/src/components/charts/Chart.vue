<template>
<div>
  <div class="nav-wrap">
    <button v-for="(day, index) in days" :key="index" @click="getPageDataByDays(day, index)" :class="{active: activeBtn === index }">{{day}} Days </button>
  </div>
  <loading-icon size="4" v-if="isLoading"></loading-icon>
  <div class="chart-dashboard" v-else>
    <div  class="no-data-warning"  v-if="isEmptyObject(chart)">
      <div class="no-data-warning__title">Oops! Seems like you don&#39;t have any data for this report period!</div>
    </div>
    <div v-else>
      <reactive-line-chart v-if="chart.others.chartType === 4 || chart.others.chartType === 3" :height="300" :chartData="chart.data" :chartOptions="chart.options"></reactive-line-chart>
      <reactive-bar-chart v-if="chart.others.chartType === 1" :height="300" :chartData="chart.data" :chartOptions="chart.options"></reactive-bar-chart>
    </div>
    <loading-icon size="4" v-if="isTableLoading"></loading-icon>
    <div v-if="!isEmptyObject(table)">
      <div class="detail-head">
        <h4>Details</h4>
        <label class="label" v-if="selectedDate">Dated: {{selectedDate}}</label>
      </div>
      <div v-if="table.columnDefs.length > 0">
        <ag-grid-vue class="ag-theme-balham grid"
          :columnDefs="table.columnDefs"
          :rowData="table.rowData"
          :gridAutoHeight="true"
          :enableSorting="true">
        </ag-grid-vue>
      </div>
      <div v-else class="no-data-warning">
        <div class="no-data-warning__title">Oops! Seems like you don&#39;t have any data for this date!</div>
      </div>
    </div>
  </div>
</div>
</template>

<script>
import loadingMixin from '@/mixins/loading';
import { mapActions, mapGetters } from 'vuex';
import { AgGridVue } from 'ag-grid-vue';

export default {
  data() {
    return {
      days: [7, 30, 90, 180],
      urlString: '',
      tableUrlString: '',
      companyId: '',
      activeBtn: '',
      isTableLoading: false,
      table: {},
      reportPeriod: '',
      selectedDate: '',
      limitedChartTypes: [],
    };
  },
  mixins: [loadingMixin],
  computed: {
    ...mapGetters({
      chart: 'chart/chart',
      chartNames: 'dashboard/chartNames',
      userProfile: 'userAuth/userProfile',
    }),
  },
  components: {
    'ag-grid-vue': AgGridVue,
  },
  methods: {
    ...mapActions({
      getGridByDate: 'chart/getGridByDate',
      getChartWithGrid: 'chart/getChartWithGrid',
      getCharts: 'dashboard/getCharts',
    }),
    isEmptyObject(obj) {
      return Object.keys(obj).length === 0 && obj.constructor === Object;
    },
    click(self) {
      return function print(mouseEvent, array) {
        if (array.length > 0) {
          self.selectedDate = '';
          const activePoint = array[0];
          const { data } = activePoint._chart;
          const label = data.labels[activePoint._index];
          self.isTableLoading = true;
          self.table = {};
          self.getGridByDate({
            companyId: self.companyId,
            reportPeriod: self.reportPeriod,
            date: label.replace(/\//g, '-'),
            urlString: self.tableUrlString,
          }).then(gridResponse => {
            self.isTableLoading = false;
            self.setGrid(gridResponse);
            self.selectedDate = label;
          });
        }
      };
    },
    getPageData(obj) {
      this.isLoading = true;
      this.getChartWithGrid(obj)
        .then(grid => {
          if (!this.isEmptyObject(this.chart) && this.urlString !== 'OfficeActivations') {
            this.chart.options.onClick = this.click(this);
          }
          this.isLoading = false;
          this.setGrid(grid);
        });
    },
    getPageDataByDays(day, index) {
      this.reportPeriod = day;
      this.activeBtn = index;
      this.selectedDate = '';
      this.getPageData({
        companyId: this.companyId,
        reportPeriod: this.reportPeriod,
        urlString: this.urlString,
      });
    },
    contains(string, ...localArray) {
      return localArray.includes(string);
    },
    monthToComparableNumber(date) {
      if (date === undefined || date === null || date.length !== 10) {
        return null;
      }
      const yearNumber = date.substring(6, 10);
      const dayNumber = date.substring(3, 5);
      const monthNumber = date.substring(0, 2);

      const result = (yearNumber * 10000) + (monthNumber * 100) + dayNumber;
      return result;
    },
    dateComparator(date1, date2) {
      const date1Number = this.monthToComparableNumber(date1);
      const date2Number = this.monthToComparableNumber(date2);

      if (date1Number === null && date2Number === null) {
        return 0;
      }
      if (date1Number === null) {
        return -1;
      }
      if (date2Number === null) {
        return 1;
      }
      return date1Number - date2Number;
    },
    capitalizeFirstLetter(string) {
      return string.charAt(0).toUpperCase() + string.slice(1);
    },
    setGrid(result) {
      this.table.columnDefs = [];
      this.table.rowData = [];
      if (result.length > 0) {
        if (this.urlString !== 'OfficeActivations') {
          Object.keys(result[0]).forEach(key => {
            const obj = {
              headerName: `${this.capitalizeFirstLetter(key)}`,
              suppressMovable: true,
              lockPosition: true,
              field: `${key.replace(/\s/g, '')}`,
            };
            if (key.includes('(UTC)')) {
              obj.comparator = this.dateComparator;
            }
            this.table.columnDefs.push(obj);
          });
        } else {
          Object.keys(result[0]).forEach(key => {
            const obj = {
              headerName: `${this.capitalizeFirstLetter(key)}`,
              suppressMovable: true,
              lockPosition: true,
              field: `${key.replace(/\s/g, '')}`,
            };
            if (!this.contains(key, 'windows', 'mac', 'windows 10 Mobile', 'iOS', 'android')) {
              this.table.columnDefs.push(obj);
            }
          });
          const dynamicLength = this.table.columnDefs.length - 1;
          this.table.columnDefs[dynamicLength] = { headerName: 'Actions on computers', children: [] };
          this.table.columnDefs[dynamicLength + 1] = { headerName: 'Actions on phones and tablets', children: [] };
          Object.keys(result[0]).forEach(key => {
            const obj = {
              headerName: `${this.capitalizeFirstLetter(key)}`,
              suppressMovable: true,
              lockPosition: true,
              field: `${key.replace(/\s/g, '')}`,
            };
            if (this.contains(key, 'windows', 'mac')) {
              this.table.columnDefs[dynamicLength].children.push(obj);
            } else if (this.contains(key, 'windows 10 Mobile', 'iOS', 'android')) {
              this.table.columnDefs[dynamicLength + 1].children.push(obj);
            }
          });
        }
        result.forEach(row => {
          const obj = {};
          Object.keys(row).forEach(key => {
            const replacedKey = `${key.replace(/\s/g, '')}`;
            obj[replacedKey] = row[key];
            if (replacedKey.includes('Date')) {
              obj[replacedKey] = obj[replacedKey] === '01/01/1900' ? '' : obj[replacedKey];
            }
          });
          this.table.rowData.push(obj);
        });
      }
    },
    starter() {
      this.limitedChartTypes = [];
      this.chartNames.forEach(name => {
        this.limitedChartTypes.push(name.replace(/\s/g, ''));
      });
      if (!this.limitedChartTypes.includes(this.urlString)) {
        this.$router.push('/Dashboard');
      } else {
        this.tableUrlString = `${this.urlString}Details`;
        this.companyId = this.userProfile.companyId;
        this.reportPeriod = 7;
        this.getPageData({
          companyId: this.companyId,
          reportPeriod: this.reportPeriod,
          urlString: this.urlString,
        });
      }
    },
  },
  mounted() {
    this.activeBtn = 0;
    this.urlString = this.$route.query.chartType;
    if (this.chartNames.length > 0) {
      this.starter();
    } else {
      this.isLoading = true;
      this.companyId = this.userProfile.companyId;
      this.getCharts({ userId: this.userProfile.id, companyId: this.companyId }).then(() => {
        this.isLoading = false;
        this.starter();
      });
    }
  },
};
</script>

<style lang="scss" scoped>
.no-data-warning{
    width: 100%;
    padding-top: 18rem;
    text-align: center;
    .no-data-warning__title{
      color: #414f64;
      font-size: 1.25rem;
    }
  }
  .no-data-warning{
    &__title {
      color: $label-color;
      font-size: $big-font-size;
    }
    &__subtitle {
      color: $subtitle-color;
      font-size: $primary-font-size;
    }
  }
  .nav-wrap {
    width: 100%;
    text-align: center;
    background: #AF3333;
    margin-bottom: 40px;
}
.nav-wrap button {
    background: transparent;
    box-shadow: none;
    border: 0;
    font-size: 15px;
    cursor: pointer;
    padding: 10px 15px;
    color: #fff;
    -webkit-transition: 0.3s ease all;
    -moz-transition: 0.3s ease all;
    transition: 0.3s ease all;
}
.nav-wrap button:hover, .nav-wrap button.active{
  background: #ffffff;
  color: #AF3333;
}
.grid {
  width: 100%;
  height: 500px;
  padding: 10px;
}

.chart-dashboard{
  > div{
    .detail-head{
      padding: 0 20px;
      background: #f2f2f2;
      display: inline-block;
      width: 100%;
      h4{
        font-size: 22px;
        float: left;
      }
      .label{
            float: right;
    font-weight: 500;
    padding-top: 10px;
      }
    }
    .no-data-warning{
          padding: 7rem 0;
    }
    &:first-child{
      padding: 20px;
    }
  }
}

</style>

