/// <reference path='../../_all.ts' />

module dashboard {
    export interface IIdentityProviderStorage {
        getIdentityProviderDetails(): ng.IPromise<IdentityProviderDetailItem>;

        postGenerateCertificate(details: CertificateGenerationDetailsViewModel): ng.IPromise<X509CertificateItem>;

        putInitialConfiguration (data: InitialConfigurationViewModel): ng.IPromise<string>;
}
}