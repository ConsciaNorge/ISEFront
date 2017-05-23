namespace dashboard {
    'use strict';

    export class IdentityProviderStorage implements IIdentityProviderStorage {
        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.httpService = $http;
        }

        getIdentityProviderDetails(): ng.IPromise<IdentityProviderDetailItem> {
            return this.httpService.get('/api/identityprovider').then(response => response.data);
        }

        postGenerateCertificate(details: CertificateGenerationDetailsViewModel): ng.IPromise<X509CertificateItem> {
            return this.httpService.post('/api/identityprovider/generatecertificate', details).then(response => response.data);
        }

        putInitialConfiguration (data: InitialConfigurationViewModel): ng.IPromise<string> {
            return this.httpService.put('/api/identityprovider', data).then(response => response.data);
        }
    }
}