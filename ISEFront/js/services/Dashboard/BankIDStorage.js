var dashboard;
(function (dashboard) {
    'use strict';
    var BankIDStorage = (function () {
        function BankIDStorage($http) {
            this.httpService = $http;
        }
        BankIDStorage.prototype.getBankIDSettings = function () {
            return this.httpService.get('/api/bankid/settings').then(function (response) { return response.data; });
        };
        BankIDStorage.prototype.putBankIDSettings = function (settings) {
            return this.httpService.put('/api/bankid/settings', settings).then(function (response) { return response.data; });
        };
        return BankIDStorage;
    }());
    dashboard.BankIDStorage = BankIDStorage;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=BankIDStorage.js.map