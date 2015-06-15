var loginController = angular.module('loginController', []);

loginController.factory('Login', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/Login/:id');
});

loginController.controller('LoginController', function ($scope, $cookies, $location, $localStorage, Login, slideshow) {

    $scope.global.slideshow = slideshow.login;
    $scope.global.bodyclass = 'homepage';

    $scope.card_number = '';
    $scope.submit = function () {
        if ($scope.card_number) {
            // login and store session
            Login.get({ id: $scope.card_number }, function (data) {
            
                if (data.Status == 'CodeNotFound') {
                    $scope.errors = 'Number is not recognised, please try again.';
                }
                else if (data.Status == 'CardExpired') {
                    $scope.errors = 'Your subscription has run out.';
                }
                else if (data.Status == 'CardSuspended') {
                    $scope.errors = 'Your subscription has been suspended.';
                }
                else if (data.Status == 'Ok') {
                    $cookies['SessionToken'] = data.SessionInfo.SessionToken;

                    data.LeisureCard['IsAdmin'] = data.SessionInfo.IsAdmin;

                    $localStorage.user = data.LeisureCard;
                    
                    if (data.LeisureCard.IsAdmin) {
                        $location.path('/admin');
                    }
                    else {
                        $location.path('/offers');
                    }
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