import Vue from 'vue';
import Router from 'vue-router';

// Users
import UserList from '../components/users/UserList';
import CreateUser from '../components/users/CreateUser';
import EditUser from '../components/users/editUser/EditUser';
import EditProfile from '../components/users/EditProfile';

// Companies
import CreateCompany from '../components/companies/createCompany/CreateCompany';
import CompanyList from '../components/companies/CompanyList';
import EditCompany from '../components/companies/editCompany/EditCompany';
import CompanyDetails from '../components/companies/CompanyDetails';
import EditMyCompany from '../components/companies/editCompany/EditMyCompany';

import Announcements from '../components/announcements/Announcements';
import ForgotPassword from '../components/authentication/ForgotPassword';
import ChangePassword from '../components/authentication/ChangePassword';

import ExternalResellerSignupForm from '../components/companies/forms/ExternalResellerSignupForm';
import ExternalCustomerSignupForm from '../components/companies/forms/ExternalCustomerSignupForm';
import ProductDetails from '../components/catalogs/customer/products/ProductDetails';
/* import Office365Configuration from
'../components/services/office365/Office365Configuration';
import Office365ProductTransition from
'../components/services/office365/Office365ProductTransition'; */
import Office365TransitionUserAndLicensesForm from '../components/services/office365/Office365TransitionUserAndLicensesForm';

// Catalog
import ResellerCatalog from '../components/catalogs/reseller/ResellerCatalogs';
import EditCatalog from '../components/catalogs/reseller/EditCatalog';
import CustomerCatalog from '../components/catalogs/customer/CustomerCatalog';

import Dashboard from '../components/dashboard/Dashboard';
import Customization from '../components/cutomizations/Customization';
import Chart from '../components/charts/Chart';
import AdminDashboard from '../components/dashboard/AdminDashboard';
import MyServices from '../components/services/MyServices';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      redirect: { path: 'dashboard' },
      meta: { requiresAuth: true, friendlyName: 'Dashboard', permission: [] },
    },
    {
      path: '/dashboard',
      component: Dashboard,
      meta: { requiresAuth: true, friendlyName: 'Dashboard', permission: ['ViewDashboard'] },
    },
    {
      path: '/admin-dashboard',
      component: AdminDashboard,
      meta: { requiresAuth: true, friendlyName: 'AdminDashboard', permission: ['ViewAdminDashboard'] },
    },
    {
      path: '/chart',
      component: Chart,
      meta: { requiresAuth: true, friendlyName: 'chart', permission: [] },
      props: route => ({ chartType: route.query.chartType }),
    },
    {
      path: '/customization',
      component: Customization,
      meta: { requiresAuth: true, friendlyName: 'Customization', permission: [] },
    },
    {
      path: '/activities',
      meta: { requiresAuth: true, friendlyName: 'Activity center' },
    },
    {
      path: '/announcements-and-news',
      component: Announcements,
      meta: { requiresAuth: true, friendlyName: 'Announcements and News' },
    },
    {
      path: '/users',
      component: UserList,
      meta: { requiresAuth: true, friendlyName: 'Users List', permission: ['ViewUsers'] },
    },
    {
      path: '/users/create',
      component: CreateUser,
      meta: { requiresAuth: true, friendlyName: 'Create User', permission: ['AddUsers'] },
    },
    {
      path: '/users/edit/:id',
      component: EditUser,
      meta: { requiresAuth: true, friendlyName: 'Edit User', permission: ['EditUsers'] },
      props: true,
    },
    {
      path: '/profile/edit',
      component: EditProfile,
      meta: { requiresAuth: true, friendlyName: 'Edit Profile', permission: [] },
      props: true,
    },
    {
      path: '/auth/forgotPassword',
      component: ForgotPassword,
      meta: { permission: [] },
      name: 'forgotPassword',
      props: route => ({ email: route.query.email }),
    },
    {
      path: '/auth/changePassword',
      component: ChangePassword,
      meta: { permission: [] },
      name: 'changePassword',
    },
    {
      path: '/companies',
      component: CompanyList,
      meta: { requiresAuth: true, friendlyName: '', permission: ['ViewAccounts'] },
      props: route => ({ accountType: route.query.accountType }),
    },
    {
      path: '/companies/create',
      component: CreateCompany,
      meta: { requiresAuth: true, friendlyName: 'Create New Account', permission: ['AddAccounts'] },
      props: route => ({ accountType: route.query.accountType }),
    },
    {
      path: '/companies/edit/:id',
      component: EditCompany,
      meta: { requiresAuth: true, friendlyName: 'Edit Company', permission: ['AddUsers', 'EditAccounts'] },
      props: true,
    },
    {
      path: '/companies/edit-my-company',
      component: EditMyCompany,
      meta: { requiresAuth: true, friendlyName: 'Edit My Company Details', permission: ['EditMyCompany', 'ViewMyCompany'] },
    },
    {
      path: '/companies/details',
      component: CompanyDetails,
      meta: { requiresAuth: true, permission: [] },
      props: route => ({ companyLevel: route.query.companyLevel }),
    },
    {
      path: '/services',
      component: CreateUser,
      meta: { requiresAuth: true, friendlyName: 'Services' },
    },
    {
      path: '/branding',
      component: CreateUser,
    },
    {
      path: '/onboarding',
      component: CreateUser,
    },
    {
      path: '/document-generator',
      component: CreateUser,
    },
    {
      path: '/welcome-letters',
      component: CreateUser,
    },
    {
      path: '/sales-and-marketing',
      component: CreateUser,
    },
    {
      path: '/external-reseller-signup-form',
      component: ExternalResellerSignupForm,
      name: 'externalResellerSignupForm',
      meta: { permission: [] },
      props: route => ({ parentId: route.query.parentId, brandColor: route.query.brandColor }),
    },
    {
      path: '/external-customer-signup-form',
      component: ExternalCustomerSignupForm,
      name: 'externalCustomerSignupForm',
      meta: { permission: [] },
      props: route => ({ parentId: route.query.parentId }),
    },
    {
      path: '/catalogs/customer/products/:productId',
      component: ProductDetails,
      props: true,
      meta: { requiresAuth: true, permission: ['ViewProductCatalog'] },
      canReuse: false,
    },
    {
      path: '/my-services',
      name: 'myServices',
      // component: Office365Configuration,
      component: MyServices,
      meta: { requiresAuth: true, permission: ['ViewProductCatalog'] },
      props: true,
    },
    {
      path: '/catalogs/reseller',
      component: ResellerCatalog,
      meta: { requiresAuth: true, permission: ['ViewPriceCatalog'] },
      name: 'Price Catalogs',
    },
    {
      path: '/catalogs/:catalogId/reseller',
      component: EditCatalog,
      meta: { requiresAuth: true, permission: ['ViewPriceCatalog'] },
      name: 'Price Catalog',
      props: true,
    },
    {
      path: '/catalogs/customer',
      component: CustomerCatalog,
      meta: { requiresAuth: true, permission: ['ViewProductCatalog'] },
      name: 'Product Catalog',
      props: true,
    },
    /* {
      path: '/product-transition',
      component: Office365ProductTransition,
      meta: { requiresAuth: true, permission: ['ViewProductCatalog'] },
      name: 'Product Transition',
      props: true,
    }, */
    {
      path: '/transition-user-licenses',
      component: Office365TransitionUserAndLicensesForm,
      meta: { requiresAuth: true, permission: ['ViewProductCatalog'] },
      name: 'Users and Licenses',
      props: true,
    },
  ],
});
