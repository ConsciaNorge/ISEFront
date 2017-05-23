var dashboard;
(function (dashboard) {
    'use strict';
    var IdentityProviderDetailItem = (function () {
        function IdentityProviderDetailItem(name, description, certificates) {
            this.name = name;
            this.description = description;
            this.certificates = certificates;
        }
        return IdentityProviderDetailItem;
    }());
    dashboard.IdentityProviderDetailItem = IdentityProviderDetailItem;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=IdentityProviderDetailItem.js.map