namespace dashboard {
    'use strict';

    export interface BankIDSettingViewModel {
        clientId: string ;
        clientSecret: string;
        oidcBaseUrl: string;
        redirectUrl: string;
        manifestUrl: string;
        authenticationType: string;
        scope: string;
    }
}