namespace dashboard {
    'use strict';

    export class IdentityProviderDetailItem {
        constructor(
            public name: string,
            public description: string,
            public certificates: X509CertificateItem []
        ) { }
    }
}
