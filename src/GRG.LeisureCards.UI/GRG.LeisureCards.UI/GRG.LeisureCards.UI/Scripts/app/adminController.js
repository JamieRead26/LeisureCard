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

adminController.controller('AdminController', function ($scope, GetLoginHistory, GetCardActivationHistory, GetSelectedOfferHistory) {

    $scope.global.slideshow = [];
    $scope.reports_card_activation = [];
    $scope.reports_offers_claimed = [];
    $scope.reports_card_usage = [];
    $scope.report_type = 'card_activation';

    $scope.get_report = function (d) {
        
        $scope.reports_card_activation = [];
        $scope.reports_offers_claimed = [];
        $scope.reports_card_usage = [];
        $scope.report_error = null;

        var reg = new RegExp(/^(\d{1,2})-(\d{1,2})-(\d{4})$/);
        if (!reg.test($scope.from_date) || !reg.test($scope.to_date)) {
            return $scope.report_error = 'From and To dates must match format dd-mm-yyyy';
        }

        var search_data = { from: $scope.from_date, to: $scope.to_date }

        if ($scope.report_type == 'card_activation') {
            GetCardActivationHistory.get(search_data, function (data) {
                $scope.reports_card_activation = data.$values;

                if ($scope.reports_card_activation.length == 0) {
                    return $scope.report_error = 'No results to show.';
                }
            });
        }
        else if($scope.report_type == 'offers_claimed'){
            GetSelectedOfferHistory.get(search_data, function (data) {

                $scope.reports_offers_claimed = data.$values;

                if ($scope.reports_offers_claimed.length == 0) {
                    return $scope.report_error = 'No results to show.';
                }
            });
        }
        else if ($scope.report_type == 'card_usage') {
            GetLoginHistory.get(search_data, function (data) {

                $scope.reports_card_usage = data.$values;

                if ($scope.reports_card_usage.length == 0) {
                    return $scope.report_error = 'No results to show.';
                }
            });
        }

    };

});