/// <reference path='../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    var X509CertificateItem = (function () {
        function X509CertificateItem(subjectName, issuer, issuerName, notBefore, notAfter, publicKeyAlgorithm, subjectPublicKey) {
            this.subjectName = subjectName;
            this.issuer = issuer;
            this.issuerName = issuerName;
            this.notBefore = notBefore;
            this.notAfter = notAfter;
            this.publicKeyAlgorithm = publicKeyAlgorithm;
            this.subjectPublicKey = subjectPublicKey;
        }
        return X509CertificateItem;
    }());
    dashboard.X509CertificateItem = X509CertificateItem;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=X509CertificateItem.js.map