<template>
  <div>
    <brand-tabs>
      <tabs @tabSelected="tabSelected">
        <tab-pane label="Dashboard Settings">
          <loading-icon size="4" v-if="isLoading"></loading-icon>
          <div v-else>
            <div class="panel select-panel" v-if="services.length > 0">
              <div class="panel-head">
                Microsoft Office 365
              </div>
              <div class="panel-body">
                <cloud-plus-check-box :value="selectAll.value" @input="getSelectAll($event)">
                  {{selectAll.friendlyName}}
                </cloud-plus-check-box>
                <cloud-plus-check-box :value="service.canAccess" @input="getServicesResult($event, index)" v-for="(service, index) in services" v-bind:key="index">
                  {{service.vendorMetricsName}}
                </cloud-plus-check-box>
              </div>
              <div class="panel-foot">
                <brand-primary-btn @click="save">Save</brand-primary-btn >
                <brand-primary-btn @click="redirect('dashboard')">Cancel</brand-primary-btn >
              </div>
            </div>
          </div>
        </tab-pane>
        <tab-pane label="Report Schedule">
          <loading-icon size="4" v-if="isLoading"></loading-icon>
          <div v-else>
            <div v-if="toggleReport">
              <div class="columns">
                <div class="column">
                  <div class="title-container" v-if="reports.length !== 0">
                    <brand-color-primary>
                      <span class="header-title has-text-weight-semibold">My Scheduled Reports</span>
                    </brand-color-primary>
                  </div>
                  <div class="is-pulled-right">
                    <brand-primary-btn @click="createNewReport" v-if="toggleReport">Schedule New Report</brand-primary-btn>
                  </div>
                </div>
              </div>
              <div v-if="reports.length === 0" class="no-reports-warning">
                <div class="no-reports-warning__title">Oops! Seems like you don&#39;t have any Schedule Reports!</div>
                <brand-color-primary><span class="link" @click="createNewReport">Schedule New Report</span></brand-color-primary>
              </div>
              <div v-else>
                <table class="table">
                  <thead>
                    <tr>
                      <th>Name</th>
                      <th>Type</th>
                      <th>Report Period</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(report, index) in reports" v-bind:key="index">
                      <td>{{report.reportName}}</td>
                      <td>{{schedulePeriodsclone[index].friendlyName}}</td>
                      <td>{{report.reportPeriod}} Days</td>
                      <td class="table-icons">
                        <a @click="editReport(report.id)"><brand-fa :class="['fa', 'fa-pencil']"></brand-fa></a>
                        <a @click="deleteReport(report)"><brand-fa :class="['fa', 'fa-trash']"></brand-fa></a>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
            <div class="scheduler-inner-block" v-if="!toggleReport">
              <cloud-plus-textfield :placeholder="'Report Name'" v-model="reportName" data-vv-name="reportName" name="reportName" v-validate="'required|max:40'" data-vv-as="Report Name" :validationErrors="errors.first('reportName')">
                Report Name
              </cloud-plus-textfield>
              <label class="label">Widgets</label>
              <div class="panel select-panel">
                <div class="panel-head">
                  Microsoft Office 365
                </div>
                <div class="panel-body">
                    <cloud-plus-check-box :value="selectAll.value" @input="getReportSelectAll($event)">
                    {{selectAll.friendlyName}}
                  </cloud-plus-check-box>
                  <cloud-plus-check-box :value="reportService.canAccess" @input="getReportServiceResult($event, index)" v-for="(reportService, index) in reportServices" v-bind:key="index">
                    {{reportService.vendorMetricsName}}
                  </cloud-plus-check-box>
                  <p class="help is-danger" v-show="widgetsErrorMessage">{{widgetsErrorMessage}}</p>
                </div>
              </div>
              <cloud-plus-select @input="onReportPeriodSelect($event)" :options="reportPeroids.map(r => ({name:r.friendlyName,value:r.value}))" :selected="selectedReportPeriod">
                Report Period
              </cloud-plus-select>
              <cloud-plus-select @input="onSchedulePeriodSelect($event)" :options="schedulePeriods.map(s => ({name:s.friendlyName,value:s.value}))" :selected="selectedSchedulePeriod">
                Schedule Period
              </cloud-plus-select>
              <div v-if="selectedSchedulePeriod == 1">
                <cloud-plus-textfield v-model="dailyoccurrence" :validationErrors='dailyErrorMessage'>
                Report Timeframe (days)  <brand-color-primary class="help" v-if="DailyNote">{{DailyNote}}</brand-color-primary>
                </cloud-plus-textfield>
              </div>
              <div v-if="selectedSchedulePeriod == 2">
                <cloud-plus-select @input="onWeeklyOccurrenceSelect($event)" :options="weeklyoccurrence.map(w => ({name:w.friendlyName,value:w.value}))" :selected="selectedWeeklyOccurrence">
                  Delivery Day
                </cloud-plus-select>
              </div>
              <div v-if="selectedSchedulePeriod == 3">
                <cloud-plus-select @input="onMonthlyoccurrenceSelect($event)" :options="monthlyoccurrence" :selected="selectedMonthlyOccurrence">
                  Delivery Date
                </cloud-plus-select>
              </div>
              <div :class="['custom-email-field', {'active' : emailErrorMessage !== ''}]">
                <cloud-plus-textfield :placeholder="'Emails'" v-model="email" :validationErrors='emailErrorMessage'>
                  Email Address
                  <div class="custom-tag-col" v-if="emailList.length > 0">
                  <brand-background-with-text-color v-for="(localEmail, index) in emailList" :key="index">{{localEmail}} <a @click="removeEmail(index)"><brand-fa :class="['fa', 'fa-times']"></brand-fa></a></brand-background-with-text-color>
                  </div>
                </cloud-plus-textfield>
                <div class="custom-plus" @click="updateEmailList(email.toLowerCase())">
                  <brand-fa :class="['fa', 'fa-plus-circle', 'fa-lg']"></brand-fa>
                </div>
              </div>
              <brand-primary-btn @click="schedulerSave">Save</brand-primary-btn>
              <brand-primary-btn @click="setReports">Cancel</brand-primary-btn>
            </div>
          </div>
        </tab-pane>
      </tabs>
    </brand-tabs>
  </div>
