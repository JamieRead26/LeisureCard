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

adminController.factory('GetCardGenerationHistory', function ($resource, config) {
    return $resource(config.apiUrl + '/Reports/GetCardGenerationHistory/:from/:to');
});

adminController.factory('GetAllCardNumbers', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/GetAllCardNumbers');
});

adminController.factory('LeisureCardUpdate', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/Update/:cardNumberOrRef/:expiryDate/:renewalDate/:suspended');
});

// Red letter data import
adminController.factory('UploadRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/RedLetter/');
});
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

// generate cards
adminController.factory('GenerateCards', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/GenerateCards/:reference/:numberOfCards/:renewalPeriodMonths');
});

adminController.controller('AdminDataImportController', function ($scope,
    GetLastGoodRedLetter, GetLastBadRedLetter,
    GetLastGoodTwoForOne, GetLastBadTwoForOne,
    GetLastGoodLeisureCard, GetLastBadLeisureCard,
    fileUpload, UploadRedLetter, config) {

    $scope.global.bodyclass = 'admin';

    $scope.imports = [];
    $scope.apiUrl = config.apiUrl;

    $scope.files = {};
    $scope.files.redletter = {};
    $scope.files.file241 = {};

    $scope.file_error = '';
    $scope.file_success = '';

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

    $scope.refresh = function () {
        window.location.reload();
    };

    $scope.uploadRedLetter = function () {
        UploadRedLetter.save();
        return $scope.file_success = 'The import is running, please refresh page after a few minutes to see results.';
    }

    $scope.uploadFile = function (key) {
        var file = '';
        var path = '';

        $scope.file_error = '';
        $scope.file_success = '';

        if(key == '241'){
            file = $scope.files.file241;
            path = '/DataImport/TwoForOne/';
        }

        if(!key || !path || !file){
            return console.error('Missing path, key or file.');
        }

        console.log('file is ' + JSON.stringify(file));
        var uploadUrl = config.apiUrl + path;
        fileUpload.uploadFileToUrl(file, uploadUrl, function(data){
            
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

var valid_iso_date = function (date) {

    if(!date){
        return false;
    }

    if (date.indexOf('Z') == -1) {
        d = new Date(date)
        date = d.toISOString();
    }
    
    var reg = new RegExp(/\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([{0,3}-][0-2]\d:[0-5]\d|Z)/);
    return reg.test(date);
}

adminController.controller('AdminCardGenerateController', function ($scope, GenerateCards) {

    $scope.reference = '';
    $scope.num_cards = '';
    $scope.duration = '';

    $scope.reset = function () {
        $scope.reference = '';
        $scope.num_cards = '';
        $scope.duration = '';
    }

    $scope.submit = function () {
        $scope.cardgenerate_error = null;
        $scope.cardgenerate_success = null;

        if (!$scope.reference || !$scope.num_cards || !$scope.duration) {
            return $scope.cardgenerate_error = 'All fields are required.';
        }
        
        var postData = {
            reference: $scope.reference,
            numberOfCards: $scope.num_cards,
            renewalPeriodMonths: $scope.duration
        };

        GenerateCards.get(postData, function (data) {
            if (data.Success) {
                return $scope.cardgenerate_success = 'Cards "' + data.CardGenerationLog.Ref + '" generated successfully.'
            }
            return $scope.cardgenerate_error = data.ErrorMessage;
        });

    }

});

adminController.controller('AdminUpdateCardController', function ($scope, $filter, GetAllCardNumbers, LeisureCardUpdate) {
    
    $scope.cards = {};
    $scope.card_numbers = [];
    $scope.suspended = false;

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
        $scope.suspended = false;
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
           
            $scope.suspended = card.Suspended;
        }
        else {
            $scope.cardNumber = cardNumber;
        }
    }

    $scope.submit = function () {

        $scope.cardupdate_error = null;

        if (!valid_iso_date($scope.expiryDate) || !valid_iso_date($scope.renewalDate)) {
            return $scope.cardupdate_error = 'Expiry and Renewal dates must match format dd-mm-yyyy';
        }

        var postData = {
            cardNumberOrRef: $scope.cardNumber,
            expiryDate: $filter('date')($scope.expiryDate, "yyyy-MM-dd"),
            renewalDate: $filter('date')($scope.renewalDate, "yyyy-MM-dd"),
            suspended: $scope.suspended
        };
        
        LeisureCardUpdate.get(postData, function (data) {

            $scope.expiryDate = '';
            $scope.renewalDate = '';
            $scope.suspended = false;

            return $scope.cardupdate_success = data.CardsUpdated + ' card/s updated successfully.';

        });
    }

});

adminController.controller('AdminReportController', function ($scope, $filter,
    GetLoginHistory, GetCardActivationHistory, GetSelectedOfferHistory, GetCardGenerationHistory, GetAllCardNumbers) {

    $scope.global.slideshow = [];
    $scope.reports_card_activation = [];
    $scope.reports_offers_claimed = [];
    $scope.reports_card_usage = [];
    $scope.generation_history = [];
    $scope.all_card_numbers = [];
    $scope.show_print = false;
    $scope.hide_dates = false;
    $scope.report_type = 'card_activation';

    $scope.reportChange = function () {
        $scope.hide_dates = $scope.report_type == 'all_card_numbers';
    }

    $scope.get_report = function () {

        $scope.reports_card_activation = [];
        $scope.reports_offers_claimed = [];
        $scope.reports_card_usage = [];
        $scope.generation_history = [];
        $scope.all_card_numbers = [];
        $scope.report_error = null;

        var no_results_check = function (array) {
            if (array.length == 0) {
                $scope.show_print = false;
                return $scope.report_error = 'No results to show.';
            } else {
                $scope.show_print = true;
            }
        };

        if ($scope.report_type != 'all_card_numbers') {
            if (!valid_iso_date($scope.from_date) || !valid_iso_date($scope.to_date)) {
                return $scope.report_error = 'From and To dates must match format dd-mm-yyyy';
            }
            var search_data = {
                from: $filter('date')($scope.from_date, "yyyy-MM-dd"),
                to: $filter('date')($scope.to_date, "yyyy-MM-dd")
            };
        }
 
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
        else if ($scope.report_type == 'generation_history') {

            GetCardGenerationHistory.get(search_data, function (data) {
                $scope.generation_history = data.$values;
                no_results_check($scope.generation_history);
            });

        }
        else if ($scope.report_type == 'all_card_numbers') {

            GetAllCardNumbers.get(function (data) {
                $scope.all_card_numbers = data.$values;
                no_results_check($scope.all_card_numbers);
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

    $scope.getGenerationHistoryHeader = function () { return ['Reference', 'Generated Date'] };
    $scope.getGenerationHistoryReport = function () {
        var report = $scope.generation_history;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Reference': report[i].Ref,
                'Generated Date': report[i].GeneratedDate
            });
        }
        return data;
    };

    $scope.getAllCardsHeader = function () {
        var data = ['Code', 
            'Reference', 
            'Renewal Period Months', 
            'Suspended',
            'Expiry Date', 
            'Renewal Date', 
            'Registration Date', 
            'Uploaded Date'];
        return data;
    };
    $scope.getAllCardsReport = function () {
        var report = $scope.all_card_numbers;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Code': report[i].Code,
                'Reference': report[i].Reference,
                'Renewal Period Months': report[i].RenewalPeriodMonths,
                'Suspended': report[i].Suspended,
                'Expiry Date': report[i].ExpiryDate,
                'Renewal Date': report[i].RenewalDate,
                'Registration Date': report[i].RegistrationDate,
                'Uploaded Date': report[i].UploadedDate
            });
        }
        return data;
    };

    $scope.print = function () {
        window.print();
    };

});