namespace dashboard {
    export interface IDashboardSidePanelItemStorage {
        get(): ng.IPromise<DashboardSidePanelItem[]>;
        put(items: DashboardSidePanelItem[]);
    }
}