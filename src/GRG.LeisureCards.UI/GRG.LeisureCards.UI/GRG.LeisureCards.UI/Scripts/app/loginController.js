
app.factory('Login', function ($resource) {
    return $resource(root + '/LeisureCard/Login/:id');
});

app.controller('LoginController', function ($scope, $cookies, $location, Login) {
    $scope.list = [];
    $scope.card_number = '';
    $scope.submit = function () {

        if ($scope.card_number) {
            // login and store session
            Login.get({ id: $scope.card_number }, function (data) {

                if (data.Status == 'CodeNotFound') {
                    $scope.errors = 'Card number not found.';
                }
                else if (data.Status == 'Ok') {
                    $cookies['SessionToken'] = data.SessionToken
                    $location.path('/offers');
                }

            });
        } else {
            $scope.errors = 'You must provide a card number.';
        }

    };
});

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
