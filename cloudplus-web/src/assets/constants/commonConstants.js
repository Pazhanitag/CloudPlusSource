// User constants
export const userPasswordSetupMethod = {
  GeneratePasswordViaLink: 1,
  GeneratePasswordManually: 2,
};

export const userStatus = {
  Suspended: 0,
  Active: 1,
};

export const companyStatus = {
  Suspended: 0,
  Active: 1,
};

export const companyLevel = {
  MyCompany: 0,
  Parent: 1,
};

export const userServiceStatus = {
  NotAvailable: 0,
  Available: 1,
  InProgress: 2,
  Removed: 3,
  Assigned: 4,
};

export const contactResellerInformation = [
  {
    value: 'email',
    display: 'Email',
    label: 'info',
  },
  {
    value: 'phoneNumber',
    display: 'Phone Number',
    label: 'info',
  },
  {
    value: 'supportSite',
    display: 'Support URL',
    label: 'link',
  },
  {
    value: 'controlPanelSiteUrl',
    display: 'Control Panel URL',
    label: 'link',
  },
];
export const contactCustomerInformation = [
  {
    value: 'email',
    display: 'Email',
    label: 'info',
  },
  {
    value: 'phoneNumber',
    display: 'Phone Number',
    label: 'info',
  },
  {
    value: 'website',
    display: 'Website',
    label: 'link',
  },

];

export const locationCompanyInformation = [
  {
    value: 'country',
    display: 'Country',
  },
  {
    value: 'state',
    display: 'State',
  },
  {
    value: 'city',
    display: 'City',
  },
  {
    value: 'zipCode',
    display: 'Zip',
  },
];

export const office365ConfigurationStatus = {
  NotEnabled: 0,
  Enabled: 1,
  InProgress: 2,
  Completed: 3,
};

export const office365DomainStatus = {
  NotConfigured: 0,
  NotVerified: 1,
  Verified: 2,
};

export const provisioningStatus = {
  NotProvisioned: 0,
  Provisioned: 1,
  InTransition: 2,
};

// Catalog Id Constants
export const catalogIdConstants = {
  office365: 1,
  support: 2,
};

// Catalog Type Constants
export const catalogTypeConstants = {
  product: 1,
  service: 2,
};

// Product Id Constants
export const productIdConstants = {
  customSecureControlPanelURL: 'Custom Control Panel URL',
};
