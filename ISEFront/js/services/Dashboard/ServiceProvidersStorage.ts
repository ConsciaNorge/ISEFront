/// <reference path='../../_all.ts' />

module dashboard {
    'use strict';

    export class ServiceProvidersStorage implements IServiceProvidersStorage {
        httpService: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.httpService = $http;
        }


        getServiceProvider(id: number): ng.IPromise<ServiceProviderDetailItem> {
            return this.httpService.get('/api/serviceprovider/' + id.toString()).then(response => response.data);
        }
    }
}