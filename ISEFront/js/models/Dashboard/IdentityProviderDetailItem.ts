/// <reference path='../../_all.ts' />

module dashboard {
    'use strict';

    export class IdentityProviderDetailItem {
        constructor(
            public name: string,
            public description: string,
            public certificates: X509CertificateItem []
        ) { }
    }
}
