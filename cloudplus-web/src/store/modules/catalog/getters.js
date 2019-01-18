export default {
  resellerCatalogs: state => state.resellerCatalogs,
  resellerCatalog: state => state.resellerCatalog,
  companiesAssignedCatalogs: state => state.companiesAssignedCatalogs,
  updateCatalog: state => ({
    productItems: state.resellerCatalog.products
      ? [].concat(...state.resellerCatalog.products.map(product => product.productItems)) : [],
    name: state.resellerCatalog.name,
    description: state.resellerCatalog.description,
    companiesAssignedToCatalog: state.resellerCatalog
      .companiesAssignedToCatalog.map(c => c.companyId),
  }),
  createCatalog: state => ({
    name: state.resellerCatalog.name,
    description: state.resellerCatalog.description,
    companiesAssignedToCatalog: state.resellerCatalog
      .companiesAssignedToCatalog.map(c => c.companyId),
  }),
  customerProducts: state => state.customerProducts,
};
