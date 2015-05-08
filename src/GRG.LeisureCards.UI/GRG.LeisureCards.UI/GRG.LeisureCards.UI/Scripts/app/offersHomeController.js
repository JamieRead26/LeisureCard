var offersHomeController = angular.module('offersHomeController', []);

offersHomeController.controller('OffersHomeController', function ($scope) {
    $scope.global.slideshow = [
        {
            img: 'http://placehold.it/1140x300',
            link: 'http://google.co.uk'
        }
    ];
});
