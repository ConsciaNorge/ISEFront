namespace dashboard {
    export interface IServiceProvidersStorage {
        getServiceProvider(id: number): ng.IPromise<ServiceProviderDetailItem>;
    }
}