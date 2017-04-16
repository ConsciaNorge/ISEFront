/// <reference path='../../_all.ts' />

module dashboard {
    export interface IDashboardSidePanelItemStorage {
        get(): ng.IPromise<DashboardSidePanelItem[]>;
        put(items: DashboardSidePanelItem[]);
    }
}