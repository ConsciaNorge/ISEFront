var dashboard;
(function (dashboard) {
    'use strict';
    var ServiceProvidersController = (function () {
        function ServiceProvidersController($scope, $location, Upload, serviceProvidersBriefItemStorage, serviceProvidersStorage) {
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
                    data: {
                        media: [file]
                    }
                }).abort().xhr(function (evt) {
                    console.log("xhr");
                }).progress(function (evt) {
                    var percent = parseInt((100.0 * evt.loaded / evt.total).toString(), 10);
                    console.log("upload progress: " + percent + "% for " + evt.config.data.media[0]);
                }).catch(function (response) {
                    console.error(response.data, response.status, response.statusText, response.headers);
                }).then(function (response) {
                    // file is uploaded successfully
                    console.log("Success!", response.data, response.status, response.headers, response.config);
                });
            }
        };
        return ServiceProvidersController;
    }());
    ServiceProvidersController.$inject = [
        '$scope',
        '$location',
        'Upload',
        'ServiceProvidersBriefItemStorage',
        'ServiceProvidersStorage'
    ];
    dashboard.ServiceProvidersController = ServiceProvidersController;
})(dashboard || (dashboard = {}));
//# sourceMappingURL=ServiceProvidersController.js.map