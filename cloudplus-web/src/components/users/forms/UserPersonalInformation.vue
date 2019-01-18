<template>
  <div class="content">
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-textfield
          @input="onInput('firstName', $event)"
          name="firstName"
          v-validate="'required|max:60'"
          data-vv-as="First Name"
          :value="user.firstName"
          :validationErrors="errors.first('firstName')">First Name</cloud-plus-textfield>
      </div>
      <div class="column">
        <cloud-plus-textfield
          @input="onInput('lastName', $event)"
          name="lastName"
          v-validate="'required|max:60'"
          data-vv-as="Last Name"
          :value="user.lastName"
          :validationErrors="errors.first('lastName')">Last Name</cloud-plus-textfield>
      </div>
    </div>
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-textfield
          @focus="onDisplayNameFocus"
          @input="onInput('displayName', $event)"
          name="displayName"
          v-validate="'required|max:60'"
          data-vv-as="Display Name"
          :value="user.displayName"
          :validationErrors="errors.first('displayName')">Display Name</cloud-plus-textfield>
      </div>
    </div>
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-textfield
          @input="onInput('companyName', $event)"
          v-validate="'max:60'"
          name="companyName"
          data-vv-as="Company Name"
          :value="user.companyName"
          :validationErrors="errors.first('companyName')">Company</cloud-plus-textfield>
      </div>
      <div class="column">
        <cloud-plus-textfield
          @input="onInput('jobTitle', $event)"
          name="jobTitle"
          data-vv-as="Job Title"
          :value="user.jobTitle"
          v-validate="'max:60'"
          :validationErrors="errors.first('jobTitle')">Job Title</cloud-plus-textfield>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';

export default {
  inject: {
    $validator: '$validator',
  },
  data() {
    return {
      checkingDisplayNameAvailability: false,
      checkDisplayNameVvalidateString: 'required|max:60',
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/personalInformation',
    }),
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
      unsavedUserChangesPresent: 'user/UNSAVED_USER_CHANGES_PRESENT',
    }),
    onInput(key, value) {
      this.unsavedUserChangesPresent();
      this.updateUserProperty({
        key,
        value,
      });
    },
    onDisplayNameFocus() {
      if (this.user.firstName !== '' && this.user.lastName !== '') {
        this.updateUserProperty({
          key: 'displayName',
          value: `${this.user.firstName} ${this.user.lastName}`,
        });
        this.$validator.validate('displayName', this.user.displayName);
      }
    },
  },
};
</script>

<style>

</style>
