<template>
  <div>
    <loading-icon size="2" v-if="isLoading"></loading-icon>
    <div class="user-service-wrapper" v-else>
    <table v-if="service" class="table">
      <tbody>
        <tr>
          <td><img style="height:30px;" :src="service.imgUrl"></td>
          <td>{{service.name}}</td>
          <td>{{service.assignedLicense}}</td>
          <td class="status-tag-td">
            <span :class="serviceStatusClass[service.status]">{{service.statusToDisplay}}</span>
          </td>
          <td v-if="service.status !== serviceStatus.inProgress && service.status !== serviceStatus.notAvailable">
            <popper trigger="click" :options="{placement: 'bottom'}">
              <div class="popper">
                <div class="dropdown-content">
                  <brand-hover v-if="service.status === serviceStatus.available || service.status === serviceStatus.assigned">
                    <span @click="openProductAssignmentModal()" class="dropdown-item">
                      Configure Service
                    </span>
                  </brand-hover>
                  <brand-hover v-if="service.status === serviceStatus.removed">
                    <span @click="openRestoreModal()" class="dropdown-item">
                      Restore service
                    </span>
                  </brand-hover>
                  <brand-hover v-if="service.status === serviceStatus.assigned">
                    <span @click="openRemoveModal()" class="dropdown-item">
                      Remove service
                    </span>
                  </brand-hover>
                </div>
              </div>
              <a slot="reference" class="card-header-icon" aria-label="more options">
                <i  class="fa fa-cog" aria-hidden="true"></i>
                <i class="fa fa-caret-down" aria-hidden="true"></i>
              </a>
            </popper>
          </td>
        </tr>
      </tbody>
    </table>
    <div v-else class="service-status-message">
      You have no enabled services for assignment. Please choose desired products and services from Product catalog
    </div>
    <div class="service-status-message" v-if="service && service.status === serviceStatus.notAvailable">
      The user domain might not be verified or added. Please add and verify domain in order to assign services.
    </div>
    <div class="service-status-message" v-if="service && service.status === serviceStatus.inProgress">
      Request in progress, please wait
    </div>
    <confirmation-modal
      v-if="showRestoreModal" :showModal="showRestoreModal"
      @cancel="closeRestoreModal"
      @confirm = "restoreService"
      confirmText="Restore">Are you sure you want to restore the user's licences?
    </confirmation-modal>
    <confirmation-modal
      v-if="showRemoveModal" :showModal="showRemoveModal"
      @cancel="closeRemoveModal"
      @confirm = "removeService"
      confirmText="Remove">Are you sure you want to remove the user's licences?
    </confirmation-modal>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations, mapActions } from 'vuex';
import { userServiceStatus } from '@/assets/constants/commonConstants';
import Popper from '@/components/shared/popper/Popper';
import ConfirmationModal from '@/components/shared/modals/ConfirmationModal';
import loadingMixin from '@/mixins/loading';

export default {
  components: {
    Popper,
    ConfirmationModal,
  },
  mixins: [loadingMixin],
  data() {
    return {
      showRestoreModal: false,
      showRemoveModal: false,
      serviceStatus: {
        notAvailable: userServiceStatus.NotAvailable,
        available: userServiceStatus.Available,
        inProgress: userServiceStatus.InProgress,
        assigned: userServiceStatus.Assigned,
        removed: userServiceStatus.Removed,
      },
      serviceStatusClass: [
        'tag is-dark',
        'tag is-info',
        'tag is-warning',
        'tag is-danger',
        'tag is-primary',
      ],
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/basicUserInfo',
      service: 'user/userServices',
    }),
  },
  methods: {
    ...mapMutations({
      setUserServiceStatusToInProgress: 'user/SET_USER_SERVICE_STATUS_TO_IN_PROGRESS',
      setUserServicesTimeout: 'user/SET_USER_SERVICES_TIMEOUT',
    }),
    ...mapActions({
      getUserServices: 'user/getUserServices',
      restoreUserService: 'office365/restoreUserService',
      removeUserService: 'office365/removeUserService',
    }),
    openProductAssignmentModal() {
      this.$emit('openModal');
    },
    restoreService() {
      this.restoreUserService().then(() => {
        this.setUserServiceStatusToInProgress();
        this.closeRestoreModal();
      });
    },
    openRestoreModal() {
      this.showRestoreModal = true;
    },
    closeRestoreModal() {
      this.showRestoreModal = false;
    },
    removeService() {
      this.removeUserService().then(() => {
        this.setUserServiceStatusToInProgress();
        this.closeRemoveModal();
      });
    },
    openRemoveModal() {
      this.showRemoveModal = true;
    },
    closeRemoveModal() {
      this.showRemoveModal = false;
    },
    loadUserServices() {
      const company = this.user.companyId;
      const username = this.user.emailAddress;
      this.getUserServices({
        username,
        company,
      }).then(() => {
        this.isLoading = false;
        if (this.service) {
          this.$emit('serviceStatusChanged', this.service.status);
        }
        this.refreshUserServices();
      });
    },
    refreshUserServices() {
      const timeout = setTimeout(() => {
        this.loadUserServices();
      }, 10000);
      this.setUserServicesTimeout(timeout);
    },
  },
  mounted() {
    this.loadUserServices();
  },
};
</script>

<style lang="scss" scoped>
  .user-service-wrapper{
    padding: 0 2rem;
  }
  table {
    width: 100%;
    border-bottom: 1px solid #f3f3f3;
  }
  .table td {
    padding-bottom:1.2rem;
  }
  .fa-cog {
    font-size: 1.5rem;
    color: $icon-color;
    margin-right: 0.2rem;
    cursor: pointer;
  }
  .service-status-message {
    text-align: center;
    font-size: 0.875rem;
    font-weight: normal;
    margin: 4rem 0;
  }
  .status-tag-td {
    align-items: center;
  }
</style>
