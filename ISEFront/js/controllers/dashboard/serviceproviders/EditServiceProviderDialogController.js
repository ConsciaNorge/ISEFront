/// <reference path='../../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    var EditServiceProviderDialogController = (function () {
        function EditServiceProviderDialogController($uibModalInstance, modalParams) {
            this.$uibModalInstance = $uibModalInstance;
            this.modalParams = modalParams;
        }
        EditServiceProviderDialogController.prototype.ok = function () {
            this.modalParams.onOk();
            this.$uibModalInstance.close();
        };
        EditServiceProviderDialogController.prototype.cancel = function () {
            if (this.modalParams.onCancel) {
                this.modalParams.onCancel();
            }
            this.$uibModalInstance.dismiss();
        };
        return EditServiceProviderDialogController;
    }());
    EditServiceProviderDialogController.$inject = [
        "$uibModalInstance",
        "modalParams"
    ];
    dashboard.EditServiceProviderDialogController = EditServiceProviderDialogController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=EditServiceProviderDialogController.js.map