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
            'Upload',
            'ServiceProvidersBriefItemStorage',
            'ServiceProvidersStorage'
        ];

        constructor(
            private $scope: IDashboardScope,
            private $location: ng.ILocationService,
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
            }   
        }
    }

}
