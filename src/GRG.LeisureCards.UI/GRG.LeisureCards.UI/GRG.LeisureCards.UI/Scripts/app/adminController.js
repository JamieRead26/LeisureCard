var adminController = angular.module('adminController', []);

adminController.factory('GetCardActivationHistory', function ($resource, config) {
    return $resource(config.apiUrl + '/Reports/GetCardActivationHistory/:from/:to');
});
adminController.factory('GetSelectedOfferHistory', function ($resource, config) {
    // Gets Offers Claimed
    return $resource(config.apiUrl + '/Reports/GetSelectedOfferHistory/:from/:to');
});
adminController.factory('GetLoginHistory', function ($resource, config) {
    // Gets Card Usage
    return $resource(config.apiUrl + '/Reports/GetLoginHistory/:from/:to');
});

// Red letter data import
adminController.factory('GetLastGoodRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastGoodRedLetterImportJournal');
});
adminController.factory('GetLastBadRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastBadTwoForOneImportJournal');
});

// Two for one data import
adminController.factory('GetLastGoodTwoForOne', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastGoodTwoForOneImportJournal');
});
adminController.factory('GetLastBadTwoForOne', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastBadTwoForOneImportJournal');
});

adminController.controller('AdminDataImportController', function ($scope,
    GetLastGoodRedLetter, GetLastBadRedLetter,
    GetLastGoodTwoForOne, GetLastBadTwoForOne,
    fileUpload, config) {

    $scope.imports = [];

    $scope.files = {};
    $scope.files.redletter = {};
    $scope.files.file241 = {};
    $scope.file_errors = '';

    var push_current_import = function (good, bad) {

        var _push = function (data) {
            return $scope.imports.push(data);
        };

        if (!bad.ImportedDateTime) {
            // use last good data
            return _push(good);
        }
        if (!good.ImportedDateTime) {
            // use last bad data
            return _push(bad);
        }
        
        if (good.ImportedDateTime >= bad.ImportedDateTime) {
            // last good data more recent than bad data
            return _push(good);
        } else {
            return _push(bad);
        }
    };

    GetLastGoodRedLetter.get(function (data) {
        var good_data = data;

        GetLastBadRedLetter.get(function (data) {
            var bad_data = data;
            push_current_import(good_data, bad_data);
        });
    });

    GetLastGoodTwoForOne.get(function (data) {
        var good_data = data;

        GetLastBadTwoForOne.get(function (data) {
            var bad_data = data;
            push_current_import(good_data, bad_data);
        });
    });
    
    $scope.refresh = function () {
        window.location.reload();
    };

    $scope.uploadFile = function (key) {

        var file = '';
        var path = '';
        if(key == 'Red Letter'){
            file = $scope.files.redletter;
            path = '/DataImport/RedLetter/';
        }
        else if(key == '2-4-1'){
            file = $scope.files.file241;
            path = '/DataImport/TwoForOne/';
        }

        if(!key || !path || !file){
            return console.error('Missing path, key or file.');
        }

        console.log('file is ' + JSON.stringify(file));
        var uploadUrl = config.apiUrl + path;
        fileUpload.uploadFileToUrl(file, uploadUrl);
    };

});

adminController.controller('AdminReportController', function ($scope,
    GetLoginHistory, GetCardActivationHistory, GetSelectedOfferHistory) {

    $scope.global.slideshow = [];
    $scope.reports_card_activation = [];
    $scope.reports_offers_claimed = [];
    $scope.reports_card_usage = [];
    $scope.report_type = 'card_activation';

    $scope.get_report = function () {

        $scope.reports_card_activation = [];
        $scope.reports_offers_claimed = [];
        $scope.reports_card_usage = [];
        $scope.report_error = null;

        var no_results_check = function (array) {
            if (array.length == 0) { return $scope.report_error = 'No results to show.'; }
        };

        var reg = new RegExp(/^(\d{1,2})-(\d{1,2})-(\d{4})$/);
        if (!reg.test($scope.from_date) || !reg.test($scope.to_date)) {
            return $scope.report_error = 'From and To dates must match format dd-mm-yyyy';
        }

        var search_data = { from: $scope.from_date, to: $scope.to_date }

        if ($scope.report_type == 'card_activation') {

            GetCardActivationHistory.get(search_data, function (data) {
                $scope.reports_card_activation = data.$values;
                no_results_check($scope.reports_card_activation);
            });

        }
        else if ($scope.report_type == 'offers_claimed') {

            GetSelectedOfferHistory.get(search_data, function (data) {
                $scope.reports_offers_claimed = data.$values;
                no_results_check($scope.reports_offers_claimed);
            });

        }
        else if ($scope.report_type == 'card_usage') {

            GetLoginHistory.get(search_data, function (data) {
                $scope.reports_card_usage = data.$values;
                no_results_check($scope.reports_card_usage);
            });

        }
    };

    $scope.getCardActivationHeader = function () { return ['Card Number', 'Activation Date'] };
    $scope.getCardActivationReport = function () {
        var report = $scope.reports_card_activation;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Card Code': report[i].Code,
                'Reg Date': report[i].RegistrationDate
            });
        }
        return data;
    };

    $scope.getClaimedOffersHeader = function () { return ['Card Number', 'Offer Name', 'Claimed Date'] };
    $scope.getClaimedOffersReport = function () {
        var report = $scope.reports_offers_claimed;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Card Code': report[i].LeisureCardCode,
                'Offer Title': report[i].OfferTitle,
                'Claimed Date': report[i].SelectedDateTime
            });
        }
        return data;
    };

    $scope.getCardUsageHeader = function () { return ['Card Number', 'Login Date and Time'] };
    $scope.getCardUsageReport = function () {
        var report = $scope.reports_card_usage;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Card Code': report[i].LeisureCardCode,
                'Login Date': report[i].LoginDateTime
            });
        }
        return data;
    };

    $scope.print = function () {
        window.print();
    };

});