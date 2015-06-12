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

adminController.factory('GetCardNumbersForUpdate', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/GetCardNumbersForUpdate');
});

adminController.factory('LeisureCardUpdate', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/Update/:cardNumberOrRef/:renewalDate/:suspended');
});

adminController.factory('GetTwoForOneMissingLocation', function ($resource, config) {
    return $resource(config.apiUrl + '/Reports/GetTwoForOneMissingLocation/');
});

// Red letter data import
adminController.factory('ProcessRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/ProcessRedLetter/');
});
adminController.factory('RetrieveRedLetter', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/RetrieveRedLetter/');
});
adminController.factory('GetRedLetterImportJournal', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/GetRedLetterImportJournal');
});

// Two for one data import
adminController.factory('Process241', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/Process241/');
});
adminController.factory('Get241ImportJournal', function ($resource, config) {
    return $resource(config.apiUrl + '/DataImport/Get241ImportJournal');
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
    GetRedLetterImportJournal,
    Get241ImportJournal,
    GetLastGoodLeisureCard, GetLastBadLeisureCard,
    fileUpload, ProcessRedLetter, RetrieveRedLetter, Process241, config) {

    $scope.global.bodyclass = 'admin';

    $scope.imports = [];
    $scope.apiUrl = config.apiUrl;

    $scope.files = {};
    $scope.files.fileRedLetter = {};
    $scope.files.file241 = {};

    $scope.file_error = '';
    $scope.file_success = '';

    var push_current_import = function (data, default_key) {

        var _push = function (data) {

            // Find any existing import records of the same UploadKey
            var i = -1;
            $.grep($scope.imports, function(item, index) {
                if (item.UploadKey == default_key)
                    i = index;
            });

            // If any existing records were found of the same UploadKey then remove them
            if (i != -1)
                $scope.imports.splice(i, 1);

            // Now add the data to the list of records.
            return $scope.imports.push(data);
        };

        if (!data.LastRun) {
            // default 
            return _push({
                LastRun: null,
                UploadKey: default_key,
                Success: null,
                Message: null,
                FileName: null,
                Status: null
            });
        }

        return _push(data);
    };

    GetRedLetterImportJournal.get(function (data) {
        push_current_import(data, 'RedLetter');
    });

    Get241ImportJournal.get(function (data) {
        push_current_import(data, '241');
    });

    $scope.refresh = function () {
        window.location.reload();
    };

    $scope.processRedLetter = function () {
        $scope.file_success = 'The import is running, please refresh page after a few minutes to see results.';
        ProcessRedLetter.get(function (data) {
            push_current_import(data, 'RedLetter');
            $scope.file_success = 'The import is complete. The result is shown in the table above.';
            setTimeout(function() { $scope.$apply('file_success = \'\''); }, 5000);
        });
    }

    $scope.retrieveRedLetter = function () {
        $scope.file_success = 'The file retrieval is running, please refresh page after a few minutes to see results.';
        RetrieveRedLetter.get(function (data) {
           
        });
    }

    $scope.process241 = function () {
        $scope.file_success = 'The import is running, please refresh page after a few minutes to see results.';
        Process241.get(function (data) {
            push_current_import(data, '241');
            $scope.file_success = 'The import is complete. The result is shown in the table above.';
            setTimeout(function () { $scope.$apply('file_success = \'\''); }, 5000);
        });
    }

    $scope.uploadFile = function (key) {
        var file = $scope.files['file' + key];
        var path = '/DataImport/Upload' + key + '/';

        $scope.file_error = '';
        $scope.file_success = '';

        if(!key || !path || !file){
            return console.error('Missing path, key or file.');
        }

        //console.log('file is ' + JSON.stringify(file));
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

adminController.controller('AdminCardGenerateController', function ($scope, $rootScope, GenerateCards) {

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
                $rootScope.$broadcast('cards_generated');
                return $scope.cardgenerate_success = 'Cards "' + data.CardGenerationLog.Ref + '" generated successfully.'
            }
            return $scope.cardgenerate_error = data.ErrorMessage;
        });

    }

});

