/// <reference path='../../../_all.ts' />

module dashboard {
    'use strict';

    export interface IEditServiceProviderModalParams {
        name: string;
        onOk(): void;
        onCancel(): void;
    }

    export interface IModalService {
        showConfirmDialog(title: string, bodyText: string, onOk: () => void, onCancel?: () => void): void;
    }

    export class ServiceProvidersController {

        private serviceProvidersBrief: ServiceProviderBriefItem[];
        private viewServiceProviders: boolean = false;
        private viewEditServiceProvider: boolean = false;
        private viewCreateServiceProvider: boolean = false;

        private editVm: EditServiceProviderViewModel;

        private newSp: CreateNewServiceProviderViewModel;
        private spMetadataFile: File;

        public static $inject = [
            '$scope',
            '$location',
            //"$uibModal",
            'Upload',
            'ServiceProvidersBriefItemStorage',
            'ServiceProvidersStorage'
        ];

        constructor(
            private $scope: IDashboardScope,
            private $location: ng.ILocationService,
            //private $uibModal: angular.ui.bootstrap.IModalService,
            private Upload: angular.angularFileUpload.IUploadService,
            private serviceProvidersBriefItemStorage: ServiceProvidersBriefItemStorage,
            private serviceProvidersStorage: ServiceProvidersStorage
        ) {

            // for its methods to be accessible from view / HTML
            $scope.spVm = this;

            serviceProvidersBriefItemStorage.get()
                .then(
                    items => this.setServiceProviders(items)
                )
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                );
        }

        private setServiceProviders(items: ServiceProviderBriefItem[])
        {
            this.serviceProvidersBrief = items;
            this.viewServiceProviders = true;
        }

        public EditServiceProvider(id: number): void {

            this.viewServiceProviders = false;

            this.editVm = new EditServiceProviderViewModel(
                this.serviceProvidersBrief.filter(item => item.id == id)[0]
            );

            this.serviceProvidersStorage.getServiceProvider(id)
                .then(
                    item => this.editVm.details = item
                )
                .catch(
                    reason => console.log('failed to get items - ' + reason)
                )

            this.viewEditServiceProvider = true;
        }

        private createNewServiceProviderClicked(): void {
            this.viewServiceProviders = false;

            this.newSp = new CreateNewServiceProviderViewModel();
            this.spMetadataFile = null;

            this.viewCreateServiceProvider = true;
        }

        private uploadSpMetadata(file: File, errorFiles: string[]) {
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
        }

        //public ShowModal(name:string):void {
        //    //let modalParams: IEditServiceProviderModalParams = {
            //    name: name,
            //    onOk: function() {},
            //    onCancel: function() {}
            //};

            //let modalInstance = this.$uibModal.open({
            //    animation: true,
            //    templateUrl: "/js/controllers/dashboard/serviceproviders/EditServiceProviderDialog.html",
            //    controller: EditServiceProviderDialogController,
            //    controllerAs: "dialogController",
            //    size: null, // default size
            //    resolve: {
            //        modalParams: () => modalParams
            //    }
            //});

    }

}
