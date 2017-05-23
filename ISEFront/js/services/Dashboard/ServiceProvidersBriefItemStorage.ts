namespace dashboard {
    'use strict';

    export class ServiceProvidersBriefItemStorage implements IServiceProvidersBriefStorage {
        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.httpService = $http;
        }

        get(): ng.IPromise<ServiceProviderBriefItem[]> {
            return this.httpService.get('/api/serviceproviders').then(response => response.data);
        }
    }
}