<template>
<div>
  <component-sticky-header :title="'Dashboard'">
    <brand-primary-btn @click="goto('customization')">Settings</brand-primary-btn>
  </component-sticky-header>
  <div class="dashboard-content">
    <loading-icon size="4" v-if="isLoading"></loading-icon>
    <div class="chart-dashboard" v-else>
      <div class="chart-item" :class="[activeClass]" v-for="(chart, index) in charts" :key="index" @click="goto( 'chart', chart.others.url)">
        <div class="chart-wrap">
          <div class="chart-head">
            <h2>
              <figure>
                <img :src="chart.others.imageUrl" alt="">
              </figure>
              <div class="fig-content">{{chartNames[index]}}</div>
            </h2>
            <div class="chart-total-wrap">
                <div class="chart-count">
                  <span class="cursor-pointer">Total: {{chart.others.total}}</span>
                  <span class="tooltip">{{chart.others.toolTip}}</span>
                </div>
                <div class="chart-percent" v-if="chart.others.trend !== 0">{{chart.others.trend >= 0 ? '+' : '' }} {{chart.others.trend}}%
                  <img src="/static/images/up_arrow.png" alt="" v-if="chart.others.trend >= 0">
                  <img src="/static/images/down_arrow.png" alt="" v-else>
                </div>
            </div>
          </div>
          <div v-if="chart.type === 1">
            <span v-html="chart.labels" v-if="barChartLabelToggle"></span>
            <bar-chart @Legends="barChartLegends" :height="300" :data="{chartData: chart.data, chartOptions: chart.options, index}"></bar-chart>
          </div>
          <horizontal-bar-chart v-if="chart.type === 2" :height="300" :data="{chartData: chart.data, chartOptions: chart.options}"></horizontal-bar-chart>
          <div v-if="chart.type === 3">
          <span v-html="chart.labels" v-if="areaChartLabelToggle"></span>
          <line-chart @Legends="areaChartLegends" :height="300" :data="{chartData: chart.data, chartOptions: chart.options, index}"></line-chart>
          </div>
        </div>
      </div>
      <div v-if="charts.length === 0" class="no-charts-warning">
        <div class="no-charts-warning__title">oh no! There is no data to make your dashboard!</div>
        <router-link class="no-charts-warning__subtitle" :to="'/catalogs/customer'" >
          <brand-color-primary><span class="link">The Catalog</span></brand-color-primary>
        </router-link>
      </div>
    </div>
  </div>
</div>
</template>

<script>
import loadingMixin from '@/mixins/loading';
import { mapActions, mapGetters } from 'vuex';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';

export default {
  data() {
    return {
      activeClass: '',
      barChartLabelToggle: false,
      areaChartLabelToggle: false,
    };
  },
  components: {
    ComponentStickyHeader,
  },
  mixins: [loadingMixin],
  computed: {
    ...mapGetters({
      charts: 'dashboard/charts',
      chartNames: 'dashboard/chartNames',
      userProfile: 'userAuth/userProfile',
    }),
  },
  methods: {
    ...mapActions({
      getCharts: 'dashboard/getCharts',
    }),
    barChartLegends({ labels, index }) {
      this.barChartLabelToggle = this.barChartLabelToggle || true;
      this.charts[index].labels = labels;
    },
    areaChartLegends({ labels, index }) {
      this.areaChartLabelToggle = this.areaChartLabelToggle || true;
      this.charts[index].labels = labels;
    },
    goto(target, param) {
      if (param) {
        this.$router.push({ path: target, query: { chartType: param } });
      } else {
        this.$router.push({ path: target });
      }
    },
  },
  mounted() {
    const { companyId } = this.userProfile;
    const userId = this.userProfile.id;
    this.getCharts({ userId, companyId }).then(() => {
      this.isLoading = false;
      if (this.charts.length < 4) {
        this.activeClass = `chart-item-${this.charts.length}`;
      }
    });
  },
};
</script>

