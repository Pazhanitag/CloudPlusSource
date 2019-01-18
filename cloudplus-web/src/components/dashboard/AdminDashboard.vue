<template>
<div>
  <brand-tabs>
    <tabs @tabSelected="tabSelected">
      <tab-pane label="General Settings">
        <loading-icon size="4" v-if="isLoading"></loading-icon>
        <div class="panel select-panel" v-else>
        <div v-if="widgets.length > 0">
          <div class="panel-head">
            Microsoft Office 365
          </div>
          <div class="panel-body">
            <cloud-plus-check-box :value="selectAll.value" @input="getSelectAll($event)">
              {{selectAll.friendlyName}}
            </cloud-plus-check-box>
            <div v-for="(widget, index) in widgets" v-bind:key="index">
              <cloud-plus-check-box :value="widget.canAccess" @input="getCheckBoxResult($event, index)">
                {{widget.vendorMetricsName}}
              </cloud-plus-check-box>
            </div>
          </div>
          <div class="panel-foot">
            <brand-primary-btn @click="save(activeTab)">Save</brand-primary-btn>
            <brand-primary-btn @click="tabSelected(activeTab)">Cancel</brand-primary-btn>
            <brand-color-primary class="margin-small top help">
              *The above settings are applicable across all the companies
            </brand-color-primary>
          </div>
        </div>
        <div v-else>
          <span class="no-widgets-warning__title">no widgets available Right now !</span>
        </div>
        </div>
      </tab-pane>
      <tab-pane label="Company Settings">
        <loading-icon size="4" v-if="isLoading"></loading-icon>
        <div v-else>
          <div v-if="companies.length > 0 && showWidgets === false">
            <div class="btn-wrap">
              <brand-primary-btn @click="resetAll" class="top-right">Reset all to default</brand-primary-btn>
            </div>
            <div v-for="(company, index) in companies" :key="index" class="company-item" @click="getWidgets(company.id, index)">
              <h4>{{company.name}}</h4>
              <img :src="company.imageUrl" alt="">
            </div>
          </div>
          <div class="panel select-panel" v-if="showWidgets === true">
            <div v-if="activeTab === 1" class="company-item full-detail">
              <h4>{{companies[companyIndex].name}}</h4>
              <img :src="companies[companyIndex].imageUrl" alt="">
            </div>
            <div v-if="widgets.length > 0">
              <div class="panel-head">
                Microsoft Office 365
              </div>
              <div class="panel-body">
                <cloud-plus-check-box :value="selectAll.value" @input="getSelectAll($event)">
                  {{selectAll.friendlyName}}
                </cloud-plus-check-box>
                <div v-for="(widget, index) in widgets" v-bind:key="index">
                  <cloud-plus-check-box :value="widget.canAccess" @input="getCheckBoxResult($event, index)">
                    {{widget.vendorMetricsName}}
                  </cloud-plus-check-box>
                </div>
              </div>
              <div class="panel-foot">
                <brand-primary-btn @click="save(activeTab)">Save</brand-primary-btn>
                <brand-primary-btn @click="tabSelected(activeTab)">Cancel</brand-primary-btn>
              </div>
            </div>
            <div v-else>
              <span class="no-widgets-warning__title">no widgets are allowed to this company by the admin</span>
            </div>
          </div>
        </div>
      </tab-pane>
    </tabs>
  </brand-tabs>
</div>
</template>

<script>
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import dashboardService from '@/services/dashboardService';
import BrandButton from '@/components/shared/styled-components/brand-buttons/BrandButton';
import { Tabs, TabPane } from '@/components/shared/tabs';
import Config from 'appConfig';

