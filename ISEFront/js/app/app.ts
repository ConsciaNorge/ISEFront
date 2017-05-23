namespace dashboard {
    'use strict';

    var dashboardApp = angular.module('dashboard',
        [
            'ui.bootstrap',
            'nonStringSelect',
            'dateTimeSandbox',
            'ngFileUpload'
        ])
        .controller('BankIDController', BankIDController)
        .controller('CiscoIseController', CiscoIseController)
        .controller('DashboardController', DashboardController)
        .controller('FirstRunController', FirstRunController)
        .controller('IdentityProviderController', IdentityProviderController)
        .controller('ServiceProvidersController', ServiceProvidersController)
        .controller('SidePanelController', SidePanelController)
        .service('BankIDStorage', BankIDStorage)
        .service('CiscoIseStorage', CiscoIseStorage)
        .service('DashboardSidePanelItemStorage', DashboardSidePanelItemStorage)
        .service('IdentityProviderStorage', IdentityProviderStorage)
        .service('ServiceProvidersBriefItemStorage', ServiceProvidersBriefItemStorage)
        .service('ServiceProvidersStorage', ServiceProvidersStorage)
        ;
}
