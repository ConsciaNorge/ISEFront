/// <reference path='../../../_all.ts' />

module dashboard {
    'use strict';

    export class EditServiceProviderViewModel {
        public details: ServiceProviderDetailItem;

        constructor(
            public brief: ServiceProviderBriefItem,
        ) {
        }
    }
}