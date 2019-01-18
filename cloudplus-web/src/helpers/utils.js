// Remove noise from FileReader base64 url string
export const parseUrlImageDataToBase64String = urlImageData => { // eslint-disable-line
  if (!urlImageData || urlImageData === '') {
    return null;
  }
  const splittedImageData = urlImageData.split('base64,');

  if (splittedImageData.length < 2) {
    return null;
  }

  return splittedImageData[1];
};

export const sortCompare = (left, right, ord) => {
  if (left === undefined || right === undefined) {
    return 0;
  }
  let leftCompare;
  let rightcompare;

  if (typeof (left) === 'string' && typeof (right) === 'string') {
    leftCompare = left.toLowerCase();
    rightcompare = right.toLowerCase();
  } else if (typeof (left) === 'number' && typeof (right) === 'number') {
    leftCompare = left;
    rightcompare = right;
  }

  if (leftCompare < rightcompare) {
    return ord === 'asc' ? -1 : 1;
  } else if (leftCompare > rightcompare) {
    return ord === 'asc' ? 1 : -1;
  }
  return 0;
};

export const userHasPermissions = (firstArray, secondArray) => {
  if (secondArray.length === 0) {
    return true;
  }
  return secondArray.some(v => firstArray.some(x => x.name === v));
};

export const areStringArraysEqual = (firstArray, secondArray) => {
  if (firstArray.length === secondArray.length) {
    const intersection = firstArray.filter(element => secondArray.includes(element));
    return intersection.length === secondArray.length;
  }
  return false;
};

export const convertArrayOfObjectsToCSV = args => {
  let ctr;
  const data = args.data || null;
  if (data == null || !data.length) {
    return null;
  }
  const columnDelimiter = args.columnDelimiter || ',';
  const lineDelimiter = args.lineDelimiter || '\n';
  const keys = Object.keys(data[0]);
  let result = '';
  result += keys.join(columnDelimiter);
  result += lineDelimiter;
  data.forEach(item => {
    ctr = 0;
    keys.forEach(key => {
      if (ctr > 0) result += columnDelimiter;
      result += item[key];
      ctr += 1;
    });
    result += lineDelimiter;
  });
  return result;
};

export const formatPrice = price => {
  const floatPrice = parseFloat(price);
  if (!Number.isNaN(floatPrice)) {
    return floatPrice.toFixed(2);
  }
  return price;
};

export const isEmptyObject = obj => Object.keys(obj).length === 0 && obj.constructor === Object;

export const backgroundColors = ['#e53935', '#EF6C00', '#7B1FA2', '#4CAF50', '#2196F3', '#FFEB3B', '#795548', '#9E9E9E', '#607D8B', '#009688'];

export const minimum = values => {
  let min = Math.min(...values);
  min -= (10 / 100) * min;
  return Math.round(min);
};

export const maximum = values => {
  let max = Math.max(...values);
  max += (10 / 100) * max;
  return Math.round(max);
};

export const formatter = () => function kFormatter(value) { let result; if (value > 999) { result = value % 1000 === 0 ? `${(value / 1000).toFixed(0)}k` : `${(value / 1000).toFixed(1)}k`; } else { result = value; } return result; };

export const initializeChartData = () => {
  const data = {
    labels: [],
    datasets: [],
  };
  return data;
};

export const initializeDataSets = (max, isBackgroundArray, fill) => {
  const obj = [];
  for (let i = 0; i < max; i += 1) {
    const clone = {
      label: '',
      backgroundColor: isBackgroundArray,
      pointBackgroundColor: 'white',
      borderWidth: 1,
      pointBorderColor: '#249EBF',
      data: [],
      fill,
      borderColor: '',
      lineTension: 0,
    };
    obj.push(clone);
  }
  return obj;
};

export const initializeOptions = (title, isElementEnabled = false, legend) => {
  const options = {
    scales: {
      yAxes: [{
        display: true,
        ticks: {
          beginAtZero: true,
        },
        gridLines: {
          display: false,
        },
      }],
      xAxes: [{
        barPercentage: 0.5,
        display: true,
        ticks: {
          beginAtZero: true,
        },
        gridLines: {
          display: false,
        },
      }],
    },
    legend: {
      display: legend.display,
      position: legend.position,
      onClick: e => e.preventDefault(),
      labels: {
        usePointStyle: false,
        fontSize: 10,
        boxWidth: 10,
      },
    },
    tooltips: {
      enabled: true,
    },
    responsive: true,
    maintainAspectRatio: false,
  };
  if (isElementEnabled) {
    options.elements = { point: { radius: 0 } };
  }
  if (title) {
    options.title = title;
  }
  return options;
};

export const callbackLegend = labels => {
  const text = [];
  let labelPercentage = '33%';
  if (labels.length === 1) {
    labelPercentage = '100%';
  } else if (labels.length === 2) {
    labelPercentage = '50%';
  }
  return function legendCallback() {
    text.push(`<style>
    .legends-list{
      display: -webkit-box;
      display: -ms-flexbox;
      display: flex;
      -ms-flex-wrap: wrap;
      flex-wrap: wrap;
      margin-bottom: 10px;
      position:relative;
    }
    span.tooltip {
      position: absolute;
      font-size: 13px;
      top: auto;
      bottom: auto;
      left: auto;
      background: #000;
      color:#ffffff;
      border: 1px solid #ccc;
      border-radius: 2px;
      line-height: normal;
      padding: 5px;
      box-shadow: 0px 0px 6px 2px #ccc6;
      -webkit-transition: 0.3s ease all;
      -moz-transition: 0.3s ease all;
      transition: 0.3s ease all;
      opacity: 0;
      visibility: hidden;
      display: block;
      max-width: 100%;
      white-space: normal;
      word-break: break-all;
      -webkit-transform: translateY(-50px);
      -moz-transform: translateY(-50px);
      transform: translateY(-50px);

  }

  .legends-list li {
      position: static;
  }
  .legends-list li:nth-child(3n):hover .tooltip{
    /* right:0; */
  }
  .legends-list li:nth-child(3n) .tooltip{
    /* right:0; */
  }

  .legends-list li span:nth-child(2):hover + .tooltip {
      top: auto;
      bottom: auto;
      opacity: 1;
      visibility: visible;
      -webkit-transform: translateY(-35px);
      -moz-transform: translateY(-35px);
      transform: translateY(-35px);
  }
    </style><ul class="legends-list">`);
    labels.forEach((label, i) => {
      if (labels.length > 3 && i >= 3) {
        if (labels.length % 3 === 1) {
          labelPercentage = '100%';
        } else {
          labelPercentage = '33%';
        }
      }
      text.push(`<li style="float: left; margin-bottom:6px; margin-right: 5px; width: calc(${labelPercentage} - 5px);" >
      <span
      style='width:10px;
        height:10px;
        float:left;
        background-color:${backgroundColors[i]};
        vertical-align: middle;
        margin-right: 5px;
        position: relative;
        top: 3px;
      '>
      </span>
      <span
      style="max-width: calc(100% - 15px);
        float:left;
        line-height:normal;
        white-space: nowrap;
        overflow: hidden;
        cursor: pointer;
        text-overflow: ellipsis;
        font-size:11px;">`);
      text.push(label);
      text.push(`</span>
      <span class="tooltip">${label}</span>
      </li>`);
    });
    text.push('</ul>');
    return text.join('');
  };
};