</template>
<script>
import { mapActions, mapGetters } from 'vuex';
import BrandButton from '@/components/shared/styled-components/brand-buttons/BrandButton';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import { Tabs, TabPane } from '@/components/shared/tabs';
import dashboardService from '@/services/dashboardService';

export default {
  inject: {
    $validator: '$validator',
  },
  data() {
    return {
      reports: [],
      toggleReport: true,
      services: [],
      reportServices: [],
      reportPeroids: [
        {
          friendlyName: '7 Days',
          canAccess: false,
          value: 7,
        },
        {
          friendlyName: '30 Days',
          canAccess: false,
          value: 30,
        },
        {
          friendlyName: '90 Days',
          canAccess: false,
          value: 90,
        },
        {
          friendlyName: '180 Days',
          canAccess: false,
          value: 180,
        },
      ],
      schedulePeriodsclone: [],
      schedulePeriods: [
        {
          friendlyName: 'Daily',
          value: 1,
        },
        {
          friendlyName: 'Weekly',
          value: 2,
        },
        {
          friendlyName: 'Monthly',
          value: 3,
        },
      ],
      selectedReportPeriod: null,
      selectedSchedulePeriod: null,
      emailList: [],
      email: '',
      emailErrorMessage: '',
      reportName: '',
      dailyoccurrence: null,
      weeklyoccurrence: [
        {
          friendlyName: 'Monday',
          value: 1,
        },
        {
          friendlyName: 'Tuesday',
          value: 2,
        },
        {
          friendlyName: 'Wednesday',
          value: 3,
        },
        {
          friendlyName: 'Thursday',
          value: 4,
        },
        {
          friendlyName: 'Friday',
          value: 5,
        },
        {
          friendlyName: 'Saturday',
          value: 6,
        },
        {
          friendlyName: 'Sunday',
          value: 7,
        },
      ],
      selectedWeeklyOccurrence: null,
      monthlyoccurrence: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28],
      selectedMonthlyOccurrence: null,
      dailyErrorMessage: '',
      widgetsErrorMessage: '',
      servicesClone: [],
      id: 0,
      widgetIds: [],
      isDeleted: false,
      DailyNote: '',
      selectAll: {
        friendlyName: 'Select All',
        value: true,
      },
    };
  },
  mixins: [toasterMixin, loadingMixin],
  components: {
    BrandButton, ComponentStickyHeader, Tabs, TabPane,
  },
  computed: {
    ...mapGetters({
      userProfile: 'userAuth/userProfile',
      subscribedServices: 'dashboard/subscribedServices',
    }),
  },
  watch: {
    // eslint-disable-next-line
    dailyoccurrence: function () {
      const { errorMessage } = this.checkNumberErrors(this.dailyoccurrence);
      this.dailyErrorMessage = errorMessage !== '' ? `The Report Timeframe (days) ${errorMessage}` : '';
      if (this.dailyErrorMessage === '') {
        this.DailyNote = Number(this.dailyoccurrence) === 1 ? '* The report will be generated for every day' : `* The report will be generated for every ${this.dailyoccurrence} days`;
      } else {
        this.DailyNote = '';
      }
    },
    // eslint-disable-next-line
    email: function() {
      this.emailErrorMessage = this.email === '' ? '' : this.emailErrorMessage;
    },
  },
  methods: {
    ...mapActions({
      getSubscribedServices: 'dashboard/getSubscribedServices',
      saveSubscribedServices: 'dashboard/saveSubscribedServices',
    }),
    getSelectAll(result) {
      this.selectAll.value = result;
      this.setServices(result);
    },
    getReportSelectAll(result) {
      this.selectAll.value = result;
      this.setReportServices(result);
    },
    setServices(value) {
      this.services.map(s => {
        s.canAccess = value;
        return s;
      });
    },
    setReportServices(value) {
      this.reportServices.map(rs => {
        rs.canAccess = value;
        return rs;
      });
    },
    setSelectAll(services) {
      const result = services.map(s => s.canAccess).every(canAccess => canAccess === true);
      if (this.selectAll.value !== result) {
        this.selectAll.value = result;
      }
    },
    tabSelected(index) {
      if (index === 0) {
        const { companyId } = this.userProfile;
        const userId = this.userProfile.id;
        this.isLoading = true;
        this.getSubscribedServices({ companyId, userId }).then(() => {
          this.isLoading = false;
          this.services = JSON.parse(JSON.stringify(this.subscribedServices));
          this.reportServices = JSON.parse(JSON.stringify(this.subscribedServices));
          this.servicesClone = JSON.parse(JSON.stringify(this.subscribedServices));
          this.setSelectAll(this.services);
        });
        this.setDefaultValues();
      }
      if (index === 1) {
        this.setReports();
      }
    },
    checkNumberErrors(stringNumber) {
      let result = false;
      let errorMessage = '';
      if (stringNumber === null || stringNumber === '') {
        errorMessage = 'field is required';
      } else if (Number.isNaN(Number(stringNumber))) {
        errorMessage = 'field should be numberic';
      } else if (stringNumber <= 0) {
        errorMessage = 'field should be a positive number';
      } else if (stringNumber.toString().indexOf('.') !== -1) {
        errorMessage = 'field should be a whole number';
      }
      if (errorMessage !== '') {
        result = true;
      }
      return { result, errorMessage };
    },
    clearReportResources() {
      this.reportName = '';
      this.reportServices = [];
      this.widgetsErrorMessage = '';
      this.dailyErrorMessage = '';
      this.selectedWeeklyOccurrence = null;
      this.emailList = [];
      this.email = '';
      this.emailErrorMessage = '';
      this.reportServices = JSON.parse(JSON.stringify(this.servicesClone));
      this.setDefaultValues();
    },
    splitString(stringToSplit, separator = ',') {
      return stringToSplit.split(separator);
    },
    joinArray(arrayToJoin, separator = ',') {
      return arrayToJoin.join(separator);
    },
    mapReportServices(report) {
      const result = report;
      let toggle = false;
      this.widgetIds.forEach(id => {
        if (id === report.vendorMetricsId) {
          toggle = true;
        }
      });
      if (toggle) {
        result.canAccess = true;
      } else {
        result.canAccess = false;
      }
      return result;
    },
    setReportResources(report) {
      this.clearReportResources();
      this.reportName = report.reportName;
      const splittedStrings = this.splitString(report.widgets);
      this.widgetIds = [];
      splittedStrings.forEach(currentstring => this.widgetIds.push(+currentstring));
      this.reportServices = this.reportServices.map(r => this.mapReportServices(r));
      this.selectedReportPeriod = report.reportPeriod;
      this.selectedSchedulePeriod = report.reportFrequency;
      switch (this.selectedSchedulePeriod) {
        case 1:
          this.dailyoccurrence = report.dayFrequency.toString();
          break;
        case 2:
          [this.selectedWeeklyOccurrence] =
            this.weeklyoccurrence.filter(day => day.value === report.weekFrequency)
              .map(w => w.value);
          break;
        case 3:
          this.selectedMonthlyOccurrence = report.monthFrequency;
          break;
        default:
          break;
      }
      this.emailList = this.splitString(report.emailList);
      this.id = report.id;
      this.isDeleted = report.isDeleted;
    },
    createNewReport() {
      this.clearReportResources();
      this.toggleReport = false;
      this.setSelectAll(this.reportServices);
    },
    onWeeklyOccurrenceSelect(result) {
      [this.selectedWeeklyOccurrence] =
        this.weeklyoccurrence.filter(w => w.value === +result).map(w => w.value);
    },
    validateEmail(email) {
      // eslint-disable-next-line
      const regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return regex.test(email);
    },
    updateEmailList(email) {
      if (this.validateEmail(email)) {
        if (!this.emailList.includes(email)) {
          this.emailList.push(email);
          this.emailErrorMessage = '';
          this.email = '';
        } else {
          this.emailErrorMessage = 'Email Address already exists in the list';
        }
      } else {
        this.emailErrorMessage = this.email === '' ? 'The Email Address field is required' : 'Invalid Email Address';
      }
    },
    async setReports() {
      const response = await this.getReports(this.userProfile.id);
      this.reports = response.data.result;
      this.schedulePeriodsclone = [];
      this.reports.forEach((rs, index) => {
        [this.schedulePeriodsclone[index]] =
         this.schedulePeriods.filter(sp => sp.value === rs.reportFrequency);
      });
      this.isLoading = false;
      this.toggleReport = true;
    },
    removeEmail(index) {
      this.emailList.splice(index, 1);
    },
    getServicesResult(result, index) {
      this.services[index].canAccess = result;
      this.setSelectAll(this.services);
    },
    getReportServiceResult(result, index) {
      this.reportServices[index].canAccess = result;
      this.setSelectAll(this.reportServices);
    },
    onMonthlyoccurrenceSelect(result) {
      this.selectedMonthlyOccurrence = result;
    },
    onReportPeriodSelect(result) {
      this.selectedReportPeriod = +result;
    },
    onSchedulePeriodSelect(result) {
      this.selectedSchedulePeriod = +result;
    },
    isSchedulePeroidEmpty() {
      let result = false;
      let errorMessage = '';
      if (this.selectedSchedulePeriod === 1) {
        ({ result, errorMessage } = this.checkNumberErrors(this.dailyoccurrence));
        this.dailyErrorMessage = errorMessage !== '' ? `The Report Timeframe (days) ${errorMessage}` : '';
      }
      return result;
    },
    getReports(userId) {
      this.isLoading = true;
      return dashboardService.getReports(userId);
    },
    getReport(id) {
      this.isLoading = true;
      return dashboardService.getReport(id);
    },
    deleteReportbyPayload(payload) {
      this.isLoading = true;
      return dashboardService.saveReport(payload);
    },
    saveReport(payload) {
      this.isLoading = true;
      return dashboardService.saveReport(payload);
    },
    async editReport(id) {
      const response = await this.getReport(id);
      this.isLoading = false;
      this.setReportResources(response.data.result[0]);
      this.toggleReport = false;
    },
    async deleteReport(report) {
      report.isDeleted = true;
      await this.deleteReportbyPayload(report);
      this.sucessToaster({
        text: 'Scheduled report deleted successfully',
      });
      this.isLoading = false;
      this.setReports();
    },
    async saveAndReload(payload) {
      await this.saveReport(payload);
      this.isLoading = false;
      this.sucessToaster({
        text: 'Scheduler settings saved successfully',
      });
      this.setReports();
    },
    async schedulerSave() {
      const widgets = this.reportServices.filter(service => service.canAccess === true)
        .map(service => service.vendorMetricsId);
      this.widgetsErrorMessage = widgets.length === 0 ? 'The Widget field is required' : '';
      this.emailErrorMessage = this.emailList.length === 0 ? 'The Email Address field is required' : '';
      this.$validator.validateAll().then(result => {
        if (result) {
          if (!this.isSchedulePeroidEmpty() && widgets.length > 0 && this.emailList.length > 0) {
            this.isLoading = true;
            const payload = {
              userId: +this.userProfile.id,
              companyId: +this.userProfile.companyId,
              reportName: this.reportName,
              widgets: this.joinArray(widgets),
              reportPeriod: this.selectedReportPeriod,
              reportFrequency: this.selectedSchedulePeriod,
              dayFrequency: this.selectedSchedulePeriod === 1 ? +this.dailyoccurrence : null,
              weekFrequency: this.selectedSchedulePeriod ===
                2 ? this.selectedWeeklyOccurrence : null,
              monthFrequency:
              this.selectedSchedulePeriod === 3 ? +this.selectedMonthlyOccurrence : null,
              emailList: this.joinArray(this.emailList),
              id: this.id,
              isDeleted: this.isDeleted,
            };
            this.saveAndReload(payload);
          } else {
            this.validationErrorToaster();
          }
        } else {
          this.validationErrorToaster();
        }
      });
    },
    save() {
      this.isLoading = true;
      this.saveSubscribedServices(this.services)
        .then(() => {
          this.isLoading = false;
          this.sucessToaster({
            text: 'Dashboard settings saved successfully',
          });
          this.redirect('dashboard');
        });
    },
    redirect(name) {
      this.$router.push(name);
    },
    setDefaultValues() {
      this.selectedReportPeriod = 7;
      this.selectedSchedulePeriod = 1;
      this.dailyoccurrence = '1';
      this.selectedMonthlyOccurrence = 1;
      this.selectedWeeklyOccurrence =
        this.selectedWeeklyOccurrence ===
          null ? this.weeklyoccurrence[4].value : this.selectedWeeklyOccurrence;
    },
  },
  mounted() {
  },
};
</script>
<style lang="scss" scoped>

