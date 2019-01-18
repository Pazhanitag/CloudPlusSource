<script>
import VueChartJs from 'vue-chartjs';

export default {
  extends: VueChartJs.HorizontalBar,
  props: { data: Object },
  data: function data() {
    return {
      chartData: {},
      chartOptions: {},
    };
  },
  mounted() {
    this.chartData = this.data.chartData;
    this.chartOptions = this.data.chartOptions;
    this.renderChart(this.chartData, this.chartOptions);
  },
};
</script>
