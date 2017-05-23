var dashboard;
(function (dashboard) {
    'use strict';
    var ServiceProvidersBriefItemStorage = (function () {
        function ServiceProvidersBriefItemStorage($http) {
            this.httpService = $http;
        }
        ServiceProvidersBriefItemStorage.prototype.get = function () {
            return this.httpService.get('/api/serviceproviders').then(function (response) { return response.data; });
        };
        return ServiceProvidersBriefItemStorage;
    }());
    dashboard.ServiceProvidersBriefItemStorage = ServiceProvidersBriefItemStorage;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=ServiceProvidersBriefItemStorage.js.map