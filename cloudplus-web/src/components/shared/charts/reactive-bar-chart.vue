<script>
import VueChartJs from 'vue-chartjs';

export default {
  extends: VueChartJs.Bar,
  props: ['chartData', 'chartOptions'],
  watch: {
    chartData: {
      handler: function handler() {
        if (this.$data._chart) {
          this.$data._chart.destroy();
          this.renderChart(this.chartData, this.chartOptions);
        }
      },
      deep: true,
    },
    chartOptions: {
      handler: function handler() {
        if (this.$data._chart) {
          this.$data._chart.destroy();
          this.renderChart(this.chartData, this.chartOptions);
        }
      },
      deep: true,
    },
  },
  mounted() {
    this.renderChart(this.chartData, this.chartOptions);
  },
};
</script>
