<template>
  <div>
    <loading-icon size="2" v-if="isLoading"></loading-icon>
    <table class="table is-fullwidth" v-else>
      <thead>
        <th class="catalog-name">Schedule name</th>
        <th class="valid-from">Valid from</th>
        <th class="valit-to">Valid to</th>
        <th class="accounts">Accounts</th>
        <th class="status">Status</th>
        <th class="download">Download</th>
        <th></th>
      </thead>
      <tbody>
        <tr v-for="(catalog, index) in catalogs" v-bind:key="index">
          <td>
            <div v-if="catalog.default" class="level">
              <div class="level-left">
                <div class="level-item">{{catalog.name}}</div>
                <div class="level-item"><span class="default-tag">default</span></div>
              </div>
            </div>
            <div v-else>{{catalog.name}}</div>
          </td>
          <td>
            {{formatDate(catalog.createDate)}}
          </td>
          <td>
          <span class="has-text-weight-light not-provided"> No expiration date </span>
          </td>
          <td>
            <assigned-catalog-accounts v-if="catalog.companiesAssignedToCatalog.length" :companies="catalog.companiesAssignedToCatalog"></assigned-catalog-accounts>
            <span v-else class="not-provided">No companies assigned</span>
          </td>
          <td>
            <span class="tag is-active">Active</span>
          </td>
          <td>
              <span @click="openEmailCatalog(catalog.id)" class="cursor-pointer margin-small left">
              <i class="fa fa-envelope" aria-hidden="true"></i>
              </span>
              <span @click="downloadCatalog(catalog)" class="cursor-pointer margin-small left">
              <i class="fa fa-download" aria-hidden="true"></i>
              </span>
          </td>
          <td>
            <popper class="cog-wheel" trigger="click" :appendToBody="true" :options="{placement: 'bottom-start'}">
                <div class="popper">
                  <div class="dropdown-content">
                    <brand-hover>
                      <span class="dropdown-item" @click="editCatalog(catalog.id)">
                        Edit Price Schedule
                      </span>
                    </brand-hover>
                    <div v-show="showAdditionalCatalogOptions">
                      <hr v-if="!catalog.default" class="dropdown-divider">
                      <brand-hover v-if="!catalog.default">
                        <span class="dropdown-item" @click="changeDefaultCatalog(catalog.id)">
                          Set as default schedule
                        </span>
                      </brand-hover>
                      <hr class="dropdown-divider">
                      <brand-hover class="has-text-danger">
                        <span class="dropdown-item" @click="showDeleteCatalogModal(catalog.id)">
                          Delete Schedule
                        </span>
                      </brand-hover>
                    </div>
                  </div>
                </div>
                <cloud-plus-cog slot="reference"></cloud-plus-cog>
            </popper>
          </td>
        </tr>
      </tbody>
    </table>
    <delete-catalog-configrmation-modal
      v-if="showDeleteModal" :showModal="showDeleteModal"
      :showLoadingIcon="deletingCatalog"
      :disableConfirmButton="deletingCatalog"
      @cancel="closeModalDeleteCatalogConfirmationModal"
      @confirm = "confirmModalDeleteCatalogConfirmationModal"
      confirmText="Delete">Are you sure you want to delete this catalog?
    </delete-catalog-configrmation-modal>
    <catalog-download-modal v-if="showCatalogDownload" :catalog="selectedCatalog" :showModal="showCatalogDownload" @closeModal="closeCatalogDownloadModal"></catalog-download-modal>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import moment from 'moment';
import Popper from '@/components/shared/popper/Popper';
import toasterMixin from '@/mixins/toaster';
import loadingMixin from '@/mixins/loading';
import DeleteCatalogConfigrmationModal from '@/components/shared/modals/ConfirmationModal';
import CloudPlusCog from '@/components/shared/misc/CloudPlusCog';
import catalogService from '@/services/catalogService';
import AssignedCatalogAccounts from './AssignedCatalogAccounts';
import CatalogDownloadModal from './CatalogDownloadModal';

