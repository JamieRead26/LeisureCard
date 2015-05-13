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

adminController.controller('AdminController', function ($scope, GetLoginHistory) {

    $scope.global.slideshow = [];

    var to = new Date(2000, 1, 1);
    var day = to.getDate();
    var month = to.getMonth();
    var year = to.getFullYear();
  
    GetLoginHistory.get({ from: '1/1/2000 00:00:00', to: '1/1/2001 00:00:00' }, function (data) {
        debugger;
    });

});