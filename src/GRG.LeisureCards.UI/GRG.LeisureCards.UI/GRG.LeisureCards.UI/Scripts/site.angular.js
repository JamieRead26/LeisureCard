
var app = angular.module('leisure', []);

function myCrtl($scope) {
    $scope.firstName = "John";
    $scope.lastName = "Doe";
}
myCrtl.$inject = ['$scope'];
app.controller('myCtrl', myCrtl);
    



/*app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/example', {
        controller: 'exampleController',
        templateUrl: 'example.html'
    })
}]);*/