export default {
  HANDLE_ROUTE_CHANGE(state, router) {
    const currentRoute = {
      friendlyName: router.currentRoute.meta.friendlyName || '',
      path: router.currentRoute.path,
    };
    const routeIndex = state.routeHistory.findIndex(route => route.path === currentRoute.path);
    if (routeIndex > -1) {
      state.routeHistory.splice(routeIndex, state.routeHistory.length - routeIndex);
      // state.routeHistory.splice(routeIndex, 1); only remove one in history
    }
    state.routeHistory.push(currentRoute);
    if (state.routeHistory.length > 3) {
      state.routeHistory.splice(0, state.routeHistory.length - 3);
    }
  },
};
