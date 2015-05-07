var offers241Controller = angular.module('offers241Controller', []);

offers241Controller.factory('Offer241Location', function ($resource) {
    return $resource(root + '/RedLetter/Keyword/:keyword');
});

offers241Controller.controller('Offer241Form', function ($scope, Offer241Location) {
    $scope.keyword = '';
    $scope.submit = function () {
        if ($scope.keyword) {

            Offer241Location.get({ keyword: $scope.keyword }, function (data) {

            });

        } else {
            $scope.errors = 'You must provide a lcoation.';
        }

    };
});