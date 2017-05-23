namespace dashboard.ISE {
    'use strict';

    export interface GuestUserViewModel extends GuestUserBriefViewModel {
        status: string;
        sponsorUserName: string;
        sponsorUserId: string;
        guestInfo: GuestInfoViewModel;
        guestAccessInfo: GuestAccessInfoViewModel;
        // TODO : Figure out how to handle this dynamic type properly in TypeScript
        //customFields: IDictionary<string, object>;
        description: string;
        guestType: string;
        reasonForVisit: string;
        statusReason: string;
        portalId: string;
    }
}

