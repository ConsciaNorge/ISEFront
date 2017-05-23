var dashboard;
(function (dashboard) {
    'use strict';
    var CiscoIseStorage = (function () {
        function CiscoIseStorage($http) {
            this.httpService = $http;
        }
        CiscoIseStorage.prototype.getIseServerSettings = function () {
            return this.httpService.get('/api/ciscoise/iseserversettings').then(function (response) { return response.data; });
        };
        CiscoIseStorage.prototype.putIseServerSettings = function (settings) {
            return this.httpService.put('/api/ciscoise/iseserversettings', settings).then(function (response) { return response.data; });
        };
        CiscoIseStorage.prototype.getIsePortals = function () {
            return this.httpService.get('/api/ciscoise/portals').then(function (response) { return response.data; });
        };
        CiscoIseStorage.prototype.getIsePortal = function (id) {
            return this.httpService.get('/api/ciscoise/portal/' + id).then(function (response) { return response.data; });
        };
        CiscoIseStorage.prototype.getIseGuestUsers = function (portalId) {
            return this.httpService.get('/api/ciscoise/portal/' + portalId + '/guestusers').then(function (response) { return response.data; });
        };
        return CiscoIseStorage;
    }());
    dashboard.CiscoIseStorage = CiscoIseStorage;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=CiscoIseStorage.js.map