adminController.controller('AdminUpdateCardController', function ($scope, $rootScope, $filter, GetCardNumbersForUpdate, LeisureCardUpdate) {
    
    $scope.cards = {};
    $scope.card_numbers = [];
    $scope.suspended = false;

    var refreshCardsForUpdate = function() {
        GetCardNumbersForUpdate.get(function (data) {
            $scope.card_numbers.length = 0; // Empty array

            var cards = data.$values;

            // pushes cards to key values
            for (var i = 0; i < cards.length; i++) {
                var code = cards[i].Code;

                $scope.cards[code] = cards[i];
                $scope.card_numbers.push(code);
            }
        });
    }

    refreshCardsForUpdate();

    $rootScope.$on('cards_generated', refreshCardsForUpdate);
    
    $scope.reset = function () {
        $scope.suspended = false;
        $scope.expiryDate = '';
        $scope.renewalDate = '';
        $scope.cardNumber = '';
        $scope.status = '';
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
            $scope.status = card.Status || '';
        }
        else {
            $scope.cardNumber = cardNumber;
        }
    }

    $scope.submit = function () {

        $scope.cardupdate_error = null;

        if (!valid_iso_date($scope.renewalDate)) {
            return $scope.cardupdate_error = 'Renewal date must match format dd-mm-yyyy';
        }

        var postData = {
            cardNumberOrRef: $scope.cardNumber,
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

adminController.controller('AdminReportController', function ($scope, $filter, GetLoginHistory, GetCardActivationHistory, GetSelectedOfferHistory, GetCardGenerationHistory, GetAllCardNumbers, GetTwoForOneMissingLocation) {

    $scope.global.slideshow = [];
    $scope.reports_card_activation = [];
    $scope.reports_offers_claimed = [];
    $scope.reports_card_usage = [];
    $scope.reports_generation_history = [];
    $scope.reports_urn_report = [];
    $scope.reports_missing_location = [];
    $scope.hide_dates = false;
    $scope.report_type = 'card_activation';

    $scope.reportChange = function () {
        $scope.hide_dates = $scope.report_type == 'urn_report' || $scope.report_type == 'missing_location';
    }

    var validateResultsReturned = function (array) {
    	if (array.length == 0) {
    		$scope.report_error = 'No results to show.';
    		return false;
    	}

    	return true;
    };

    $scope.get_report = function () {

    	$scope.reports_card_activation = [];
        $scope.reports_offers_claimed = [];
        $scope.reports_card_usage = [];
        $scope.reports_generation_history = [];
        $scope.reports_urn_report = [];
        $scope.reports_missing_location = [];
        $scope.report_error = null;

	    var request = {}, action;

	    if ($scope.report_type != 'urn_report' && $scope.report_type != 'missing_location') {
	        request.from = $filter('date')($scope.from_date, "yyyy-MM-dd") || '2000-01-01';
	        request.to = $filter('date')($scope.to_date, "yyyy-MM-dd") || '3000-01-01';
        }
 
	    switch ($scope.report_type) {
	    	case 'card_activation':
	    		action = GetCardActivationHistory;
	    		break;
	    	case 'offers_claimed':
	    		action = GetSelectedOfferHistory;
	    		break;
	    	case 'card_usage':
	    		action = GetLoginHistory;
	    		break;
	    	case 'generation_history':
	    		action = GetCardGenerationHistory;
	    		break;
	    	case 'urn_report':
	    		action = GetAllCardNumbers;
	    		break;
	        case 'missing_location':
	            action = GetTwoForOneMissingLocation;
	            break;
	    	default :
			    throw 'Unexpected report type ' + $scope.report_type;
	    }

	    action.get(request, function(data) {
	    	$scope['reports_' + $scope.report_type] = data.$values;
		    if (validateResultsReturned($scope['reports_' + $scope.report_type])) {
			    setTimeout(function() { $('#download_reports_' + $scope.report_type).click(); });
		    }
	    });
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
        var report = $scope.reports_generation_history;
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
            'Status',
            'Expiry Date', 
            'Renewal Date', 
            'Registration Date', 
            'Uploaded Date'];
        return data;
    };
    $scope.getAllCardsReport = function () {
        var report = $scope.reports_urn_report;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Code': report[i].Code,
                'Reference': report[i].Reference,
                'Renewal Period Months': report[i].RenewalPeriodMonths,
                'Status': report[i].Status,
                'Expiry Date': report[i].ExpiryDate,
                'Renewal Date': report[i].RenewalDate,
                'Registration Date': report[i].RegistrationDate,
                'Uploaded Date': report[i].UploadedDate
            });
        }
        return data;
    };

    $scope.get241MissingLocationHeader = function () {
        var data = ['Id',
            'OutletName',
            'Address1',
            'Address2',
            'TownCity',
            'County',
            'PostCode'];
        return data;
    };
    $scope.get241MissingLocationReport = function () {
        var report = $scope.reports_missing_location;
        var data = [];
        for (var i = 0; i < report.length; i++) {
            data.push({
                'Id': report[i].Id,
                'OutletName': report[i].OutletName,
                'Address1': report[i].Address1,
                'Address2': report[i].Address2,
                'TownCity': report[i].TownCity,
                'County': report[i].County,
                'PostCode': report[i].PostCode
            });
        }
        return data;
    };

    $scope.print = function () {
        window.print();
    };

});