export default {
  components: {
    Popper,
    AssignedCatalogAccounts,
    CloudPlusCog,
    DeleteCatalogConfigrmationModal,
    CatalogDownloadModal,
  },
  mixins: [toasterMixin, loadingMixin],
  props: {
    catalogs: {
      required: true,
      type: Array,
    },
    showAdditionalCatalogOptions: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      showDeleteModal: false,
      deleteCatalogId: -1,
      deletingCatalog: false,
      showCatalogDownload: false,
      selectedCatalog: {},
    };
  },
  computed: {
    ...mapGetters({
      companyId: 'userAuth/companyId',
    }),
  },
  methods: {
    ...mapActions({
      deleteCatalog: 'catalog/deleteCatalog',
      changeDefaultCatalog: 'catalog/changeDefaultCatalog',
    }),
    formatDate(date) {
      if (date) {
        return moment(date).format('MM/DD/YYYY');
      }
      return '';
    },
    editCatalog(catalogId) {
      this.$router.push({
        path: `/catalogs/${catalogId}/reseller`,
      });
    },
    showDeleteCatalogModal(catalogId) {
      this.deleteCatalogId = catalogId;
      this.showDeleteModal = true;
    },
    closeModalDeleteCatalogConfirmationModal() {
      this.deleteCatalogId = -1;
      this.showDeleteModal = false;
    },
    confirmModalDeleteCatalogConfirmationModal() {
      this.deletingCatalog = true;
      this.deleteCatalog(this.deleteCatalogId).then(() => {
        this.sucessToaster({
          text: 'You\'ve successfully delete the catalog.',
          icon: 'trash',
        });
      }).finally(() => {
        this.deletingCatalog = false;
        this.deleteCatalogId = -1;
        this.showDeleteModal = false;
      });
    },
    openEmailCatalog(catalogId) {
      this.selectedCatalog = {
        id: catalogId,
        name: this.catalogs.find(c => c.id === catalogId).name,
      };
      this.showCatalogDownload = true;
    },
    closeCatalogDownloadModal() {
      this.showCatalogDownload = false;
    },
    downloadPriceScheduleAsExcel(blob, catalogName) {
      // It is necessary to create a new blob object with mime-type explicitly set
      // otherwise only Chrome works like it should
      const newBlob = new Blob([blob], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

      // IE doesn't allow using a blob object directly as link href
      // instead it is necessary to use msSaveOrOpenBlob
      if (window.navigator && window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(newBlob);
        return;
      }

      // For other browsers:
      // Create a link pointing to the ObjectURL containing the blob.
      const data = window.URL.createObjectURL(newBlob);
      const link = document.createElement('a');
      link.setAttribute('type', 'hidden');
      link.href = data;
      link.download = `${catalogName}.xlsx`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

      setTimeout(() => {
        // For Firefox it is necessary to delay revoking the ObjectURL
        window.URL.revokeObjectURL(data);
      }, 100);
    },
    async downloadCatalog(catalog) {
      this.isLoading = true;
      const rawDownloadResponse = await catalogService.rawDownload(catalog.id, this.companyId);
      this.isLoading = false;
      const priceScheduleDetails = rawDownloadResponse.data;
      if (priceScheduleDetails != null) {
        this.downloadPriceScheduleAsExcel(priceScheduleDetails, catalog.name);
      }
    },
  },
  mounted() {
    this.isLoading = false;
  },
};
</script>

<style scoped>
.default-tag {
  font-size: 0.5rem;
  text-transform: uppercase;
  color: #ffffff;
  background-color: #50c54e;
  padding: 0.2rem 0.6rem;
  border-radius: 3px;
  margin-left: 1rem;
}
.cog-wheel {
  justify-content: flex-end;
}
.fa-envelope,.fa-download {
  color: #54667a;
}
</style>
