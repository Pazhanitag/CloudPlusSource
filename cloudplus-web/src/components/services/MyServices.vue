<template>
  <div>
    <component-sticky-header title="My Services"></component-sticky-header>
    <div class="component-main">
      <loading-icon size="2" v-if="isLoading"></loading-icon>
      <div :class="[!isLoading && provisioningStatus === statusNotEnabled && supportProductItems.length === 0 ? '' : 'component-main__white']" v-else>
        <expansion-panel :product="products.find(p => p.id ===  office365Product.id)" v-if="provisioningStatus !== statusNotEnabled">
          <template>
            <office365-configuration :provisioningStatus="provisioningStatus" @setProvisioningStatus="setProvisioningStatus" :productTransitionStatus="productTransitionStatus">
            </office365-configuration>
          </template>
        </expansion-panel>
        <expansion-panel v-for="productItem in supportProductItems" v-bind:key="productItem.id" :product="productItem" v-if="supportProductItems.length > 0">
          <template>
            <div class="support-item-name">
              <tr>
                <td class="product-item-name">
                  <span class="product-item-weight"> Your saved Custom Control Panel URL is
                    <span class="has-text-weight-bold">{{productItem.customControlPanel[0].urls}}</span><br></span>
                  <!-- <div class="product-description">{{productItem.customControlPanel[0].urls}}</div> -->
                </td>
              </tr>
            </div>
          </template>
        </expansion-panel>
      </div>
      <div class="office-365-not-enabled-warning" v-if="!isLoading && provisioningStatus === statusNotEnabled && supportProductItems.length === 0">
        <div class="office-365-not-enabled-warning__title">You have no enabled services for assignment.</div>
        <router-link class="office-365-not-enabled-warning__subtitle" :to="'/catalogs/customer'" >
          <brand-color-primary><span class="link">Please choose desired products and services from The Catalog</span></brand-color-primary>
        </router-link>
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters, mapActions, mapMutations } from 'vuex';
import loadingMixin from '@/mixins/loading';
import ComponentStickyHeader from '@/components/shared/navigation/ComponentStickyHeader';
import { office365ConfigurationStatus, provisioningStatus, catalogIdConstants, productIdConstants } from '@/assets/constants/commonConstants';
import provisioningService from '@/services/provisioningService';
import office365UtilitiesService from '@/services/office365UtilitiesService';
import ExpansionPanel from './ExpansionPanel';
import Office365Configuration from './office365/Office365Configuration';

export default {
  mixins: [loadingMixin],
  data() {
    return {
      provisioningStatus: {
        notProvisioned: provisioningStatus.NotProvisioned,
        provisioned: provisioningStatus.Provisioned,
        inTransition: provisioningStatus.InTransition,
      },
      statusEnabled: office365ConfigurationStatus.Enabled,
      statusNotEnabled: office365ConfigurationStatus.NotEnabled,
      statusCompleted: office365ConfigurationStatus.Completed,
      statusInProgress: office365ConfigurationStatus.InProgress,
      catalogIds: catalogIdConstants,
      productIds: productIdConstants,
      productTransitionStatus: false,
    };
  },
  components: {
    ComponentStickyHeader,
    ExpansionPanel,
    Office365Configuration,
  },
  computed: {
    ...mapGetters({
      office365Product: 'product/selectedProduct',
      supportProductItems: 'support/supportProductItems',
      products: 'product/allProducts',
      companyId: 'userAuth/companyId',
    }),
  },
  methods: {
    ...mapActions({
      getAllProducts: 'product/getAllProducts',
      setCustomerStatusToInTransition: 'office365/setProvisioningStatusToInTransition',
      getSupportProductItems: 'support/getSupportProductItems',
    }),
    ...mapMutations({
      setProducts: 'product/SET_PRODUCTS',
    }),
    setProvisioningStatus(status) {
      this.provisioningStatus = status;
      if (this.provisioningStatus === this.statusNotEnabled) {
        this.setProducts(this.products.filter(p => p.id !== this.catalogIds.office365));
      }
    },
    setStatusIcon(target, id, icon) {
      return target.map(element => {
        if (element.id === id) {
          element.statusIcon = icon;
          return element;
        }
        return element;
      });
    },
    getOffice365ProvisioningStatus() {
      return provisioningService.getProductAvailability(this.companyId, this.office365Product.id)
        .then(response => {
          const office365ProvisioningStatusResponse = response.data.result;
          if (office365ProvisioningStatusResponse === this.provisioningStatus.inTransition) {
            this.productTransitionStatus = true;
            this.setProducts(this.setStatusIcon(this.products, this.catalogIds.office365, 'fa fa-hourglass'));
          } else if (office365ProvisioningStatusResponse ===
            this.provisioningStatus.notProvisioned) {
            this.provisioningStatus = office365ConfigurationStatus.NotEnabled;
            this.setProducts(this.products.filter(p => p.id !== this.catalogIds.office365));
          } else {
            office365UtilitiesService.getProvisioningStatus(this.companyId)
              .then(officeServiceResponse => {
                this.provisioningStatus = officeServiceResponse.data.result;
                if (this.provisioningStatus === this.statusEnabled) {
                  this.setProducts(this.setStatusIcon(this.products, this.catalogIds.office365, 'fa fa-exclamation-triangle'));
                } else if (this.provisioningStatus === this.statusCompleted) {
                  this.setProducts(this.setStatusIcon(this.products, this.catalogIds.office365, 'fa fa-check'));
                } else if (this.provisioningStatus === this.statusInProgress) {
                  this.setProducts(this.setStatusIcon(this.products, this.catalogIds.office365, 'fa fa-hourglass'));
                }
              });
          }
        });
    },
    productDescription(product) {
      const startChar = product.description.indexOf('main-description');
      let spaceChar = false;
      let counter = startChar + 120;
      if (product.name === 'Microsoft Exchange Online - Kiosk') {
        return product.description.slice(startChar + 23, startChar + 90);
      }
      if (product.name === this.productIds.customSecureControlPanelURL) {
        return product.description.slice(startChar + 23, startChar + 65);
      }
      while (!spaceChar) {
        if (product.description.charAt(counter) === '') {
          spaceChar = true;
        } else {
          counter += 1;
        }
      }
      return product.description.slice(startChar + 23, counter);
    },
  },
  async created() {
    this.isLoading = true;
    await this.getSupportProductItems(this.companyId);
    await this.getAllProducts();
    await this.getOffice365ProvisioningStatus();
    this.isLoading = false;
  },
};
</script>

<style lang="scss" scoped>
  .office-365-not-enabled-warning{
    padding-top: 18rem;
    text-align: center;
    &__title {
      color: $label-color;
      font-size: $big-font-size;
    }
    &__subtitle {
      color: $subtitle-color;
      font-size: $primary-font-size;
    }
  }

  .support-item-name {
    max-width: 557px;
    margin-left: auto;
    text-align: right;
  }
</style>
