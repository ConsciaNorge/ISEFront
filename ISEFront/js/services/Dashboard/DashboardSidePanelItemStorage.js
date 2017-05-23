var dashboard;
(function (dashboard) {
    'use strict';
    /**
     * Services that persists and retrieves TODOs from localStorage.
     */
    var DashboardSidePanelItemStorage = (function () {
        function DashboardSidePanelItemStorage($http) {
            this.STORAGE_ID = 'dashboard-sidepanel-items';
            this.httpService = $http;
        }
        DashboardSidePanelItemStorage.prototype.get = function () {
            return this.httpService.get('/api/dashboard/sidepanelitems').then(function (response) { return response.data; });
        };
        DashboardSidePanelItemStorage.prototype.put = function (todos) {
            localStorage.setItem(this.STORAGE_ID, JSON.stringify(todos));
        };
        return DashboardSidePanelItemStorage;
    }());
    dashboard.DashboardSidePanelItemStorage = DashboardSidePanelItemStorage;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=DashboardSidePanelItemStorage.js.map