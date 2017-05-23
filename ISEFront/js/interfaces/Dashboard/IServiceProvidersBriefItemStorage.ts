namespace dashboard {
    export interface IServiceProvidersBriefStorage {
        get(): ng.IPromise<ServiceProviderBriefItem[]>;
    }
}