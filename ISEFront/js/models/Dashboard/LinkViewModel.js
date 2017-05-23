var dashboard;
(function (dashboard) {
    'use strict';
    var LinkViewModel = (function () {
        function LinkViewModel(rel, href, type) {
            this.rel = rel;
            this.href = href;
            this.type = type;
        }
        return LinkViewModel;
    }());
    dashboard.LinkViewModel = LinkViewModel;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=LinkViewModel.js.map