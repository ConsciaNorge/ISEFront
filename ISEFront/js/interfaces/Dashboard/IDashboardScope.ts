/// <reference path='../../_all.ts' />

module dashboard {
    export interface IDashboardScope extends ng.IScope {
        vm: DashboardController;
        sidePanelVm: SidePanelController;
    }
}