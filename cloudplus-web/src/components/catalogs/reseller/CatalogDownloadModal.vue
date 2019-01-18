<template>
  <div>
    <cloud-plus-card-modal :showModal="showModal" @closeModal="closeModal" :modalContentHeight="'auto'"  width="50rem">
      <p slot="header">Send Price Schedule via Email</p>
      <div slot="content" class="content">
        <h2 class="subtitle">Please fill in the details to send Price schedule.</h2>
      </div>
      <section slot="section">
        <div class="columns">
          <div class="column">
            <div :class="['custom-email-field', {'active' : toErrorMessage !== ''}]">
              <cloud-plus-textfield v-model="catalogDownload.to" :validationErrors='toErrorMessage'>
                Email Recipients
                <div class="custom-tag-col" v-if="recipients.length > 0">
                <brand-background-with-text-color v-for="(localEmail, index) in recipients" :key="index">{{localEmail}} <a @click="removeRecipient(index)"><brand-fa :class="['fa', 'fa-times']"></brand-fa></a></brand-background-with-text-color>
                </div>
              </cloud-plus-textfield>
              <div class="custom-plus" @click="updateRecipients(catalogDownload.to.toLowerCase())">
                <brand-fa :class="['fa', 'fa-plus-circle', 'fa-lg']"></brand-fa>
              </div>
            </div>
            <cloud-plus-textfield
              @input="onInput('subject', $event)"
              :value="catalogDownload.subject"
              data-vv-name="subject"
              name="subject"
              v-validate="'required'"
              data-vv-as="subject"
              :validationErrors="errors.first('subject')">Email Subject
            </cloud-plus-textfield>
            <cloud-plus-textarea
              @input="onInput('body', $event)"
              :value="catalogDownload.body"
              :rows = 3
              data-vv-name="body"
              name="body"
              v-validate="'required'"
              data-vv-as="body"
              :validationErrors="errors.first('body')">Email Content
            </cloud-plus-textarea>
          </div>
        </div>
      </section>
      <div slot="footer">
        <brand-primary-btn class="btn-new-catalog--modal-footer" :disabled="saving" @click="downloadCatalog()">
          Send
          <loading-icon :inline="true" v-show="saving"></loading-icon>
        </brand-primary-btn>
      </div>
    </cloud-plus-card-modal>
  </div>
</template>

<script>
import CloudPlusCardModal from '@/components/shared/CloudPlusCardModal';
import toasterMixin from '@/mixins/toaster';
import { mapActions, mapGetters } from 'vuex';
import catalogService from '@/services/catalogService';

export default {
  $_veeValidate: {
    validator: 'new',
  },
  mixins: [toasterMixin],
  components: {
    CloudPlusCardModal,
  },
  props: {
    showModal: {
      type: Boolean,
    },
    catalog: {
      required: true,
      type: Object,
    },
  },
  data() {
    return {
      saving: false,
      catalogDownload: {
        to: '',
        subject: '',
        body: '',
      },
      recipients: [],
      toErrorMessage: '',
    };
  },
  computed: {
    ...mapGetters({
      userProfile: 'userAuth/userProfile',
    }),
  },
  methods: {
    ...mapActions({
      createCatalog: 'catalog/createCatalog',
    }),
    joinArray(arrayToJoin, separator = ',') {
      return arrayToJoin.join(separator);
    },
    downloadCatalog() {
      const catalogToDownload = {
        catalogId: this.catalog.id,
        userId: this.userProfile.id,
        companyId: this.userProfile.companyId,
        recipients: this.joinArray(this.recipients),
        subject: this.catalogDownload.subject,
        body: this.catalogDownload.body,
      };
      this.$validator.validateAll().then(result => {
        if (result) {
          if (this.recipients.length > 0) {
            this.saving = true;
            catalogService.downloadCatalog(catalogToDownload).then(() => {
              this.closeModal();
              this.sucessToaster({
                text: 'Price Schedule is successfully sent via email',
              });
            }).finally(() => {
              this.saving = false;
            });
          } else {
            this.toErrorMessage = 'please add atleast one recipient';
          }
        }
      });
    },
    closeModal() {
      this.$emit('closeModal');
    },
    onInput(key, value) {
      this.catalogDownload[key] = value;
    },
    removeRecipient(index) {
      this.recipients.splice(index, 1);
    },
    validateEmail(email) {
      // eslint-disable-next-line
      const regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return regex.test(email);
    },
    updateRecipients(email) {
      if (this.validateEmail(email)) {
        if (!this.recipients.includes(email)) {
          this.recipients.push(email);
          this.toErrorMessage = '';
          this.catalogDownload.to = '';
        } else {
          this.toErrorMessage = 'Email Address already exists in the list';
        }
      } else {
        this.toErrorMessage = this.catalogDownload.to === '' ? 'The Email Address field is required' : 'Invalid Email Address';
      }
    },
  },
  mounted() {
    this.catalogDownload.subject = `Price Schedule - ${this.catalog.name}`;
    this.catalogDownload.body = 'Please see the attached Price Schedule';
    this.updateRecipients(this.userProfile.email);
  },
};
</script>

<style scoped lang="scss">
.content {
  padding-top: 1.875rem;
  padding-bottom: 1rem;
}

.title {
  text-align: center;
  color: $label-color;
  font-size: $font-size;
  padding-top: 1.875rem;
  padding-bottom: 1rem;
}

.subtitle {
  color: $subtitle-color;
  font-size: $secondary-font-size;
}

.description {
  width: 100%;
  border-radius: $border-radius;
}

.field:first-of-type {
  min-height: 4.5rem;
  padding-top: 0.3rem;
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
</style>
