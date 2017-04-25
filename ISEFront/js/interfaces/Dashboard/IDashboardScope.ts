/// <reference path='../../_all.ts' />

module dashboard {
    export interface IDashboardScope extends ng.IScope {
        vm: DashboardController;
        certificatesVm: CertificatesController;
        sidePanelVm: SidePanelController;
        spVm: ServiceProvidersController;
        idpVm: IdentityProviderController;
        frVm: FirstRunController;
    }
}