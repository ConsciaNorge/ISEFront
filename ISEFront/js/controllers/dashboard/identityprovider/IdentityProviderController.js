/// <reference path='../../../_all.ts' />
// Using date time picker from typescript : http://www.byteblocks.com/Post/Use-Datetime-Picker-With-Typescript
// Date time picker angular directive : https://gist.github.com/eugenekgn/f00c4d764430642dca4b
var dashboard;
(function (dashboard) {
    'use strict';
    var IdentityProviderController = (function () {
        function IdentityProviderController($scope, $location, identityProviderStorage) {
            var _this = this;
            this.$scope = $scope;
            this.$location = $location;
            this.identityProviderStorage = identityProviderStorage;
            this.viewIdentityProviderDetails = false;
            this.viewInstalledCertificate = false;
            this.viewCertificateGeneratorPane = false;
            // for its methods to be accessible from view / HTML
            $scope.idpVm = this;
            // Can use angular directive?
            this.notBeforePicker = $('#notBeforePicker').datetimepicker().data('DateTimePicker');
            this.notAfterPicker = $('#notAfterPicker').datetimepicker().data('DateTimePicker');
            identityProviderStorage.getIdentityProviderDetails()
                .then(function (result) {
                _this.idpDetails = result;
                _this.viewIdentityProviderDetails = true;
                if (result.certificates != null && result.certificates.length > 0) {
                    _this.viewInstalledCertificate = true;
                }
            })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        }
        IdentityProviderController.prototype.baseUrl = function () {
            return this.$location.protocol() + '://' + this.$location.host();
        };
        IdentityProviderController.prototype.generateNewCertificate = function () {
            var subjectName = "CN=" + this.$location.host();
            var issuerName = "CN=ca." + this.$location.host();
            this.certificateGeneration = new dashboard.CertificateGenerationDetailsViewModel(subjectName, issuerName);
            this.notBeforePicker.date(this.certificateGeneration.notBefore);
            this.notAfterPicker.date(this.certificateGeneration.notAfter);
            this.viewInstalledCertificate = false;
            this.viewCertificateGeneratorPane = true;
        };
        IdentityProviderController.prototype.randomizeCertificatePassword = function () {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/.+- _";
            for (var i = 0; i < 32; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            this.certificateGeneration.privateKeyPassword = text;
        };
        IdentityProviderController.prototype.generateCertificateClicked = function () {
            var _this = this;
            this.certificateGeneration.notBefore = this.notBeforePicker.date().toDate();
            this.certificateGeneration.notAfter = this.notAfterPicker.date().toDate();
            // TODO : Add form validation code here. Even better, add form validation before activating the button
            var confirmed = confirm("You are about to generate a new certificate for the identity provider, are you sure?");
            if (confirmed) {
                this.identityProviderStorage.postGenerateCertificate(this.certificateGeneration)
                    .then(function (result) {
                    _this.certificateGeneration = null;
                    _this.viewCertificateGeneratorPane = false;
                    //this.idpDetails.certificate = result;
                    _this.viewIdentityProviderDetails = true;
                    if (result != null) {
                        _this.viewInstalledCertificate = true;
                    }
                })
                    .catch(function (reason) { return console.log('failed to generate certificate - ' + reason); });
            }
        };
        IdentityProviderController.prototype.cancelGenerateCertificate = function () {
            // TODO : Add code to handle discarding changes if there were any.
            this.certificateGeneration = null;
            this.viewCertificateGeneratorPane = false;
            this.viewIdentityProviderDetails = true;
            if (this.idpDetails.certificates != null && this.idpDetails.certificates.length > 0) {
                this.viewInstalledCertificate = true;
            }
        };
        IdentityProviderController.prototype.saveChanges = function () {
            // TODO : Implement save changes. This would require implementing another model
            alert('Function not implemented yet');
        };
        return IdentityProviderController;
    }());
    IdentityProviderController.$inject = [
        '$scope',
        '$location',
        'IdentityProviderStorage'
    ];
    dashboard.IdentityProviderController = IdentityProviderController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=IdentityProviderController.js.map