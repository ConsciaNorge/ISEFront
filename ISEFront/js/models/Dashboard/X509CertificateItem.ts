namespace dashboard {
    'use strict';

    export class X509CertificateItem {
        constructor(
            public subjectName: string,
            public issuer: string,
            public issuerName: string,
            public notBefore: Date,
            public notAfter: Date,
            public publicKeyAlgorithm: string,
            public subjectPublicKey: string
        ) { }
    }
}
