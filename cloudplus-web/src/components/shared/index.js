import Vue from 'vue';

import CloudPlusTextfield from './input-components/CloudPlusTextfield';
import CloudPlusTextarea from './input-components/CloudPlusTextarea';
import CloudPlusSelect from './input-components/CloudPlusSelect';
import CloudPlusRadioButton from './input-components/CloudPlusRadioButton';
import CloudPlusCheckBox from './input-components/CloudPlusCheckBox';
import CloudPlusTooltip from './input-components/CloudPlusToolitp';
import LoadingIcon from './misc/LoadingIcon';
import BarChart from './charts/bar-chart';
import HorizontalBarChart from './charts/horizontal-bar-chart';
import LineChart from './charts/line-chart';
import ReactiveLineChart from './charts/reactive-line-chart';
import ReactiveBarChart from './charts/reactive-bar-chart';

export const textfield = Vue.component('cloud-plus-textfield', CloudPlusTextfield);
export const textarea = Vue.component('cloud-plus-textarea', CloudPlusTextarea);
export const select = Vue.component('cloud-plus-select', CloudPlusSelect);
export const radioBtn = Vue.component('cloud-plus-radio-btn', CloudPlusRadioButton);
export const checkBox = Vue.component('cloud-plus-check-box', CloudPlusCheckBox);
export const loadingIcon = Vue.component('loading-icon', LoadingIcon);
export const tooltip = Vue.component('cloud-plus-tooltip', CloudPlusTooltip);
export const barChart = Vue.component('bar-chart', BarChart);
export const horizontalBarChart = Vue.component('horizontal-bar-chart', HorizontalBarChart);
export const lineChart = Vue.component('line-chart', LineChart);
export const reactiveLineChart = Vue.component('reactive-line-chart', ReactiveLineChart);
export const reactiveBarChart = Vue.component('reactive-bar-chart', ReactiveBarChart);
