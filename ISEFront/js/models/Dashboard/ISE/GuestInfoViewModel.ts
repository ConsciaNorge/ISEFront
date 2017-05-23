namespace dashboard.ISE {
    'use strict';

    export interface GuestInfoViewModel {
        userName: string;
        firstName: string;
        lastName: string;
        emailAddress: string;
        password: string;
        creationTime: Date;
        enabled: boolean;
        notificationLanguage: string;
        smsServiceProvider: string;
        phoneNumber: string;
        company: string;
    }
}

