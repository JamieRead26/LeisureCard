var offersHomeController = angular.module('offersHomeController', []);

offersHomeController.controller('OffersHomeController', function ($scope, slideshow) {
    $scope.global.slideshow = slideshow.offershome;
});
