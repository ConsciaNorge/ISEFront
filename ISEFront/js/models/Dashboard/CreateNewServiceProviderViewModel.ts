/// <reference path='../../_all.ts' />

module dashboard {
    'use strict';

    export class CreateNewServiceProviderViewModel {
        constructor(
            public name: string = '',
            public description: string = ''
        ) { }
    }
}
