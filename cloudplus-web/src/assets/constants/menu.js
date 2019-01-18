export default {
  menu: [
    // {
    //   section: 'Home',
    //   items: [{
    //     name: 'Dashboard',
    //     icon: 'home',
    //     items: [{
    //       name: 'Reports',
    //       href: '/',
    //     }, {
    //       name: 'Announcements & News',
    //       href: '/announcements-and-news',
    //     }, {
    //       name: 'Notifications',
    //       href: '/notifications',
    //     }],
    //     href: '',
    //   }, {
    //     name: 'Activity center',
    //     icon: 'list',
    //     href: '/activities',
    //   }],
    // },
    {
      section: 'My Network',
      items: [{
        name: 'My Customers',
        icon: 'building',
        href: '/companies?accountType=1',
        exact: true,
        permissions: ['ViewAccounts'],
      }, {
        name: 'My Resellers',
        icon: 'tag',
        href: '/companies?accountType=0',
        exact: true,
        permissions: ['ViewAccounts'],
      }, {
        name: 'Create New Account',
        icon: 'mouse-pointer',
        href: '/companies/create',
        exact: false,
        permissions: ['AddAccounts'],
      }],
      permissions: ['ViewAccounts', 'AddAccounts'],
    },
    {
      section: 'Products And Services',
      items: [{
        name: 'Admin Dashboard',
        icon: 'tachometer',
        href: '/admin-dashboard',
        exact: true,
        permissions: ['ViewAdminDashboard'],
      },
      {
        name: 'My Dashboard',
        icon: 'bar-chart',
        href: '/dashboard',
        exact: true,
        permissions: ['ViewDashboard'],
      },
      {
        name: 'The Catalog',
        icon: 'cloud',
        href: '/catalogs/customer',
        exact: true,
        permissions: ['ViewProductCatalog'],
      }, {
        name: 'My Services',
        icon: 'briefcase',
        href: '/my-services',
      }, {
        name: 'Price Schedules',
        icon: 'book',
        href: '/catalogs/reseller',
        exact: true,
        permissions: ['ViewPriceCatalog'],
      },
      ],
      permissions: ['ViewPriceCatalog', 'SetMsrpFixed', 'ViewProductCatalog', 'ViewAdminDashboard', 'ViewDashboard'],
    },
    {
      section: 'My users',
      items: [
        {
          name: 'User List',
          icon: 'users',
          href: '/users',
          exact: true,
          permissions: ['ViewUsers'],
        },
        {
          name: 'Create New User',
          icon: 'user',
          href: '/users/create',
          exact: true,
          permissions: ['AddUsers'],
        },
      ],
      permissions: ['ViewUsers', 'AddUsers'],
    },
    {
      section: 'My Account',
      items: [
        {
          name: 'My Company',
          href: '/companies/details?companyLevel=0',
          icon: 'briefcase',
          exact: true,
        },
        {
          name: 'Support',
          href: '/companies/details?companyLevel=1',
          icon: 'building',
          exact: true,
        },
      //     {
      //       name: 'Sales and Marketing',
      //       icon: 'line-chart',
      //       items: [
      //         {
      //           name: 'Branding',
      //           href: '/branding',
      //         },
      //         {
      //           name: 'Onboarding',
      //           href: '/onboarding',
      //         },
      //         {
      //           name: 'Document generator',
      //           href: '/document-generator',
      //         },
      //         {
      //           name: 'Welcome letters',
      //           href: '/welcome-letters',
      //         },
      //       ],
      //       href: '',
      //     },
      ],
    },
  ],
};
