// Using date time picker from typescript : http://www.byteblocks.com/Post/Use-Datetime-Picker-With-Typescript
// Date time picker angular directive : https://gist.github.com/eugenekgn/f00c4d764430642dca4b

namespace dashboard {
    'use strict';

    export class IdentityProviderController {
        private idpDetails: IdentityProviderDetailItem;

        private viewIdentityProviderDetails: boolean = false;
        private viewInstalledCertificate: boolean = false;
        private viewCertificateGeneratorPane: boolean = false;

        public certificateGeneration: CertificateGenerationDetailsViewModel;

        notBeforePicker: BootstrapV3DatetimePicker.Datetimepicker;
        notAfterPicker: BootstrapV3DatetimePicker.Datetimepicker;

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
            $scope.idpVm = this;

            // Can use angular directive?
            this.notBeforePicker = $('#notBeforePicker').datetimepicker().data('DateTimePicker');
            this.notAfterPicker = $('#notAfterPicker').datetimepicker().data('DateTimePicker');

            identityProviderStorage.getIdentityProviderDetails()
                .then(result => {
                        this.idpDetails = result;
                        this.viewIdentityProviderDetails = true;
                        if (result.certificates != null && result.certificates.length > 0) {
                            this.viewInstalledCertificate = true;
                        }
                    }
                )
                .catch(
                reason => console.log('failed to get items - ' + reason)
                );
        }

        private baseUrl(): string {
            return this.$location.protocol() + '://' + this.$location.host();
        }

        private generateNewCertificate(): void {
            let subjectName: string = "CN=" + this.$location.host();
            let issuerName: string = "CN=ca." + this.$location.host();
            this.certificateGeneration = new CertificateGenerationDetailsViewModel(
                subjectName,
                issuerName
            );

            this.notBeforePicker.date(this.certificateGeneration.notBefore);
            this.notAfterPicker.date(this.certificateGeneration.notAfter);

            this.viewInstalledCertificate = false;
            this.viewCertificateGeneratorPane = true;
        }

        private randomizeCertificatePassword(): void {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/.+- _";

            for (var i = 0; i < 32; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            this.certificateGeneration.privateKeyPassword = text;
        }

        private generateCertificateClicked(): void {
            this.certificateGeneration.notBefore = this.notBeforePicker.date().toDate();
            this.certificateGeneration.notAfter = this.notAfterPicker.date().toDate();

            // TODO : Add form validation code here. Even better, add form validation before activating the button

            let confirmed: boolean = confirm("You are about to generate a new certificate for the identity provider, are you sure?");
            if (confirmed)
            {
                this.identityProviderStorage.postGenerateCertificate(this.certificateGeneration)
                    .then(result => {
                        this.certificateGeneration = null;
                        this.viewCertificateGeneratorPane = false;

                        //this.idpDetails.certificate = result;
                    
                        this.viewIdentityProviderDetails = true;
                        if (result != null) {
                            this.viewInstalledCertificate = true;
                        }
                    })
                    .catch(
                        reason => console.log('failed to generate certificate - ' + reason)
                    );
            }
        }

        private cancelGenerateCertificate(): void {
            // TODO : Add code to handle discarding changes if there were any.

            this.certificateGeneration = null;
            this.viewCertificateGeneratorPane = false;

            this.viewIdentityProviderDetails = true;
            if (this.idpDetails.certificates != null && this.idpDetails.certificates.length > 0) {
                this.viewInstalledCertificate = true;
            }
        }

        private saveChanges(): void {
            // TODO : Implement save changes. This would require implementing another model
            alert('Function not implemented yet');
        }
    }

}
