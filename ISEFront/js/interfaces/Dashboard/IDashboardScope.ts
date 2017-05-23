namespace dashboard {
    export interface IDashboardScope extends ng.IScope {
        vm: DashboardController;
        sidePanelVm: SidePanelController;
        spVm: ServiceProvidersController;
        idpVm: IdentityProviderController;
        frVm: FirstRunController;
        ciVm: CiscoIseController;
        bidVm: BankIDController;
    }
}