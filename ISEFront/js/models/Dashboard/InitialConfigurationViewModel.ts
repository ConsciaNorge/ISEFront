namespace dashboard {
    'use strict';

    export class InitialConfigurationViewModel {
        constructor(
            public idpName: string,
            public idpDescription: string,
            public idpCertificateParameters: CertificateGenerationDetailsViewModel
        ) { }
    }
}
