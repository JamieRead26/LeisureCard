var loginController = angular.module('loginController', []);

loginController.factory('Login', function ($resource, config) {
    return $resource(config.apiUrl + '/LeisureCard/Login/:id');
});

loginController.controller('LoginController', function ($scope, $cookies, $location, $localStorage, Login, slideshow, ngDialog, $http, config) {

    $scope.global.slideshow = slideshow.login;
    $scope.global.bodyclass = 'homepage';

    $scope.card_number = '';

    var loginUser = function (session, card) {
        $cookies['SessionToken'] = session.SessionToken;
        card['IsAdmin'] = session.IsAdmin;
        $localStorage.user = card;
 
        if (session.IsAdmin) {
            $location.path('/admin');
        }
        else {
            $location.path('/offers');
        }
    };

    $scope.session = {};
    $scope.card = {};
    $scope.terms = { terms_checked: false };

    $scope.terms_required = true;
    $scope.check_terms = function () {

        if ($scope.terms.terms_checked) {
            $http.get(config.apiUrl + '/LeisureCard/AcceptTerms');
        }

        loginUser($scope.session, $scope.card);
        ngDialog.close();
    }

    $scope.login = function () {
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
                 
                    // show login popup
                    if (data.DisplayMemberLoginPopup) {

                        $scope.terms_required = data.MemberLoginPopupAcceptanceMandatory;
                        $scope.card = data.LeisureCard;
                        $scope.session = data.SessionInfo;

                        ngDialog.open({
                            template: 'partial/terms_popup',
                            scope: $scope
                        });
                    }
                    else {
                        loginUser(data.SessionInfo, data.LeisureCard);
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