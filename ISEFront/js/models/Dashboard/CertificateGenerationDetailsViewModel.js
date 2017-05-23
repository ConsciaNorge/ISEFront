var dashboard;
(function (dashboard) {
    'use strict';
    var CertificateGenerationDetailsViewModel = (function () {
        function CertificateGenerationDetailsViewModel(subjectName, issuerName, notBefore, notAfter, privateKeyPassword, keyStrength, hashAlgorithm) {
            if (subjectName === void 0) { subjectName = ""; }
            if (issuerName === void 0) { issuerName = ""; }
            if (notBefore === void 0) { notBefore = new Date(); }
            if (notAfter === void 0) { notAfter = moment().add('years', 2).toDate(); }
            if (privateKeyPassword === void 0) { privateKeyPassword = ''; }
            if (keyStrength === void 0) { keyStrength = 2048; }
            if (hashAlgorithm === void 0) { hashAlgorithm = 'SHA512withRSA'; }
            this.subjectName = subjectName;
            this.issuerName = issuerName;
            this.notBefore = notBefore;
            this.notAfter = notAfter;
            this.privateKeyPassword = privateKeyPassword;
            this.keyStrength = keyStrength;
            this.hashAlgorithm = hashAlgorithm;
        }
        return CertificateGenerationDetailsViewModel;
    }());
    dashboard.CertificateGenerationDetailsViewModel = CertificateGenerationDetailsViewModel;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=CertificateGenerationDetailsViewModel.js.map