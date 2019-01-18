<template>
  <div class="content">
    <div class="columns is-desktop">
      <div class="column">
        <cloud-plus-radio-btn name="password_setup" :checked="user.passwordSetupMethod == passwordSetupMethod.GeneratePasswordViaLink" checkedValue="1" :value="user.passwordSetupMethod" v-model="user.passwordSetupMethod" @input="onInput('passwordSetupMethod', 1)">Set password by email link
        </cloud-plus-radio-btn>
      </div>
      <div class="column">
        <cloud-plus-radio-btn class="checkbox-padding" name="password_setup" :checked="user.passwordSetupMethod == passwordSetupMethod.GeneratePasswordManually" checkedValue="2" :value="user.passwordSetupMethod" @input="onInput('passwordSetupMethod', 2)">Manually generate password
        </cloud-plus-radio-btn>
      </div>
    </div>
    <div class="columns is-desktop" v-if="user.passwordSetupMethod == passwordSetupMethod.GeneratePasswordViaLink">
      <div class="column">
        <cloud-plus-select
          :selected="selectedEmailAddressValue"
          @input="changeSelectedEmailAddressValue($event)"
          :options="emailAddressoptions">Use
        </cloud-plus-select>
      </div>
    </div>
    <div  v-if="user.passwordSetupMethod == passwordSetupMethod.GeneratePasswordManually">
      <div class="columns is-desktop">
        <div class="column">
          <cloud-plus-password-textfield v-validate="'required|min:6'" :value="user.password" @input="onInput('password', $event)" data-vv-name="password" class="is-pulled-left password-text-field-with-randomize" :validationErrors="errors.first('password')">New Password</cloud-plus-password-textfield>
          <i class="fa fa-refresh random-password" @click="generatePassword" title="Random password"></i>
          <cloud-plus-check-box class="same-as-domain-checkbox"  @input="onSendPasswordViaEmailChecked" :value="user.sendPlainPasswordViaEmail">Email new password</cloud-plus-check-box>
        </div>
        <div class="column">
          <cloud-plus-password-textfield v-validate="`retyped_password:${user.password}|required|min:6`" data-vv-name="retypedPassword" data-vv-as="retype password" :value="user.passwordRetyped" @input="onInput('passwordRetyped', $event)" :validationErrors="errors.first('retypedPassword')">Retype Password</cloud-plus-password-textfield>
        </div>
      </div>
      <div v-if="user.sendPlainPasswordViaEmail">
        <div class="columns is-desktop">
          <div class="column">
            <p class="warning-message">WARNING! This option will send the access credentials in plain text to the provided email address.</p>
          </div>
        </div>
        <div class="columns is-desktop">
          <div class="column">
            <cloud-plus-select
              :selected="selectedEmailAddressValue"
              @input="changeSelectedEmailAddressValue($event)"
              :options="emailAddressoptions">Use
            </cloud-plus-select>
          </div>
        </div>
      </div>
       <div class="columns is-desktop" v-if="user.sendPlainPasswordViaEmail">
      <div class="column">
        <cloud-plus-textfield
          v-validate="{ rules: { required: !this.primaryOrAlternateSelected, email: !this.primaryOrAlternateSelected} }"
          :value="user.passwordSetupEmail"
          @input="onInput('passwordSetupEmail', $event)"
          data-vv-delay="300"
          data-vv-name="passwordSetupEmail"
          data-vv-as="email"
          name="passwordSetupEmail"
          :disabled="primaryOrAlternateSelected"
          :validationErrors="errors.first('passwordSetupEmail')">Email
        </cloud-plus-textfield>
      </div>
      <div class="column" v-if="!primaryOrAlternateSelected">
        <cloud-plus-textfield v-validate="`retyped_email:${user.passwordSetupEmail}|required|email`" data-vv-delay="300" data-vv-name="passwordSetupEmailRetyped" data-vv-as="retype email" name="passwordSetupEmailRetyped" :value="user.passwordSetupEmailRetyped" @input="onInput('passwordSetupEmailRetyped', $event)" :validationErrors="errors.first('passwordSetupEmailRetyped')">Repeat Email</cloud-plus-textfield>
      </div>
    </div>
    </div>
    <div class="columns is-desktop" v-if="user.passwordSetupMethod == passwordSetupMethod.GeneratePasswordViaLink">
      <div class="column">
        <cloud-plus-textfield
          v-validate="{ rules: { required: !this.primaryOrAlternateSelected, email: !this.primaryOrAlternateSelected} }"
          :value="user.passwordSetupEmail"
          @input="onInput('passwordSetupEmail', $event)"
          data-vv-name="passwordSetupEmail"
          data-vv-as="email"
          name="passwordSetupEmail"
          :disabled="primaryOrAlternateSelected"
          :validationErrors="errors.first('passwordSetupEmail')">Email
        </cloud-plus-textfield>
      </div>
      <div class="column" v-if="!primaryOrAlternateSelected">
        <cloud-plus-textfield v-validate="`retyped_email:${user.passwordSetupEmail}|required|email`" data-vv-name="passwordSetupEmailRetyped" data-vv-as="retype email" name="passwordSetupEmailRetyped" :value="user.passwordSetupEmailRetyped" @input="onInput('passwordSetupEmailRetyped', $event)" :validationErrors="errors.first('passwordSetupEmailRetyped')">Repeat Email</cloud-plus-textfield>
      </div>
    </div>

  </div>
