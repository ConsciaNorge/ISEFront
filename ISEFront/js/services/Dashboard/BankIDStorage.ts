namespace dashboard {
    'use strict';

    export class BankIDStorage implements IBankIDStorage {
        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.httpService = $http;
        }

        getBankIDSettings(): ng.IPromise<BankIDSettingViewModel> {
            return this.httpService.get('/api/bankid/settings').then(response => response.data);
        }

        putBankIDSettings(settings: BankIDSettingViewModel): ng.IPromise<BankIDSettingViewModel> {
            return this.httpService.put('/api/bankid/settings', settings).then(response => response.data);
        }
    }
}