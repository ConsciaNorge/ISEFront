namespace dashboard {
    'use strict';

    export class ServiceProviderBriefItem {
        constructor(
            public id: number,
            public name: string,
            public description: string,
            public wantAuthnRequestSigned: boolean,
            public signSAMLResponse: boolean,
            public signAssertion: boolean,
            public encryptAssertion: boolean
        ) { }
    }
}
