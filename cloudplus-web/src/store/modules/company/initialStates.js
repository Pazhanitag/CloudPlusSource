import accountTypes from '@/assets/constants/accountTypes';
import billingTypes from '@/assets/constants/billingTypes';
import countries from '@/assets/constants/countries';

export default {
  companyDefaultState() {
    return {
      id: 0,
      type: accountTypes.Customer,
      name: '',
      billingType: billingTypes[0].value,
      tags: '',
      email: '',
      phoneNumber: '',
      country: countries[229].name,
      state: '',
      city: '',
      zipCode: '',
      streetAddress: '',
      domains: [{ name: '', isPrimary: true }],
      newDomains: [],
      website: '',
      supportSite: '',
      controlPanelSiteUrl: 'cp.cloudplusservice.com',
      websiteSameAsPrimaryDomain: true,
      logoBase64: '',
      logoUrl: '',
      catalogId: null,
      primaryBrandColor: {
        hex: '#2D8AC2',
        hsl: {
          h: 203,
          s: 0.62,
          l: 0.47,
        },
        hsv: {
          h: 203,
          s: 0.77,
          v: 0.76,
        },
        rgba: {
          r: 45,
          g: 138,
          b: 194,
        },
      },
      secondaryBrandColor: {
        hex: '#6abec5',
        hsl: {
          h: 185,
          s: 0.44,
          l: 0.59,
        },
        hsv: {
          h: 185,
          s: 0.46,
          v: 0.77,
        },
        rgba: {
          r: 106,
          g: 190,
          b: 197,
        },
      },
      textColor: {
        hex: '#ffffff',
        hsl: {
          h: 0,
          s: 0,
          l: 100,
        },
        hsv: {
          h: 0,
          s: 0,
          v: 100,
        },
        rgba: {
          r: 255,
          g: 255,
          b: 255,
        },
      },
      sendWelcomeLetter: false,
      sendOnboardingInformation: false,
      sendInformationAboutLatestServices: false,
      sendInformationAboutDiscounts: false,
      sendMiscNewsAndInformation: false,
      companyId: 0,
      domain: '',
      parentId: 0,
    };
  },
  pagedUsersDefaultState() {
    return {
      pageNumber: 0,
      pageSize: 0,
      totalNumberOfPages: 0,
      totalNumberOfRecords: 0,
      results: [],
    };
  },
  productCustomerCompanyDefaultState() {
    return {
      domain: '',
      firstName: '',
      lastName: '',
      address2: '',
    };
  },
};
