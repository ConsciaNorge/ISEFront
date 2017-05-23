var dashboard;
(function (dashboard) {
    'use strict';
    var CreateNewServiceProviderViewModel = (function () {
        function CreateNewServiceProviderViewModel(name, description) {
            if (name === void 0) { name = ''; }
            if (description === void 0) { description = ''; }
            this.name = name;
            this.description = description;
        }
        return CreateNewServiceProviderViewModel;
    }());
    dashboard.CreateNewServiceProviderViewModel = CreateNewServiceProviderViewModel;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=CreateNewServiceProviderViewModel.js.map