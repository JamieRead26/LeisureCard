var offers241Controller = angular.module('offers241Controller', []);

offers241Controller.factory('Offer241GetAll', function ($resource, config) {
    return $resource(config.apiUrl + '/TwoForOne/GetAll');
});

offers241Controller.controller('offers241Controller', function ($scope, Offer241GetAll) {

    $scope.global.slideshow = [
        {
            img: 'http://placehold.it/1140x300',
            link: 'http://google.co.uk'
        }
    ];

    Offer241GetAll.get(function (data) {
        $scope.offers = data.$values;
    });

    $scope.keyword = '';
    $scope.submit = function () {
        if ($scope.keyword) {



        } else {
            $scope.errors = 'You must provide a location.';
        }

    };
});
