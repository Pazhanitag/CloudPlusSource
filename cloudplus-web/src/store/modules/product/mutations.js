export default {
  SET_PRODUCTS(state, products) {
    state.products = products;
  },
  SET_PRODUCT_CATEGORIES(state, categories) {
    state.categories = categories;
  },
  SET_PRODUCT(state, product) {
    state.selectedProduct = product;
  },
};
