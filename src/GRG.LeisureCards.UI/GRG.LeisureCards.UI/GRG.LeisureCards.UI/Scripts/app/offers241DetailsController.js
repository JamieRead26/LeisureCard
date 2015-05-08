var offers241DetailsController = angular.module('offers241DetailsController', []);

offers241DetailsController.controller('offers241DetailsController', function ($scope, $routeParams) {
    $scope.id = $routeParams.id;
});
