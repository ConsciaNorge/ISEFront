namespace dashboard {
    'use strict';

    export class BankIDController {
        settings: BankIDSettingViewModel;

        public static $inject = [
            '$scope',
            '$location',
            'BankIDStorage'
        ];

        constructor(
            private $scope: IDashboardScope,
            private $location: ng.ILocationService,
            private bankIDStorage: BankIDStorage
        ) {
            $scope.bidVm = this;

            this.getSettings();
        }

        private getSettings(): void {
            this.bankIDStorage.getBankIDSettings()
                .then(items => {
                    this.settings = items;
                })
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                );
        }

        private save(): void {
            this.bankIDStorage.putBankIDSettings(this.settings)
                .then(items => {
                    this.settings = items;
                })
                .catch(
                reason => console.log('failed to get items - ' + reason)
                );
        }
    }
}
