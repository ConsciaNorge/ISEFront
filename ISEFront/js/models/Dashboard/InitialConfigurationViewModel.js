/// <reference path='../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    var InitialConfigurationViewModel = (function () {
        function InitialConfigurationViewModel(idpName, idpDescription, idpCertificateParameters) {
            this.idpName = idpName;
            this.idpDescription = idpDescription;
            this.idpCertificateParameters = idpCertificateParameters;
        }
        return InitialConfigurationViewModel;
    }());
    dashboard.InitialConfigurationViewModel = InitialConfigurationViewModel;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=InitialConfigurationViewModel.js.map