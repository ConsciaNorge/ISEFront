var dashboard;
(function (dashboard) {
    'use strict';
    var CiscoIseController = (function () {
        function CiscoIseController($scope, $location, ciscoIseStorage) {
            this.$scope = $scope;
            this.$location = $location;
            this.ciscoIseStorage = ciscoIseStorage;
            $scope.ciVm = this;
            this.getServerSettings();
            this.getPortals();
        }
        CiscoIseController.prototype.save = function () {
            var _this = this;
            this.ciscoIseStorage.putIseServerSettings(this.serverSettings)
                .then(function (items) {
                _this.serverSettings = items;
                _this.getPortals();
            })
                .catch(function (reason) { return console.log('failed to put settings - ' + reason); });
        };
        CiscoIseController.prototype.getServerSettings = function () {
            var _this = this;
            this.ciscoIseStorage.getIseServerSettings()
                .then(function (items) { return _this.serverSettings = items; })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        };
        CiscoIseController.prototype.getPortals = function () {
            var _this = this;
            this.ciscoIseStorage.getIsePortals()
                .then(function (portals) { return _this.portals = portals; })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        };
        CiscoIseController.prototype.getGuestUsersForPortal = function (portalId) {
            var _this = this;
            this.ciscoIseStorage.getIseGuestUsers(portalId)
                .then(function (guestUsers) { return _this.guestUsers = guestUsers; })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        };
        CiscoIseController.prototype.portalSelected = function (id) {
            var _this = this;
            this.ciscoIseStorage.getIsePortal(id)
                .then(function (currentPortal) { return _this.currentPortal = currentPortal; })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
            this.getGuestUsersForPortal(id);
        };
        return CiscoIseController;
    }());
    CiscoIseController.$inject = [
        '$scope',
        '$location',
        'CiscoIseStorage'
    ];
    dashboard.CiscoIseController = CiscoIseController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=CiscoIseController.js.map