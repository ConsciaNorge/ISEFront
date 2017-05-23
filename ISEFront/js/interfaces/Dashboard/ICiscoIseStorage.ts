namespace dashboard {
    export interface ICiscoIseStorage {
        getIseServerSettings(): ng.IPromise<IseServerSettingsViewModel>;

        putIseServerSettings(settings: IseServerSettingsViewModel): ng.IPromise<IseServerSettingsViewModel>;

        getIsePortals(): ng.IPromise<ISE.PortalBriefViewModel[]>;

        getIsePortal(id: string): ng.IPromise<ISE.PortalViewModel>;

        getIseGuestUsers(portalId: string): ng.IPromise<ISE.GuestUserBriefViewModel[]>;
    }
}