/// <reference path='../../_all.ts' />

module dashboard {
    'use strict';

    export class CertificateGenerationDetailsViewModel {
        constructor(
            public subjectName: string = "",
            public issuerName: string = "",
            public notBefore: Date = new Date(),
            public notAfter: Date = moment().add('years', 2).toDate(),
            public privateKeyPassword: string = '',
            public keyStrength: number = 2048,
            public hashAlgorithm: string = 'SHA512withRSA'
        ) { }
    }
}