export default {
  data() {
    return {
      companies: [],
      companyIndex: '',
      tabs: ['Company Settings', 'General Settings'],
      activeTab: '',
      widgets: [],
      showWidgets: false,
      selectAll: {
        friendlyName: 'Select All',
        value: true,
      },
    };
  },
  components: {
    BrandButton, Tabs, TabPane,
  },
  mixins: [toasterMixin, loadingMixin],
  methods: {
    resetAll() {
      this.isLoading = true;
      dashboardService.resetAlltoDefault().then(() => {
        this.isLoading = false;
        this.sucessToaster({
          text: 'All companies are reset to default successfully',
        });
        this.tabSelected(this.activeTab);
      });
    },
    getSelectAll(result) {
      this.selectAll.value = result;
      this.setServices(result);
    },
    setServices(value) {
      this.widgets.map(s => {
        s.canAccess = value;
        return s;
      });
    },
    setSelectAll(services) {
      const result = services.map(s => s.canAccess).every(canAccess => canAccess === true);
      if (this.selectAll.value !== result) {
        this.selectAll.value = result;
      }
    },
    clearResources() {
      this.companies = [];
      this.widgets = [];
      this.showWidgets = false;
    },
    tabSelected(index) {
      this.clearResources();
      this.activeTab = index;
      switch (index) {
        case 0:
          this.getWidgets();
          break;
        case 1:
          this.getAllCompanies();
          break;
        default:
          break;
      }
    },
    save(activeTab) {
      this.isLoading = true;
      if (activeTab === 1) {
        dashboardService.updateCompanyWidgets(this.widgets).then(() => {
          this.isLoading = false;
          this.sucessToaster({
            text: 'Company settings saved successfully',
          });
          this.tabSelected(activeTab);
        });
      } else if (activeTab === 0) {
        dashboardService.updateAllWidgets(this.widgets).then(() => {
          this.isLoading = false;
          this.sucessToaster({
            text: 'General settings saved successfully',
          });
          this.tabSelected(activeTab);
        });
      }
    },
    getCheckBoxResult(result, index) {
      this.widgets[index].canAccess = result;
      this.setSelectAll(this.widgets);
    },
    getAllCompanies() {
      this.isLoading = true;
      dashboardService.getAllCompanies().then(response => {
        this.isLoading = false;
        this.companies = response.data.result;
        this.companies.forEach(company => {
          company.imageUrl = company.logoUrl === '' ? '../../../static/images/default-logo.svg' : `${Config.companyLogoUrl}${company.logoUrl}`;
        });
      });
    },
    getWidgets(companyId, index) {
      this.companyIndex = index;
      this.showWidgets = false;
      this.isLoading = true;
      if (companyId !== undefined) {
        dashboardService.getCompanyWidgets(companyId).then(response => {
          this.widgets = response.data.result;
          this.showWidgets = true;
          this.isLoading = false;
          this.setSelectAll(this.widgets);
        });
      } else {
        dashboardService.getAllWidgets().then(response => {
          this.widgets = response.data.result;
          this.showWidgets = true;
          this.isLoading = false;
          this.setSelectAll(this.widgets);
        });
      }
    },
  },
  created() {
  },
  mounted() {
  },
};
</script>

<style lang="scss" scoped>
.company-item {
  width: calc(20% - 20px);
  float: left;
  padding: 0;
  min-height: 200px;
  background: #ffffff;
  cursor: pointer;
  -webkit-box-shadow:0px 0px 16px 0px rgba(0, 0, 0, 0.18);
  -moz-box-shadow:0px 0px 16px 0px rgba(0, 0, 0, 0.18);
  box-shadow:0px 0px 16px 0px rgba(0, 0, 0, 0.18);
  height: 100%;
  margin:10px;
      text-align: center;
  img {
    max-height: 150px;
    padding: 10px;
    display: inline-block;
  }
  h4{
    font-weight: 600;
    text-align: center;
    min-height: 30px;
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
    padding: 10px;
    background: #f2f2f2;
    text-transform: capitalize;
    -webkit-transition :0.3s ease all;
    -moz-transition :0.3s ease all;
    transition :0.3s ease all;
  }

  &.full-detail{
    width: calc(100% + 40px);
    margin: -20px -20px 20px -20px;
    box-shadow: none;
    cursor: default;
  }
}
.top-right{
  margin: 0px 10px;
}

.no-widgets-warning__title{
  color: #414f64;
  font-size: 1.25rem;
}
.btn-wrap{
  width: 100%;
  display: inline-block;
  text-align: right;
}
</style>

