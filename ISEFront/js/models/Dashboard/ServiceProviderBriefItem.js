/// <reference path='../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    var ServiceProviderBriefItem = (function () {
        function ServiceProviderBriefItem(id, name, description, wantAuthnRequestSigned, signSAMLResponse, signAssertion, encryptAssertion) {
            this.id = id;
            this.name = name;
            this.description = description;
            this.wantAuthnRequestSigned = wantAuthnRequestSigned;
            this.signSAMLResponse = signSAMLResponse;
            this.signAssertion = signAssertion;
            this.encryptAssertion = encryptAssertion;
        }
        return ServiceProviderBriefItem;
    }());
    dashboard.ServiceProviderBriefItem = ServiceProviderBriefItem;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=ServiceProviderBriefItem.js.map