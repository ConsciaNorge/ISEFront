/// <reference path='../../_all.ts' />

module dashboard {
    export interface IServiceProvidersBriefStorage {
        get(): ng.IPromise<ServiceProviderBriefItem[]>;
    }
}