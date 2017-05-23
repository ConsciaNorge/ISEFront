var dashboard;
(function (dashboard) {
    'use strict';
    var ServiceProvidersStorage = (function () {
        function ServiceProvidersStorage($http) {
            this.httpService = $http;
        }
        ServiceProvidersStorage.prototype.getServiceProvider = function (id) {
            return this.httpService.get('/api/serviceprovider/' + id.toString()).then(function (response) { return response.data; });
        };
        return ServiceProvidersStorage;
    }());
    dashboard.ServiceProvidersStorage = ServiceProvidersStorage;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=ServiceProvidersStorage.js.map