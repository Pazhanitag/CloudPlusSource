import { minimum, maximum, formatter, backgroundColors, initializeChartData, initializeDataSets, initializeOptions, isEmptyObject } from '../../../helpers/utils';

export default {
  SET_CHART(state, chart) {
    state.chart = {};
    if (!isEmptyObject(chart)) {
      state.chart.others = {};
      state.chart.others = chart.others;
      state.chart.data = initializeChartData();
      state.chart.options = {};
      if (chart.others.chartType === 1) {
        state.chart.data.datasets = initializeDataSets(chart.legends.length, '', true);
        chart.legends.forEach((legend, i) => {
          state.chart.data.datasets[i].label = legend;
          state.chart.data.datasets[i].backgroundColor = backgroundColors[i];
          // state.chart.data.datasets[i].data.push(chart.values[i]);
        });
        chart.values.forEach((value, i) => {
          state.chart.data.datasets[i].data.push(value);
        });
        state.chart.options = initializeOptions({ display: true, position: 'bottom', text: '' }, false, { display: true, position: 'bottom' });
        state.chart.options.scales.yAxes[0].ticks.callback = formatter();
      }
      if (chart.others.chartType === 3) {
        state.chart.data.datasets = initializeDataSets(chart.legends.length, '', true);
        chart.props.forEach(prop => {
          // if (i === 0 || i === chart.props.length - 1) {
          state.chart.data.labels.push(prop);
          // }
        });
        chart.legends.forEach((legend, i) => {
          state.chart.data.datasets[i].label = legend;
          state.chart.data.datasets[i].backgroundColor = backgroundColors[i];
          chart.values[i].forEach(value => {
            state.chart.data.datasets[i].data.push(value);
          });
        });
        state.chart.options = initializeOptions({ display: true, position: 'bottom', text: '' }, false, { display: true, position: 'bottom' });
        state.chart.options.scales.yAxes[0].ticks.callback = function gbFormatter(value) { return (`${value.toString()}GB`); };
        state.chart.options.scales.yAxes[0].ticks.beginAtZero = false;
        state.chart.options.scales.xAxes[0].ticks.beginAtZero = false;
        const values = [];
        chart.values.forEach(value => {
          value.forEach(v => {
            values.push(v);
          });
        });
        state.chart.options.scales.yAxes[0].ticks.suggestedMin =
          minimum(values);
        state.chart.options.scales.yAxes[0].ticks.suggestedMax =
          maximum(values);
      }
      if (chart.others.chartType === 4) {
        state.chart.data.datasets = initializeDataSets(chart.legends.length, '', false);
        chart.props.forEach(prop => {
          // if (i === 0 || i === chart.props.length - 1) {
          state.chart.data.labels.push(prop);
          // }
        });
        chart.legends.forEach((legend, i) => {
          state.chart.data.datasets[i].label = legend;
          state.chart.data.datasets[i].backgroundColor = backgroundColors[i];
          state.chart.data.datasets[i].borderColor = backgroundColors[i];
          chart.values[i].forEach(value => {
            state.chart.data.datasets[i].data.push(value);
          });
        });
        state.chart.options = initializeOptions({ display: true, position: 'bottom', text: '' }, false, { display: true, position: 'bottom' });
        state.chart.options.scales.yAxes[0].ticks.callback = formatter();
      }
      state.chart.options.title.text = chart.others.metrics;
    }
  },
};

