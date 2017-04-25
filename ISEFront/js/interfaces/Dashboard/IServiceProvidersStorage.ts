/// <reference path='../../_all.ts' />

module dashboard {
    export interface IServiceProvidersStorage {
        getServiceProvider(id: number): ng.IPromise<ServiceProviderDetailItem>;
    }
}