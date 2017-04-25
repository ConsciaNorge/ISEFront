/// <reference path='../../../_all.ts' />
var dashboard;
(function (dashboard) {
    'use strict';
    var ServiceProvidersController = (function () {
        function ServiceProvidersController($scope, $location, 
            //private $uibModal: angular.ui.bootstrap.IModalService,
            Upload, serviceProvidersBriefItemStorage, serviceProvidersStorage) {
            var _this = this;
            this.$scope = $scope;
            this.$location = $location;
            this.Upload = Upload;
            this.serviceProvidersBriefItemStorage = serviceProvidersBriefItemStorage;
            this.serviceProvidersStorage = serviceProvidersStorage;
            this.viewServiceProviders = false;
            this.viewEditServiceProvider = false;
            this.viewCreateServiceProvider = false;
            // for its methods to be accessible from view / HTML
            $scope.spVm = this;
            serviceProvidersBriefItemStorage.get()
                .then(function (items) { return _this.setServiceProviders(items); })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
        }
        ServiceProvidersController.prototype.setServiceProviders = function (items) {
            this.serviceProvidersBrief = items;
            this.viewServiceProviders = true;
        };
        ServiceProvidersController.prototype.EditServiceProvider = function (id) {
            var _this = this;
            this.viewServiceProviders = false;
            this.editVm = new dashboard.EditServiceProviderViewModel(this.serviceProvidersBrief.filter(function (item) { return item.id == id; })[0]);
            this.serviceProvidersStorage.getServiceProvider(id)
                .then(function (item) { return _this.editVm.details = item; })
                .catch(function (reason) { return console.log('failed to get items - ' + reason); });
            this.viewEditServiceProvider = true;
        };
        ServiceProvidersController.prototype.createNewServiceProviderClicked = function () {
            this.viewServiceProviders = false;
            this.newSp = new dashboard.CreateNewServiceProviderViewModel();
            this.spMetadataFile = null;
            this.viewCreateServiceProvider = true;
        };
        ServiceProvidersController.prototype.uploadSpMetadata = function (file, errorFiles) {
            this.spMetadataFile = file;
            if (file) {
                this.Upload.upload({
                    url: '/api/serviceprovider/spmetadataupload',
                    method: 'POST',
                    file: file
                }).progress(function (evt) {
                    //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    alert('Uploaded successfully ' + file.name);
                }).error(function (err) {
                    alert('Error occured during upload');
                });
                //file.upload = Upload.upload({
                //    url: 'https://angular-file-upload-cors-srv.appspot.com/upload',
                //    data: { file: file }
                //});
                //file.upload.then(function (response) {
                //    $timeout(function () {
                //        file.result = response.data;
                //    });
                //}, function (response) {
                //    if (response.status > 0)
                //        $scope.errorMsg = response.status + ': ' + response.data;
                //}, function (evt) {
                //    file.progress = Math.min(100, parseInt(100.0 *
                //        evt.loaded / evt.total));
                //});
                ;
            }
        };
        return ServiceProvidersController;
    }());
    ServiceProvidersController.$inject = [
        '$scope',
        '$location',
        //"$uibModal",
        'Upload',
        'ServiceProvidersBriefItemStorage',
        'ServiceProvidersStorage'
    ];
    dashboard.ServiceProvidersController = ServiceProvidersController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=ServiceProvidersController.js.map