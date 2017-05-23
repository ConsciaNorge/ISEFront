var dashboard;
(function (dashboard) {
    'use strict';
    var IsePortalBriefViewModel = (function () {
        function IsePortalBriefViewModel(id, name, description, link) {
            this.id = id;
            this.name = name;
            this.description = description;
            this.link = link;
        }
        return IsePortalBriefViewModel;
    }());
    dashboard.IsePortalBriefViewModel = IsePortalBriefViewModel;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=IsePortalBriefViewModel.js.map