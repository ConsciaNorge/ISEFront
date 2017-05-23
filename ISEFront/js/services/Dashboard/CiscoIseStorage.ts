namespace dashboard {
    'use strict';

    export class CiscoIseStorage implements ICiscoIseStorage {
        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.httpService = $http;
        }

        getIseServerSettings(): ng.IPromise<IseServerSettingsViewModel> {
            return this.httpService.get('/api/ciscoise/iseserversettings').then(response => response.data);
        }

        putIseServerSettings(settings: IseServerSettingsViewModel): ng.IPromise<IseServerSettingsViewModel> {
            return this.httpService.put('/api/ciscoise/iseserversettings', settings).then(response => response.data);
        }

        getIsePortals(): ng.IPromise<ISE.PortalBriefViewModel[]> {
            return this.httpService.get('/api/ciscoise/portals').then(response => response.data);
        }

        getIsePortal(id: string): ng.IPromise<ISE.PortalViewModel> {
            return this.httpService.get('/api/ciscoise/portal/' + id).then(response => response.data);
        }

        getIseGuestUsers(portalId: string): ng.IPromise<ISE.GuestUserBriefViewModel[]> {
            return this.httpService.get('/api/ciscoise/portal/' + portalId + '/guestusers').then(response => response.data);
        }
    }
}