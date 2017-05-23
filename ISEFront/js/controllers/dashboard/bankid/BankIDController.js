var dashboard;
(function (dashboard) {
    'use strict';
    var BankIDController = (function () {
        function BankIDController($scope, $location, bankIDStorage) {
            this.$scope = $scope;
            this.$location = $location;
            this.bankIDStorage = bankIDStorage;
            $scope.bidVm = this;
            this.getSettings();
        }
        BankIDController.prototype.getSettings = function () {
            var _this = this;
            this.bankIDStorage.getBankIDSettings()
                .then(function (items) {
                _this.settings = items;
            })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        };
        BankIDController.prototype.save = function () {
            var _this = this;
            this.bankIDStorage.putBankIDSettings(this.settings)
                .then(function (items) {
                _this.settings = items;
            })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        };
        return BankIDController;
    }());
    BankIDController.$inject = [
        '$scope',
        '$location',
        'BankIDStorage'
    ];
    dashboard.BankIDController = BankIDController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=BankIDController.js.map