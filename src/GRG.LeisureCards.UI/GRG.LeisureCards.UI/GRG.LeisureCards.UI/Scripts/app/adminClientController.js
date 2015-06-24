var adminClientController = angular.module('adminClientController', []);

adminClientController.factory('GetTenantByKey', function ($resource, config) {
    return $resource(config.apiUrl + '/Tenant/Get/:key');
});

// add urns
adminClientController.factory('GetNewUrnsImportJournal', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetNewUrnsImportJournal');
});
adminClientController.factory('RetriveNewUrns', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/RetriveNewUrns');
});

// deactivated urns
adminClientController.factory('ProcessDeactivateUrnsData', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/ProcessDeactivateUrnsData/');
});
adminClientController.factory('GetDeactivateUrnsImportJournal', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetDeactivateUrnsImportJournal');
});
adminClientController.factory('RetriveDeactivateUrns', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/RetriveDeactivateUrns');
});

adminClientController.controller('AdminClientUrnsController', function ($scope, $location, config,
    GetNewUrnsImportJournal, ProcessDeactivateUrnsData, GetDeactivateUrnsImportJournal,
    PushImportToArray, fileUpload, RetriveDeactivateUrns, RetriveNewUrns, $http) {

    $scope.imports = [];

    $scope.files = {};
    $scope.files.NewUrns = {};
    $scope.files.DeactivatedUrns = {};

    $scope.apiUrl = config.apiUrl;

    $scope.reference = $scope.$parent.key;

    GetNewUrnsImportJournal.get(function (data) {
        PushImportToArray.push($scope, data, 'NewUrns');
    });

    GetDeactivateUrnsImportJournal.get(function (data) {
        PushImportToArray.push($scope, data, 'DeactivatedUrns');
    });

    $scope.number = {
        duration: 0
    };

    $scope.processAdd = function () {

        $scope.file_error = null;
        $scope.file_success = null;

        if ($scope.number.duration) {
            var url = config.apiUrl + '/DataImport/ProcessNewUrnsData/' + $scope.number.duration;
            $http.get(url).then(function (r) {
                PushImportToArray.push($scope, r.data, 'NewUrns');
                $scope.file_success = 'The import is complete. The result is shown in the table above.';
                setTimeout(function () { $scope.$apply('file_success = \'\''); }, 5000);
            });

            return $scope.file_success = 'The import is running, please refresh page after a few minutes to see results.';
        }

        return $scope.file_error = 'You must set the card duration.';
    }

    $scope.processDeactivate = function () {
        $scope.file_success = 'The import is running, please refresh page after a few minutes to see results.';
        ProcessDeactivateUrnsData.get(function (data) {
            PushImportToArray.push($scope, data, 'DeactivatedUrns');
            $scope.file_success = 'The import is complete. The result is shown in the table above.';
            setTimeout(function () { $scope.$apply('file_success = \'\''); }, 5000);
        });
    }

    $scope.retrieveAdd = function () {
        $scope.file_success = 'The file retrieval is running, please refresh page after a few minutes to see results.';
        RetriveNewUrns.get();
    }
    
    $scope.retrieveDeactivate = function () {
        $scope.file_success = 'The file retrieval is running, please refresh page after a few minutes to see results.';
        RetriveDeactivateUrns.get();
    }

    $scope.uploadFile = function (key) {
        var file = $scope.files[key];
        
        $scope.file_error = null;
        $scope.file_success = null;

        if (!$scope.reference) {
            return $scope.file_error = 'Please enter a client ID.';
        }

        var path = '/DataImport/Upload' + key + '/' + $scope.reference;

        if (!key || !path || !file) {
            return console.error('Missing path, key or file.');
        }

        //console.log('file is ' + JSON.stringify(file));
        var uploadUrl = config.apiUrl + path;
        fileUpload.uploadFileToUrl(file, uploadUrl, function (data) {

            if (!data.ExceptionMessage && data) {
                var imports = [];

                for (var i = 0; i < $scope.imports.length; i++) {
                    if ($scope.imports[i].UploadKey !== data.UploadKey) {
                        imports.push($scope.imports[i]);
                    } else {
                        imports.splice(i, 0, data);
                    }
                }

                $scope.imports = imports;

                if (!data.Message) {
                    return $scope.file_success = 'File uploaded successfully.';
                }

                $scope.file_error = 'Warning! file uploaded successfully but server responded: ' + data.Message;

            } else {
                $scope.file_error = 'File upload failed: ' + data.ExceptionMessage;
            }
        });
    };

});

adminClientController.controller('AdminClientDetailsController', function ($scope, $location,
    config, $http, $routeParams, GetTenantByKey) {

    $scope.global.bodyclass = 'admin';

    $scope.tenant = {};
    $scope.key = $routeParams.key;
    $scope.keyreadonly = $scope.key != 'new';

    if ($scope.key != 'new') {
        GetTenantByKey.get({ key: $scope.key }, function (data) {
            $scope.tenant = data;
        });
    }
    $scope.saveTenant = function () {

        $scope.tenant_success = null;
        $scope.tenant_error = null;

        if ($scope.tenant.Key) {
            var saveorupdate = $scope.key == 'new' ? 'Save' : 'Update';
            var url = config.apiUrl + '/Tenant/' + saveorupdate;

            $http.post(url, $scope.tenant);

            if ($scope.key == 'new') {
                $scope.key = $scope.tenant.Key;
                return $scope.tenant_success = 'New client "' + $scope.tenant.Key + '" saved.';
            }
            return $scope.tenant_success = 'Client "' + $scope.tenant.Key + '" updated.';
        }
        else {
            return $scope.tenant_error = 'Please select a client to update.';
        }
    }

    $scope.closeTenant = function () {
        if (confirm("Would you like to save your changes?")) {
            $scope.saveTenant();
        }

        $location.path('/admin');
    }

    $scope.deleteTenant = function () {
        if ($scope.tenant.Key) {
            $scope.tenant['Active'] = false;
            $scope.saveTenant();
            return $scope.tenant_success = 'Client "' + $scope.tenant.Key + '" deleted.';
        }
        return $scope.tenant_error = 'Please enter a client ID.';
    }

});

adminClientController.controller('AdminPopupReportController', function ($scope, config, $http) {
    /*
    not complete

    $scope.getHeader = function () { return ['Card Number', 'Activation Date'] };
    $scope.getReport = function () {
        
        var key = $scope.$parent.tenantKey;

        if(!key){
            return $scope.report_error = 'Please select a client.';
        }

        var url = config.apiUrl + '/Reports/GetLoginPopupReport/'+ key +'/{from}/{to}';
        $http.get(url).then(function (r) {
            $scope.report = r.data.$values;
        });

        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Card Code': report[i].Code,
                'Reg Date': report[i].RegistrationDate
            });
        }
        return data;
    };
    */

});