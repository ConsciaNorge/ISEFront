var dashboard;
(function (dashboard) {
    'use strict';
    var IdentityProviderStorage = (function () {
        function IdentityProviderStorage($http) {
            this.httpService = $http;
        }
        IdentityProviderStorage.prototype.getIdentityProviderDetails = function () {
            return this.httpService.get('/api/identityprovider').then(function (response) { return response.data; });
        };
        IdentityProviderStorage.prototype.postGenerateCertificate = function (details) {
            return this.httpService.post('/api/identityprovider/generatecertificate', details).then(function (response) { return response.data; });
        };
        IdentityProviderStorage.prototype.putInitialConfiguration = function (data) {
            return this.httpService.put('/api/identityprovider', data).then(function (response) { return response.data; });
        };
        return IdentityProviderStorage;
    }());
    dashboard.IdentityProviderStorage = IdentityProviderStorage;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=IdentityProviderStorage.js.map