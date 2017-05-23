namespace dashboard {
    export interface IBankIDStorage {
        getBankIDSettings(): ng.IPromise<BankIDSettingViewModel>;

        putBankIDSettings(settings: BankIDSettingViewModel): ng.IPromise<BankIDSettingViewModel>;
    }
}