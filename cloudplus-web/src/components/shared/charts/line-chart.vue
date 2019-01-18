<script>
import VueChartJs from 'vue-chartjs';

export default {
  extends: VueChartJs.Line,
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
    if (this.$data._chart != null) {
      this.$emit('Legends', { labels: this.$data._chart.generateLegend(), index: this.data.index });
    }
  },
};
</script>
