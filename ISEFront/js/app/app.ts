/// <reference path='../_all.ts' />

/**
 * The main TodoMVC app module.
 *
 * @type {angular.Module}
 */
module dashboard {
    'use strict';

    var dashboardApp = angular.module('dashboard',
        [
            'ui.bootstrap',
            'nonStringSelect',
            'dateTimeSandbox',
            'ngFileUpload'
        ])
        .controller('CertificatesController', CertificatesController)
        .controller('DashboardController', DashboardController)
        .controller('EditServiceProviderDialogController', EditServiceProviderDialogController)
        .controller('FirstRunController', FirstRunController)
        .controller('IdentityProviderController', IdentityProviderController)
        .controller('ServiceProvidersController', ServiceProvidersController)
        .controller('SidePanelController', SidePanelController)
        .service('DashboardSidePanelItemStorage', DashboardSidePanelItemStorage)
        .service('IdentityProviderStorage', IdentityProviderStorage)
        .service('ServiceProvidersBriefItemStorage', ServiceProvidersBriefItemStorage)
        .service('ServiceProvidersStorage', ServiceProvidersStorage)
        ;
}
