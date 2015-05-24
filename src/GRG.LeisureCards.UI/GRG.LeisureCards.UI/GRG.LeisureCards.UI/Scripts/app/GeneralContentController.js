var generalContentController = angular.module('generalContentController', []);

generalContentController.controller('TermsController', function ($scope, slideshow) {
    $scope.global.bodyclass = 'terms';
    $scope.global.slideshow = [];
});