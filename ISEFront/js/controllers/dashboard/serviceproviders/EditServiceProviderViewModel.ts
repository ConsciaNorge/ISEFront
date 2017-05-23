namespace dashboard {
    'use strict';

    export class EditServiceProviderViewModel {
        public details: ServiceProviderDetailItem;

        constructor(
            public brief: ServiceProviderBriefItem,
        ) {
        }
    }
}