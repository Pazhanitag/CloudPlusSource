import { minimum, maximum, formatter, backgroundColors, initializeChartData, initializeDataSets, initializeOptions, callbackLegend } from '../../../helpers/utils';

const labelObject = {
  Office365: '\ue902',
  Exchange: '\ue901',
  OneDrive: '\ue903',
  SharePoint: '\ue904',
  SkypeForBusiness: '\ue905',
  Yammer: '\ue907',
  Teams: '\ue906',
  default: '\ue900',
};

function cutomizeTooltipCallbacks(LegendsArray) {
  return {
    title: function title(tooltipItem) {
      return LegendsArray[tooltipItem[0].index];
    },
  };
}

export default {
  SET_SUBSCRIBED_SERVICES(state, data) {
    state.subscribedServices = data;
  },
  SET_CHARTS(state, chart) {
    state.chartNames = [];
    state.charts = [];
    chart.metrics.forEach((metric, i) => {
      state.chartNames.push(metric);
      const obj = {
        type: chart.chartType[i],
        data: initializeChartData(),
        options: {},
        others: {},
      };
      state.charts.push(obj);
    });
    state.charts.forEach((chartElement, index) => {
      chartElement.others = chart.graphData[index].others;
      if (chartElement.type === 1) {
        chartElement.data.datasets = initializeDataSets(chart.graphData[index].legends.length, '', false);
        chart.graphData[index].legends.forEach((legend, i) => {
          chartElement.data.datasets[i].label = legend;
          chartElement.data.datasets[i].backgroundColor = backgroundColors[i];
        });
        const values = [];
        chart.graphData[index].values[0].forEach((value, i) => {
          values.push(value);
          chartElement.data.datasets[i].data.push(value);
        });
        chartElement.options = initializeOptions({}, false, { display: false, position: 'top' });
        chartElement.options.scales.yAxes[0].ticks.callback = formatter();
        chartElement.options.scales.yAxes[0].ticks.beginAtZero = false;
        chartElement.options.scales.yAxes[0].ticks.suggestedMin = minimum(values);
        chartElement.options.scales.yAxes[0].ticks.suggestedMax = maximum(values);
        chartElement.options.legendCallback = callbackLegend(chart.graphData[index].legends);
      }
      if (chartElement.type === 2) {
        chartElement.data.datasets = initializeDataSets(1, [], false);
        chart.graphData[index].legends.forEach((legend, i) => {
          if (chart.graphData[index].values[0][i] !== 0) {
            if (labelObject[legend] !== undefined) {
              chartElement.data.labels.push(labelObject[legend]);
            } else {
              chartElement.data.labels.push(labelObject.default);
            }
          }
          chartElement.data.datasets[0].backgroundColor.push(backgroundColors[i]);
        });
        const values = [];
        chart.graphData[index].values[0].forEach(value => {
          chartElement.data.datasets[0].data.push(value);
          values.push(value);
        });
        chartElement.options = initializeOptions({}, false, { display: false, position: 'top' });
        chartElement.options.scales.yAxes[0].ticks.fontFamily = 'icomoon';
        chartElement.options.scales.yAxes[0].ticks.fontSize = 20;
        chartElement.options.scales.yAxes[0].ticks.fontColor = '#094ab1';
        chartElement.options.scales.xAxes[0].ticks.beginAtZero = false;
        chartElement.options.scales.xAxes[0].ticks.suggestedMin = minimum(values);
        chartElement.options.scales.xAxes[0].ticks.suggestedMax = maximum(values);
        chartElement.options.tooltips.callbacks =
          cutomizeTooltipCallbacks(chart.graphData[index].legends);
      }
      if (chartElement.type === 3) {
        chartElement.data.datasets = initializeDataSets(chart.graphData[index].legends.length, '', true);
        chart.graphData[index].props.forEach(prop => {
          chartElement.data.labels.push(prop);
        });
        chart.graphData[index].legends.forEach((legend, i) => {
          chartElement.data.datasets[i].label = legend;
          chartElement.data.datasets[i].backgroundColor = backgroundColors[i];
          chart.graphData[index].values[i].forEach(value => {
            chartElement.data.datasets[i].data.push(value);
          });
        });
        chartElement.options = initializeOptions({}, true, { display: false, position: 'top' });
        chartElement.options.elements = { point: { radius: 0 } };
        chartElement.options.scales.yAxes[0].ticks.beginAtZero = false;
        chartElement.options.scales.xAxes[0].ticks.beginAtZero = false;
        const values = [];
        chart.graphData[index].values.forEach(value => {
          value.forEach(v => {
            values.push(v);
          });
        });
        chartElement.options.scales.yAxes[0].ticks.suggestedMin =
          minimum(values);
        chartElement.options.scales.yAxes[0].ticks.suggestedMax =
          maximum(values);
        chartElement.options.legendCallback = callbackLegend(chart.graphData[index].legends);
      }
    });
  },
};

