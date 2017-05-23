var dashboard;
(function (dashboard) {
    'use strict';
    var dashboardApp = angular.module('dashboard', [
        'ui.bootstrap',
        'nonStringSelect',
        'dateTimeSandbox',
        'ngFileUpload'
    ])
        .controller('BankIDController', dashboard.BankIDController)
        .controller('CiscoIseController', dashboard.CiscoIseController)
        .controller('DashboardController', dashboard.DashboardController)
        .controller('FirstRunController', dashboard.FirstRunController)
        .controller('IdentityProviderController', dashboard.IdentityProviderController)
        .controller('ServiceProvidersController', dashboard.ServiceProvidersController)
        .controller('SidePanelController', dashboard.SidePanelController)
        .service('BankIDStorage', dashboard.BankIDStorage)
        .service('CiscoIseStorage', dashboard.CiscoIseStorage)
        .service('DashboardSidePanelItemStorage', dashboard.DashboardSidePanelItemStorage)
        .service('IdentityProviderStorage', dashboard.IdentityProviderStorage)
        .service('ServiceProvidersBriefItemStorage', dashboard.ServiceProvidersBriefItemStorage)
        .service('ServiceProvidersStorage', dashboard.ServiceProvidersStorage);
})(dashboard || (dashboard = {}));
//# sourceMappingURL=app.js.map