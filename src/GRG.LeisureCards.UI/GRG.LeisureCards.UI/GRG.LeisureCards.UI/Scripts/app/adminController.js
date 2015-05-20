var adminController = angular.module('adminController', []);

adminController.factory('GetCardActivationHistory', function ($resource, config) {
    return $resource(config.apiUrl + '/Reports/GetCardActivationHistory/:from/:to');
});
// Gets Offers Claimed
adminController.factory('GetSelectedOfferHistory', function ($resource, config) {
    return $resource(config.apiUrl + '/Reports/GetSelectedOfferHistory/:from/:to');
});
// Gets Card Usage
adminController.factory('GetLoginHistory', function ($resource, config) {
    return $resource(config.apiUrl + '/Reports/GetLoginHistory/:from/:to');
});

adminController.factory('GetAllCardNumbers', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/GetAllCardNumbers');
});

adminController.factory('LeisureCardUpdate', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/Update/:cardNumber/:expiryDate/:renewalDate');
});

// Red letter data import
adminController.factory('GetLastGoodRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastGoodRedLetterImportJournal');
});
adminController.factory('GetLastBadRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastBadRedLetterImportJournal');
});

// Two for one data import
adminController.factory('GetLastGoodTwoForOne', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastGoodTwoForOneImportJournal');
});
adminController.factory('GetLastBadTwoForOne', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastBadTwoForOneImportJournal');
});

// LeisureCards data import
adminController.factory('GetLastGoodLeisureCard', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastGoodLeisureCardImportJournal');
});
adminController.factory('GetLastBadLeisureCard', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetLastBadLeisureCardImportJournal');
});

adminController.controller('AdminDataImportController', function ($scope,
    GetLastGoodRedLetter, GetLastBadRedLetter,
    GetLastGoodTwoForOne, GetLastBadTwoForOne,
    GetLastGoodLeisureCard, GetLastBadLeisureCard,
    fileUpload, config) {

    $scope.global.bodyclass = 'admin';

    $scope.imports = [];
    $scope.apiUrl = config.apiUrl;

    $scope.files = {};
    $scope.files.redletter = {};
    $scope.files.file241 = {};
    $scope.files.leisureCards = {};
    $scope.file_errors = '';

    var push_current_import = function (good, bad, default_key) {

        var _push = function (data) {
            return $scope.imports.push(data);
        };

        if (!good.ImportedDateTime && !bad.ImportedDateTime) {
            // default 
            var doc = {
                ImportedDateTime: null,
                Message: null,
                StackTrace: null,
                Success: null,
                UploadKey: default_key,
            }
            return _push(doc);
        }

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
            push_current_import(good_data, bad_data, 'RedLetter');
        });
    });

    GetLastGoodTwoForOne.get(function (data) {
        var good_data = data;
        GetLastBadTwoForOne.get(function (data) {
            var bad_data = data;
            push_current_import(good_data, bad_data, '241');
        });
    });
    
    GetLastGoodLeisureCard.get(function (data) {
        var good_data = data;
        GetLastBadLeisureCard.get(function (data) {
            var bad_data = data;
            push_current_import(good_data, bad_data, 'LeisureCards');
        });
    });

    $scope.refresh = function () {
        window.location.reload();
    };

    $scope.uploadFile = function (key) {
        var file = '';
        var path = '';

        if(key == 'RedLetter'){
            file = $scope.files.redletter;
            path = '/DataImport/RedLetter/';
        }
        else if(key == '241'){
            file = $scope.files.file241;
            path = '/DataImport/TwoForOne/';
        }
        else if (key == 'LeisureCards') {
            file = $scope.files.leisureCards;
            path = '/DataImport/LeisureCards/';
        }

        if(!key || !path || !file){
            return console.error('Missing path, key or file.');
        }

        console.log('file is ' + JSON.stringify(file));
        var uploadUrl = config.apiUrl + path;
        fileUpload.uploadFileToUrl(file, uploadUrl);
    };

});

var valid_iso_date = function (date) {
    var reg = new RegExp(/\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([{0,3}-][0-2]\d:[0-5]\d|Z)/);
    debugger;
    //return reg.test(date);
    return true;
}

adminController.controller('AdminUpdateCardController', function ($scope, $filter, GetAllCardNumbers, LeisureCardUpdate) {
    
    $scope.cards = {};
    $scope.card_numbers = [];
    $scope.status = '';

    GetAllCardNumbers.get(function (data) {
        var cards = data.$values;

        // pushes cards to key values
        for (var i = 0; i < cards.length; i++) {
            var code = cards[i].Code;

            $scope.cards[code] = cards[i];
            $scope.card_numbers.push(code);
        }
    });
    
    $scope.reset = function () {
        $scope.status = '';
        $scope.expiryDate = '';
        $scope.renewalDate = '';
        $scope.cardNumber = '';
    }

    $scope.change = function (cardNumber) {
        var card = $scope.cards[cardNumber];
        if (card) {
            $scope.cardNumber = card.Code;

            if (card.ExpiryDate) {
                $scope.expiryDate = card.ExpiryDate;
            } else {
                $scope.expiryDate = '';
            }

            if (card.RenewalDate) {
                $scope.renewalDate = card.RenewalDate;
            } else {
                $scope.renewalDate = '';
            }
            
            $scope.status = card.Status;
        }
    }

    $scope.submit = function () {

        if (!valid_iso_date($scope.expiryDate) || !valid_iso_date($scope.renewalDate)) {
            return $scope.cardupdate_error = 'Expiry and Renewal dates must match format dd-mm-yyyy';
        }

        if (!$scope.cardNumber) {
            return $scope.cardupdate_error = 'Invalid card number.';
        }

        var postData = {
            cardNumber: $scope.cardNumber,
            expiryDate: $filter('date')($scope.expiryDate, "yyyy-MM-dd"),
            renewalDate: $filter('date')($scope.renewalDate, "yyyy-MM-dd")
        };
        
        LeisureCardUpdate.get(postData, function (data) {

            var card = data;

            if (card) {

                $scope.expiryDate = card.ExpiryDate;
                $scope.renewalDate = card.RenewalDate;
                $scope.status = card.Status;

                //clean up local scope
                var code = card.Code;
                $scope.cards[code].Status = card.Status;
                $scope.cards[code].ExpiryDate = card.ExpiryDate;
                $scope.cards[code].RenewalDate = card.RenewalDate;

                return $scope.cardupdate_success = 'Card updated successfully.';
            }
            return $scope.cardupdate_success = 'An error occurred when trying to update the card.';
        });
    }

});

adminController.controller('AdminReportController', function ($scope, $filter,
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

        if (!valid_iso_date($scope.from_date) || !valid_iso_date($scope.to_date)) {
            return $scope.report_error = 'From and To dates must match format dd-mm-yyyy';
        }

        var search_data = {
            from: $filter('date')($scope.from_date, "yyyy-MM-dd"),
            to: $filter('date')($scope.to_date, "yyyy-MM-dd")
        };
       
        if ($scope.report_type == 'card_activation') {

            GetCardActivationHistory.get(search_data, function (data) {
                $scope.reports_card_activation = data.$values;
                debugger;
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