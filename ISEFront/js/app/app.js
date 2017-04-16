/// <reference path='../_all.ts' />
/**
 * The main TodoMVC app module.
 *
 * @type {angular.Module}
 */
var dashboard;
(function (dashboard) {
    'use strict';
    var dashboardApp = angular.module('dashboard', [])
        .controller('SidePanelController', dashboard.SidePanelController)
        .controller('DashboardController', dashboard.DashboardController)
        .service('DashboardSidePanelItemStorage', dashboard.DashboardSidePanelItemStorage);
})(dashboard || (dashboard = {}));
//# sourceMappingURL=app.js.map