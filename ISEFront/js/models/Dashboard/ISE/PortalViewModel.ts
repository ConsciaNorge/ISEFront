namespace dashboard.ISE {
    'use strict';

    export interface PortalViewModel extends PortalBriefViewModel {
        allowSponsorToChangeOwnPassword: boolean;
        guestUserFieldList: GuestUserFieldViewModel[];
        portalType: string;
    }
}

