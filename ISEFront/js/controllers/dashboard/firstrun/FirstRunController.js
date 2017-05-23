var dashboard;
(function (dashboard) {
    'use strict';
    var FirstRunController = (function () {
        function FirstRunController($scope, $location, identityProviderStorage) {
            this.$scope = $scope;
            this.$location = $location;
            this.identityProviderStorage = identityProviderStorage;
            this.viewInitialConfiguration = true;
            // for its methods to be accessible from view / HTML
            $scope.frVm = this;
            this.identityProviderName = $location.protocol() + "://" + $location.host() + '/';
            this.identityProviderDescription = 'ISEFront SAML Identity Provider';
            var subjectName = "CN=" + this.$location.host();
            var issuerName = "CN=ca." + this.$location.host();
            this.idPCertificateParameters = new dashboard.CertificateGenerationDetailsViewModel(subjectName, issuerName);
            this.notBeforePicker = $('#notBeforePicker').datetimepicker().data('DateTimePicker');
            this.notBeforePicker.date(this.idPCertificateParameters.notBefore);
            this.notAfterPicker = $('#notAfterPicker').datetimepicker().data('DateTimePicker');
            this.notAfterPicker.date(this.idPCertificateParameters.notAfter);
            this.idPCertificateParameters.privateKeyPassword = this.generateRandomPassword();
        }
        FirstRunController.prototype.randomizeCertificatePassword = function () {
            this.idPCertificateParameters.privateKeyPassword = this.generateRandomPassword();
        };
        FirstRunController.prototype.saveAndContinue = function () {
            // Todo : Form validation
            this.idPCertificateParameters.notBefore = this.notBeforePicker.date().toDate();
            this.idPCertificateParameters.notAfter = this.notAfterPicker.date().toDate();
            this.identityProviderStorage.putInitialConfiguration(new dashboard.InitialConfigurationViewModel(this.identityProviderName, this.identityProviderDescription, this.idPCertificateParameters))
                .then(function (result) {
                window.open('../dashboard', '_self');
            });
        };
        FirstRunController.prototype.generateRandomPassword = function (length) {
            if (length === void 0) { length = 32; }
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/.+- _";
            for (var i = 0; i < 32; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            return text;
        };
        return FirstRunController;
    }());
    FirstRunController.$inject = [
        '$scope',
        '$location',
        'IdentityProviderStorage'
    ];
    dashboard.FirstRunController = FirstRunController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=FirstRunController.js.map