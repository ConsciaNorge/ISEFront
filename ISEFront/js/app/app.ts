/// <reference path='../_all.ts' />

/**
 * The main TodoMVC app module.
 *
 * @type {angular.Module}
 */
module dashboard {
    'use strict';

    var dashboardApp = angular.module('dashboard', [])
        .controller('SidePanelController', SidePanelController)
        .controller('DashboardController', DashboardController)
        .service('DashboardSidePanelItemStorage', DashboardSidePanelItemStorage);
}
