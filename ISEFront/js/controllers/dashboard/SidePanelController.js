/// <reference path='../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    /**
     * The main controller for the app. The controller:
     * - retrieves and persists the model via the todoStorage service
     * - exposes the model to the template and provides event handlers
     */
    var SidePanelController = (function () {
        // dependencies are injected via AngularJS $injector
        // controller's name is registered in Application.ts and specified from ng-controller attribute in index.html
        function SidePanelController($scope, $location, sidePanelItemStorage) {
            var _this = this;
            this.$scope = $scope;
            this.$location = $location;
            this.sidePanelItemStorage = sidePanelItemStorage;
            sidePanelItemStorage.get().then(function (items) { return _this.sidePanelItems = items; }).catch(function (reason) { return console.log('failed to get items - ' + reason); });
            // 'vm' stands for 'view model'. We're adding a reference to the controller to the scope
            // for its methods to be accessible from view / HTML
            $scope.sidePanelVm = this;
        }
        return SidePanelController;
    }());
    // $inject annotation.
    // It provides $injector with information about dependencies to be injected into constructor
    // it is better to have it close to the constructor, because the parameters must match in count and type.
    // See http://docs.angularjs.org/guide/di
    SidePanelController.$inject = [
        '$scope',
        '$location',
        'DashboardSidePanelItemStorage'
    ];
    dashboard.SidePanelController = SidePanelController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=SidePanelController.js.map