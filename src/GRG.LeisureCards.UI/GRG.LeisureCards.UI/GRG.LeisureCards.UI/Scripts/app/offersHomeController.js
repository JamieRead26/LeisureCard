var offersHomeController = angular.module('offersHomeController', []);

offersHomeController.controller('OffersHomeController', function ($scope, slideshow) {
    $scope.global.bodyclass = 'offers-home';
    $scope.global.slideshow = slideshow.offershome;
});
