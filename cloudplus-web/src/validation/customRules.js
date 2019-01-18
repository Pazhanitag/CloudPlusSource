import userUtilitiesService from '@/services/userUtilitiesService';

const rules = {
  retyped_password: {
    getMessage: () => 'The retyped password does not match the new password.',
    validate: (value, [newPassword]) => value === newPassword,
  },
  retyped_email: { // TODO: merge retyped_password and retyped_email int one rule
    getMessage: () => 'The retyped email does not match the primary email.',
    validate: (value, [newEmail]) => value === newEmail,
  },
  user_exists: {
    getMessage: () => 'Primary or alternative email not found.',
    validate: value => userUtilitiesService.isEmailValied(value),
  },
  verify_no_special_characters: {
    getMessage: () => 'This field can not contain special characters.',
    validate: value => {
      const rx = new RegExp(/^[a-zA-Z0-9-._]+$/);
      return rx.test(value);
    },
  },
  phone_number_format: {
    getMessage: () => 'The Phone Number field is not a valid number.',
    validate: value => {
      const rx = new RegExp(/^(?=.*[0-9])[- /0-9]+$/);
      return rx.test(value);
    },
  },
  phone_number_format_office: {
    getMessage: () => 'The Phone Number field is not a valid number.',
    validate: value => {
      const rx = new RegExp(/^(1[ \-/.]?)?(\((\d{3})\)|(\d{3}))[ \-/.]?(\d{3})[ \-/.]?(\d{4})$/);
      return rx.test(value);
    },
  },
  verify_domain: {
    getMessage: () => 'This field is not a valid domain.',
    validate: value => {
      const rx = new RegExp(/^(?!www\.)(?:[a-zA-Z0-9]+(?:-*[a-zA-Z0-9])*\.)+[a-zA-Z]{2,6}$/);
      return rx.test(value);
    },
  },
  office365_password: {
    getMessage: () => 'You must choose a strong password that contains a combination of letters, and at least one number. Choose another password and try again.',
    validate: value => {
      const rx = new RegExp(/^(?=.*\d)(?=.*[a-zA-Z])/);
      return rx.test(value);
    },
  },
};

// export an 'install' function.
export default Validator => {
  // for every rule we defined above.
  Object.keys(rules).forEach(rule => {
    //  add the rule.
    Validator.extend(rule, rules[rule]);
  });
};
