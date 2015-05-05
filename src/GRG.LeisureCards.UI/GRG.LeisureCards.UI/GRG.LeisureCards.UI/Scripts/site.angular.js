
var app = angular.module('leisure', ['ngResource']);
var root = '//localhost:1623';
/*
function myCrtl($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";
}
myCrtl.$inject = ['$scope'];
app.controller('myCtrl', myCrtl);
*/
    
/*
app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/list', {
        controller: 'listController',
        templateUrl: 'list.html'
    })
}]);
*/

app.factory("LeisureCard", function ($resource) {
    return $resource(root + '/RedLetter/Get/:id');
});

app.controller('CardController', function ($scope, LeisureCard) {
    LeisureCard.get({ id: 6056 }, function (data) {
        $scope.card = data;
        debugger;
    });
});