.scheduler-inner-block{
  margin: 0 20px;
  width: 100%;
  max-width: 320px;
  .panel{
    margin: 0;
    width: 100%;
    max-width: 250px;
     margin-bottom: 10px;
    .level{
      margin-bottom: 5px;
    }

  }
  .control {
    max-width: 200px;
  }
  .custom-email-field{
        position: relative;
    width: 100%;
    float: none;
    max-width: 250px;
    display: inline-block;
    clear: right;
    margin-bottom: 10px;
    &.active {
      .custom-plus{
        bottom: 35px;
      }
    }

    .custom-plus{
    width: 10%;
    padding-left: 25px;
    position: absolute;
    bottom: 20px;
    right: -5px;
    }
    .custom-tag-col{
      div{
        padding: 3px 5px;
        font-weight: normal;
        display: inline-block;
        margin-right: 10px;
        margin-bottom: 5px;
        a{
          i{
            color:#fff
          }
        }
      }
    }
    .custom-error-msg{
      clear: both;
    }
  }
}
.custom-email-field .custom-tag-col {
    max-height: 100px;
    overflow-y: auto;
    padding: 10px 0;
}
.scheduler-inner-block .field {
    max-width: 250px;
}
.no-reports-warning{
    width: 100%;
    padding-top: 18rem;
    text-align: center;
    .no-reports-warning__title{
      color: #414f64;
      font-size: 1.25rem;
    }
}
.no-reports-warning{
  &__title {
    color: $label-color;
    font-size: $big-font-size;
  }
  &__subtitle {
    color: $subtitle-color;
    font-size: $primary-font-size;
  }
  .link{
  cursor: pointer;
  }
}
.title-container {
  float: left;
  display: inline;
  margin: 0.4rem 0.93rem;
}
.header-title {
  font-size: 1.1rem;
}
.columns {
  padding: 1rem 1.5rem 1rem 1.2rem;
  padding-top: 0px;
  margin: 0;
  position: relative;
  .title-container{
    position: absolute;
    content: '';
    left: 50%;
    -webkit-transform: translateX(-50%);
    -moz-transform: translateX(-50%);
    transform: translateX(-50%);
  }
}
.table thead td, .table thead th, .table td{
  padding: 10px 15px;
}

.table{
   width: 650px;
  margin: 0 auto;
  tbody{
    tr{
      td{
        &:last-child,&:nth-child(3),&:nth-child(2){
              width: 20%;
        }
        &:first-child{
          text-overflow: ellipsis;
          max-width: 260px;
          overflow: hidden;
          white-space: nowrap;
        }
      }
    }
  }
  thead{
    tr{
      th{
        &:last-child,&:nth-child(3),&:nth-child(2){
              width: 20%;
        }
      }
    }
  }
}
.table td.table-icons a{
  margin: 0px 20px 0px 0px;
}
</style>
