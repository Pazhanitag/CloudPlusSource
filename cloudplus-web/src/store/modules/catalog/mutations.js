import defaultStates from './initialStates';

export default {
  SET_RESELLER_CATALOGS(state, resellerCatalogs) {
    state.resellerCatalogs = resellerCatalogs;
  },
  SET_RESELLER_CATALOG(state, resellerCatalog) {
    state.resellerCatalog = resellerCatalog;
  },
  UPDATE_RESELLER_PRICE(state, { productItemId, newResellerPrice }) {
    state.resellerCatalog.products.forEach(product => {
      product.productItems.forEach(productItem => {
        if (productItem.productItemId === productItemId) {
          productItem.resellerPrice = newResellerPrice;
        }
      });
    });
  },
  UPDATE_RERATIL_PRICE(state, { productItemId, newRetailPrice }) {
    state.resellerCatalog.products.forEach(product => {
      product.productItems.forEach(productItem => {
        if (productItem.productItemId === productItemId) {
          productItem.retailPrice = newRetailPrice;
        }
      });
    });
  },
  UPDATE_FIXED_RETAIL_PRICE(state, { productItemId, fixedRetailPrice }) {
    state.resellerCatalog.products.forEach(product => {
      product.productItems.forEach(productItem => {
        if (productItem.productItemId === productItemId) {
          productItem.fixedRetailPrice = fixedRetailPrice;
        }
      });
    });
  },
  UPDATE_PRODUCT_ITEM_AVAILABILITY(state, { productItemId, available }) {
    state.resellerCatalog.products.forEach(product => {
      product.productItems.forEach(productItem => {
        if (productItem.productItemId === productItemId) {
          productItem.available = available;
        }
      });
    });
  },
  ADD_COMPANY_TO_CATALOG(state, companyId) {
    const companyToBeAdded = state.companiesAssignedCatalogs
      .find(c => c.companyId === companyId);
    state.resellerCatalog.companiesAssignedToCatalog.push(companyToBeAdded);
  },
  REMOVE_COMPANY_FROM_CATALOG(state, companyId) {
    const without = state.resellerCatalog.companiesAssignedToCatalog
      .filter(c => c.companyId !== companyId);
    state.resellerCatalog.companiesAssignedToCatalog = without;
  },
  SET_COMPANIES_ASSIGNED_CATALOGS(state, companiesCatalogs) {
    state.companiesAssignedCatalogs = companiesCatalogs;
  },
  UPDATE_CATALOG_PROPERTY(state, { key, value }) {
    state.resellerCatalog[key] = value;
  },
  RESET_CATALOG_STATE(state) {
    state.resellerCatalogs = defaultStates.resellerCatalogsDefaultState();
    state.resellerCatalog = defaultStates.resellerCatalogDefaultState();
    state.companiesAssignedCatalogs = defaultStates.companiesAssignedCatalogsDefaultState();
  },
  RESET_CURRENT_PRICE_CATALOG_STATE(state) {
    state.resellerCatalog = defaultStates.resellerCatalogDefaultState();
  },
  SET_CUSTOMER_PRODUCTS(state, products) {
    state.customerProducts = products;
  },
};
