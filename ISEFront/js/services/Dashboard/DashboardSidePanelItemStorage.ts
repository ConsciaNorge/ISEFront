/// <reference path='../../_all.ts' />

module dashboard {
    'use strict';

    /**
     * Services that persists and retrieves TODOs from localStorage.
     */
    export class DashboardSidePanelItemStorage implements IDashboardSidePanelItemStorage {

        STORAGE_ID = 'dashboard-sidepanel-items';

        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.httpService = $http;
        }

        get(): ng.IPromise<DashboardSidePanelItem[]> {
            return this.httpService.get('/api/dashboard/sidepanelitems').then(response => response.data);
        }

        put(todos: DashboardSidePanelItem[]) {
            localStorage.setItem(this.STORAGE_ID, JSON.stringify(todos));
        }
    }
}