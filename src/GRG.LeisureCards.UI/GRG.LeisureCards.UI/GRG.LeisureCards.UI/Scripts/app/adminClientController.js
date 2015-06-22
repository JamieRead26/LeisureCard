var adminClientController = angular.module('adminClientController', []);

adminClientController.controller('AdminClientDetailsController', function ($scope, config, $http) {
    $scope.global.bodyclass = 'admin';

    $scope.tenants = [];
    $scope.currentTenant = {};
    $scope.selectTenant = function () {
        var t = $scope.currentTenant;
        if(t){
            $scope.tenantKey = t.Key;
            $scope.tenantName = t.Name;
            $scope.tenantActive = t.Active;
            $scope.domain = t.Domain;
            $scope.comments = t.Comments;
            $scope.memberLoginPopupDisplayed = t.MemberLoginPopupDisplayed;
            $scope.memberLoginPopupMandatory = t.MemberLoginPopupMandatory;
            $scope.urnCount = t.UrnCount;

            $scope.ftpUsername = t.FtpUsername;
            $scope.ftpAddress = t.FtpServer;
            $scope.ftpPassword = t.FtpPassword;
        }
    }

    $http.get(config.apiUrl + '/Tenant/GetAll').then(function (r) {
        $scope.tenants = r.data.$values;
    });

    $scope.saveTenant = function () {

        $scope.tenant_success = null;
        $scope.tenant_error = null;

        if ($scope.tenantKey) {
            var tenant = {
                Key: $scope.tenantKey,
                Name: $scope.tenantName,
                Active: $scope.tenantActive,
                Domain: $scope.domain,
                Comments: $scope.comments,
                MemberLoginPopupDisplayed: $scope.memberLoginPopupDisplayed,
                MemberLoginPopupMandatory: $scope.memberLoginPopupMandatory,
                FtpUsername: $scope.ftpUsername,
                FtpServer: $scope.ftpAddress,
                FtpPassword: $scope.ftpPassword
            };

            $http.post(config.apiUrl + '/Tenant/Update', tenant);
            $scope.tenant_success = 'Client updated.';
        }
        else {
            $scope.tenant_error = 'Please select a client to update.';
        }
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