<style lang="scss" scoped>
  .chart-dashboard {
    margin: 0 ;
    padding: 0 15px;
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    -ms-flex-wrap: wrap;
    flex-wrap: wrap;
    background-color: #fff;
    height: 100%;
  }
  .chart-dashboard .chart-item {
    width: 25%;
    float: left;
    padding: 15px 15px 25px;
    min-height: 525px;
    background: #ffffff;
  }
  .chart-dashboard .chart-item.chart-item-1{
    width:100%;
  }
   .chart-dashboard .chart-item.chart-item-2{
    width:50%;
   }
    .chart-dashboard .chart-item.chart-item-3{
    width:33.33%;
   }
  .chart-dashboard .chart-item > div{
    -webkit-box-shadow:0px 0px 16px 0px rgba(0, 0, 0, 0.18);
    -moz-box-shadow:0px 0px 16px 0px rgba(0, 0, 0, 0.18);
    box-shadow:0px 0px 16px 0px rgba(0, 0, 0, 0.18);
    height: 100%;
  }
  .chart-dashboard:after, .chart-dashboard:before {
    content: '';
    display: table;
    clear: both;
  }
  .chart-head {
    padding: 10px 12px;
    text-align: left;
    background: #f5f5f5;
    margin-bottom: 10px;
    min-height: 170px;
    position: relative;
  }
  .chart-head h2 {
    font-size: 0;
    color: #000;
    width: 100%;
    min-height: 70px;
    line-height: normal;
    display: inline-block;
  }
  .no-charts-warning{
    width: 100%;
    padding-top: 18rem;
    text-align: center;
    .no-charts-warning__title{
      color: #414f64;
      font-size: 1.25rem;
    }
  }
  .no-charts-warning{
    &__title {
      color: $label-color;
      font-size: $big-font-size;
    }
    &__subtitle {
      color: $subtitle-color;
      font-size: $primary-font-size;
    }
  }

  .chart-total-wrap {
    width: 100%;
    text-align: left;
    padding-top: 13px;
    color: #000;
    display: inline-block;
  }

  .chart-count {
    font-size: 22px;
    float: left;
    max-width: 135px;
  }

  .chart-percent {
    float: right;
    max-width: calc(100% - 135px);
    width: 100%;
    text-align: right;
    padding-top: 10px;
  }
  .chart-head h2 figure{
    display: inline-block;
    width: 50px;
    vertical-align: middle;
    margin-right: 10px;
  }
  .chart-head h2 .fig-content{
    width: 100%;
    text-align: left;
    display: inline-block;
    font-size: 18px;
    vertical-align: middle
  }
  .chart-head h2 figure + .fig-content{
    width: calc(100% - 80px);
  }
  .chart-head + div {
    padding: 0 10px;
  }
  .dashboard-content{
    clear: both;
    height: calc(100vh - 65px);
    padding-top: 65px;
  }

  @media screen and (max-width:1300px){
    .chart-count span{
      display: block;
    }
    .chart-count{
      font-size: 20px;
      line-height: normal;
      max-width: 80px;
    }
    .chart-percent{
          max-width: calc(100% - 80px);
    }

    .chart-head h2 figure{
      width: 40px;
    }
    .chart-head h2 figure + .fig-content {
    width: calc(100% - 55px);
    }
    .chart-head h2 .fig-content{
        font-size: 16px;
    }
    .chart-count{
      font-size: 17px;
    }
  }

@media screen and (max-width:991px){
  .chart-dashboard .chart-item{
    width: 33.33%;
  }
}
@media screen and (max-width:767px){
  .chart-dashboard .chart-item{
    width: 50%;
  }
}
@media screen and (max-width:640px){
  .chart-dashboard .chart-item{
    width: 100%;
  }
    .chart-dashboard .chart-item.chart-item-2{
     width:100%;
   }
    .chart-dashboard .chart-item.chart-item-3{
     width:100%;
   }
    .chart-dashboard .chart-item.chart-item-1{
     width:100%;
    }
}
.tooltip {
    position: absolute;
    font-size: 9px;
    top: auto;
    bottom: auto;
    left: auto;
    background: #000;
    color:#ffffff;
    border: 1px solid #ccc;
    border-radius: 2px;
    line-height: normal;
    padding: 3px 7px;
    box-shadow: 0px 0px 6px 2px #ccc6;
    -webkit-transition: 0.3s ease all;
    -moz-transition: 0.3s ease all;
    transition: 0.3s ease all;
    opacity: 0;
    visibility: hidden;
    display: block;
    z-index: 9;
    white-space: normal;
    word-break: break-all;
    -webkit-transform: translateY(-50px);
    -moz-transform: translateY(-50px);
    transform: translateY(-50px);
  }
  .chart-count:hover .tooltip{
    top: auto;
    bottom: auto;
    opacity: 1;
    visibility: visible;
    -webkit-transform: translateY(-60px);
    -moz-transform: translateY(-60px);
    transform: translateY(-60px);
  }
</style>

