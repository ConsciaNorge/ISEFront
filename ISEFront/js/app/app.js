/// <reference path='../_all.ts' />
/**
 * The main TodoMVC app module.
 *
 * @type {angular.Module}
 */
var dashboard;
(function (dashboard) {
    'use strict';
    var dashboardApp = angular.module('dashboard', [
        'ui.bootstrap',
        'nonStringSelect',
        'dateTimeSandbox',
        'ngFileUpload'
    ])
        .controller('CertificatesController', dashboard.CertificatesController)
        .controller('DashboardController', dashboard.DashboardController)
        .controller('EditServiceProviderDialogController', dashboard.EditServiceProviderDialogController)
        .controller('FirstRunController', dashboard.FirstRunController)
        .controller('IdentityProviderController', dashboard.IdentityProviderController)
        .controller('ServiceProvidersController', dashboard.ServiceProvidersController)
        .controller('SidePanelController', dashboard.SidePanelController)
        .service('DashboardSidePanelItemStorage', dashboard.DashboardSidePanelItemStorage)
        .service('IdentityProviderStorage', dashboard.IdentityProviderStorage)
        .service('ServiceProvidersBriefItemStorage', dashboard.ServiceProvidersBriefItemStorage)
        .service('ServiceProvidersStorage', dashboard.ServiceProvidersStorage);
})(dashboard || (dashboard = {}));
//# sourceMappingURL=app.js.map