/// <reference path='../../../_all.ts' />

module dashboard {
    'use strict';

    export class FirstRunController {
        private identityProviderName: string;
        private identityProviderDescription: string;
        private idPCertificateParameters: CertificateGenerationDetailsViewModel;

        private notBeforePicker: BootstrapV3DatetimePicker.Datetimepicker;
        private notAfterPicker: BootstrapV3DatetimePicker.Datetimepicker;


        private viewInitialConfiguration: boolean = true;

        public static $inject = [
            '$scope',
            '$location',
            'IdentityProviderStorage'
        ];

        constructor(
            private $scope: IDashboardScope,
            private $location: ng.ILocationService,
            private identityProviderStorage: IdentityProviderStorage
        ) {

            // for its methods to be accessible from view / HTML
            $scope.frVm = this;

            this.identityProviderName = $location.protocol() + "://" + $location.host() + '/';
            this.identityProviderDescription = 'ISEFront SAML Identity Provider'

            let subjectName: string = "CN=" + this.$location.host();
            let issuerName: string = "CN=ca." + this.$location.host();

            this.idPCertificateParameters = new CertificateGenerationDetailsViewModel(
                subjectName,
                issuerName
            );

            this.notBeforePicker = $('#notBeforePicker').datetimepicker().data('DateTimePicker');
            this.notBeforePicker.date(this.idPCertificateParameters.notBefore);

            this.notAfterPicker = $('#notAfterPicker').datetimepicker().data('DateTimePicker');
            this.notAfterPicker.date(this.idPCertificateParameters.notAfter);

            this.idPCertificateParameters.privateKeyPassword = this.generateRandomPassword();
        }

        private randomizeCertificatePassword(): void {
            this.idPCertificateParameters.privateKeyPassword = this.generateRandomPassword();
        }

        private saveAndContinue(): void {
            // Todo : Form validation

            this.idPCertificateParameters.notBefore = this.notBeforePicker.date().toDate();
            this.idPCertificateParameters.notAfter = this.notAfterPicker.date().toDate();

            this.identityProviderStorage.putInitialConfiguration(new InitialConfigurationViewModel(
                    this.identityProviderName,
                    this.identityProviderDescription,
                    this.idPCertificateParameters
                ))
                .then(result => {
                    window.open('../dashboard', '_self');
                })
        }

        private generateRandomPassword(length: number = 32) : string {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/.+- _";

            for (var i = 0; i < 32; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        }
    }
}
