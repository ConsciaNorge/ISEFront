/// <reference path='../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    var CreateNewServiceProviderViewModel = (function () {
        function CreateNewServiceProviderViewModel(name, description, certificate) {
            this.name = name;
            this.description = description;
            this.certificate = certificate;
        }
        return CreateNewServiceProviderViewModel;
    }());
    dashboard.CreateNewServiceProviderViewModel = CreateNewServiceProviderViewModel;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=CreateNewServiceProviderModel.js.map