namespace dashboard {
    'use strict';

    export class CiscoIseController {
        public serverSettings: IseServerSettingsViewModel;
        public portals: ISE.PortalBriefViewModel[];
        public currentPortal: ISE.PortalViewModel;
        public guestUsers: ISE.GuestUserBriefViewModel[];

        public static $inject = [
            '$scope',
            '$location',
            'CiscoIseStorage'
        ];

        constructor(
            private $scope: IDashboardScope,
            private $location: ng.ILocationService,
            private ciscoIseStorage: CiscoIseStorage
        ) {
            $scope.ciVm = this;

            this.getServerSettings();
            this.getPortals();
        }

        private save(): void {
            this.ciscoIseStorage.putIseServerSettings(this.serverSettings)
                .then(items => {
                    this.serverSettings = items;
                    this.getPortals();
                })
                .catch(
                    reason => console.log('failed to put settings - ' + reason)
                );
        }

        private getServerSettings(): void {
            this.ciscoIseStorage.getIseServerSettings()
                .then(
                    items => this.serverSettings = items
                )
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                );
        }

        private getPortals(): void {
            this.ciscoIseStorage.getIsePortals()
                .then(
                    portals => this.portals = portals
                )
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                );
        }

        private getGuestUsersForPortal(portalId: string): void {
            this.ciscoIseStorage.getIseGuestUsers(portalId)
                .then(
                    guestUsers => this.guestUsers = guestUsers
                )
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                );
        }

        private portalSelected(id: string): void {
            this.ciscoIseStorage.getIsePortal(id)
                .then(
                    currentPortal => this.currentPortal = currentPortal
                )
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                );

            this.getGuestUsersForPortal(id);
        }
    }
}