</template>

<script>
import { mapGetters, mapMutations } from 'vuex';
import PasswordGenerator from 'generate-password';
import CloudPlusPasswordTextfield from '@/components/shared/input-components/CloudPlusPasswordTextfield';
import { userPasswordSetupMethod } from '@/assets/constants/commonConstants';

export default {
  inject: {
    $validator: '$validator',
  },
  components: {
    CloudPlusPasswordTextfield,
  },
  watch: {
    // eslint-disable-next-line
    'user.passwordSetupMethod': function () {
      if (this.user.passwordSetupMethod === this.passwordSetupMethod.GeneratePasswordViaLink) {
        this.updateUserProperty({
          key: 'sendPlainPasswordViaEmail',
          value: false,
        });
      }
    },
    // eslint-disable-next-line
    'generalInformation.emailAddress': function() {
      if (this.validateEmail(this.generalInformation.emailAddress) === true) {
        if (this.emailAddressoptions.find(option => option.value === 1) === undefined) {
          this.emailAddressoptions.push({
            name: 'Primary Email Address',
            value: 1,
          });
          this.selectedEmailAddressValue = 1;
          this.primaryOrAlternateSelected = true;
        }
      } else {
        const indexOfemailAddress =
          this.emailAddressoptions.findIndex(option => option.value === 1);
        if (indexOfemailAddress > -1) {
          this.emailAddressoptions.splice(indexOfemailAddress, 1);
          this.selectedEmailAddressValue = 3;
          this.changeEmailAddress(3);
        }
      }
    },
    // eslint-disable-next-line
    'generalInformation.alternativeEmail': function() {
      if (this.validateEmail(this.generalInformation.alternativeEmail) === true) {
        if (this.emailAddressoptions.find(option => option.value === 2) === undefined) {
          this.emailAddressoptions.push({
            name: 'Alternate Email Address',
            value: 2,
          });
        }
      } else {
        const indexOfAlternative = this.emailAddressoptions.findIndex(option => option.value === 2);
        if (indexOfAlternative > -1) {
          this.emailAddressoptions.splice(indexOfAlternative, 1);
          this.selectedEmailAddressValue = 3;
          this.changeEmailAddress(3);
        }
      }
    },
  },
  data() {
    return {
      passwordSetupMethod: userPasswordSetupMethod,
      selectedEmailAddressValue: null,
      primaryOrAlternateSelected: true,
      emailAddressoptions: [],
    };
  },
  computed: {
    ...mapGetters({
      user: 'user/userSecurityInformation',
      generalInformation: 'user/generalInformation',
      companyContact: 'company/contactInformation',
    }),
  },
  methods: {
    ...mapMutations({
      updateUserProperty: 'user/UPDATE_USER_PROPERTY',
    }),
    onInput(key, value) {
      this.updateUserProperty({
        key,
        value,
      });
    },
    validateEmail(email) {
      // eslint-disable-next-line
      const regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return regex.test(email);
    },
    changeSelectedEmailAddressValue(value) {
      this.selectedEmailAddressValue = parseInt(value, 10);
      this.changeEmailAddress(this.selectedEmailAddressValue);
    },
    changeEmailAddress(value) {
      let primaryOrAlternativeOrCustomEmail = '';
      if (value === 3) {
        this.primaryOrAlternateSelected = false;
      } else {
        this.primaryOrAlternateSelected = true;
        switch (value) {
          case 1:
            primaryOrAlternativeOrCustomEmail = this.validateEmail(this.generalInformation.emailAddress) === true ? this.generalInformation.emailAddress : '';
            break;
          case 2:
            primaryOrAlternativeOrCustomEmail = this.validateEmail(this.generalInformation.alternativeEmail) === true ? this.generalInformation.alternativeEmail : '';
            break;
          default:
            break;
        }
      }
      this.updateUserProperty({
        key: 'passwordSetupEmail',
        value: primaryOrAlternativeOrCustomEmail,
      });
      this.updateUserProperty({
        key: 'passwordSetupEmailRetyped',
        value: primaryOrAlternativeOrCustomEmail,
      });
    },
    generatePassword() {
      const password = PasswordGenerator.generate({
        length: 6,
        numbers: true,
      });
      this.updateUserProperty({
        key: 'password',
        value: password,
      });
      this.updateUserProperty({
        key: 'passwordRetyped',
        value: password,
      });
      this.$validator.validate('password', password);
    },
    onSendPasswordViaEmailChecked(checkboxValue) {
      this.updateUserProperty({
        key: 'sendPlainPasswordViaEmail',
        value: checkboxValue,
      });
    },
  },
  mounted() {
    if (this.validateEmail(this.generalInformation.emailAddress) === true) {
      this.selectedEmailAddressValue = 1;
      this.emailAddressoptions.push({
        name: 'Primary Email Address',
        value: 1,
      });
      this.updateUserProperty({
        key: 'passwordSetupEmail',
        value: this.generalInformation.emailAddress,
      });
      this.updateUserProperty({
        key: 'passwordSetupEmailRetyped',
        value: this.generalInformation.emailAddress,
      });
    }
    if (this.validateEmail(this.generalInformation.alternativeEmail) === true) {
      this.emailAddressoptions.push({
        name: 'Alternate Email Address',
        value: 2,
      });
    }
    if (this.selectedEmailAddressValue === null) {
      this.selectedEmailAddressValue = 3;
      this.primaryOrAlternateSelected = false;
    }
    this.emailAddressoptions.push({
      name: 'Custom Email Address',
      value: 3,
    });
  },
};
</script>

<style scoped lang="scss">
.random-password {
  top: 2.325rem;
  font-size: $primary-font-size;
  position: relative;
  left: 1rem;
  width: 7%;
  cursor: pointer;
}

.password-text-field-with-randomize {
  width: 92%;
}
.warning-message {
  color: red;
  font-size: $small-font-size;
}
@media screen and (min-width: 1131px) {
  .checkbox-padding {
    padding-left: 0rem;
  }
}
@media screen and (max-width: 1130px) {
  .checkbox-padding {
    padding-left: 2rem;
  }
}
@media screen and (max-width: 1100px) {
  .checkbox-padding {
    padding-left: 0rem;
  }
}
</style>
