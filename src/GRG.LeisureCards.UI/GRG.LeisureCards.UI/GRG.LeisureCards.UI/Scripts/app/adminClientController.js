var adminClientController = angular.module('adminClientController', []);

adminClientController.factory('GetTenantByKey', function ($resource, config) {
    return $resource(config.apiUrl + '/Tenant/Get/:key');
});

// add urns
adminClientController.factory('GetNewUrnsImportJournal', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetNewUrnsImportJournal');
});
adminClientController.factory('ProcessNewUrnsData', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/ProcessNewUrnsData/:cardDurationMonths');
});
adminClientController.factory('UploadNewUrns', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/UploadNewUrns/:tenantKey');
});

// deactivated urns
adminClientController.factory('UploadDeactivateUrns', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/UploadDeactivateUrns/:tenantKey');
});
adminClientController.factory('ProcessDeactivateUrnsData', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/ProcessDeactivateUrnsData/');
});
adminClientController.factory('GetDeactivateUrnsImportJournal', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetDeactivateUrnsImportJournal');
});

adminClientController.controller('AdminClientUrnsController', function ($scope, $location,
    GetNewUrnsImportJournal, ProcessNewUrnsData, UploadNewUrns,
    UploadDeactivateUrns, ProcessDeactivateUrnsData, GetDeactivateUrnsImportJournal,
    PushImportToArray) {

    $scope.imports = [];

    GetNewUrnsImportJournal.get(function (data) {
        PushImportToArray.push($scope, data, 'Add');
    });

    GetDeactivateUrnsImportJournal.get(function (data) {
        PushImportToArray.push($scope, data, 'Deactivate');
    });

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