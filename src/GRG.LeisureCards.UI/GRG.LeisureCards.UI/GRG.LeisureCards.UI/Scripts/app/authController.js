var loginController = angular.module('loginController', []);

loginController.factory('Login', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/Login/:id');
});

loginController.controller('LoginController', function ($scope, $cookies, $location, $localStorage, Login) {

    $scope.global.slideshow = [
        {
            img: 'http://placehold.it/1140x300',
            link: 'http://google.co.uk'
        },
        {
            img: 'http://placehold.it/1140x300',
            link: 'http://google.co.uk'
        }
    ];

    $scope.card_number = '';
    $scope.submit = function () {
        if ($scope.card_number) {
            // login and store session
            Login.get({ id: $scope.card_number }, function (data) {

                if (data.Status == 'CodeNotFound') {
                    $scope.errors = 'Card number not found.';
                }
                else if (data.Status == 'Ok') {
                    $cookies['SessionToken'] = data.SessionInfo.SessionToken;
                    $localStorage.user = data.LeisureCard;
                    
                    if (data.LeisureCard.IsAdmin) {
                        $location.path('/admin');
                    }

                    $location.path('/offers');
                }

            });

        } else {
            $scope.errors = 'You must provide a card number.';
        }

    };
});

var logoutController = angular.module('logoutController', []);
logoutController.controller('logoutController', function ($scope, $cookies, $location, $timeout, $localStorage) {
   
    $cookies.SessionToken = '';
    $localStorage.$reset();

    $timeout(function () {
        $location.path('/');
    },100);
});