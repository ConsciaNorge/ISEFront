namespace dashboard.ISE {
    'use strict';

    export interface GuestAccessInfoViewModel {
        validDays: number;
        fromDate: Date;
        toDate: Date;
        location: string;
        ssid: string;
        groupTag: string;
    }
}

