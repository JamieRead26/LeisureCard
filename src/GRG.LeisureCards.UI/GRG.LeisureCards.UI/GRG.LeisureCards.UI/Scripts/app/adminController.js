var adminController = angular.module('adminController', []);

adminController.factory('GetCardActivationHistory', function ($resource, config) {
    return $resource(config.apiUrl + '/GetCardActivationHistory/:from/:to');
});
adminController.factory('GetSelectedOfferHistory', function ($resource, config) {
    // Gets Offers Claimed
    return $resource(config.apiUrl + '/GetSelectedOfferHistory/:from/:to');
});
adminController.factory('GetLoginHistory', function ($resource, config) {
    // Gets Card Usage
    return $resource(config.apiUrl + '/GetLoginHistory/:from/:to');
});

adminController.controller('AdminController', function ($scope, GetLoginHistory) {

    $scope.global.slideshow = [];

    
    GetLoginHistory.get({ from: '1/1/2000', to: '12/12/2015' }, function (data) {
        debugger;
    });

});