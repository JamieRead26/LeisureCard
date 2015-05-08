var app = angular.module('leisureApp', [
    'ngResource',
    'ngRoute',
    'ui.router',
    'ngCookies',
    'offersExperienceController',
    'offers241Controller',
    'offersHomeController',
    'slideController',
    'loginController'
]);

var root = '//localhost:1623';

app.factory('authInterceptor', function ($rootScope, $q, $cookies, $location, $timeout) {
    return {
        request: function (config) {
            delete $rootScope.errorKey;

            config.headers = config.headers || {};
            if ($cookies.SessionToken) {
                config.headers['SessionToken'] = $cookies.SessionToken;
            }
            else {
                $location.path('/');
            }
            return config;
        },
        responseError: function (response) {
            var status = response.status;

            // user is not authenticated -> redirect
            if (status === 401) {
                $rootScope.errorKey = 'global.errors.unauthorized';
                $timeout(function () {
                    $location.path('/');
                }, 3000);

                // ignore form validation errors because there are handled in the specific controller
            } else if (status !== 0 && angular.isUndefined(response.data.errors)) {

                // server error
                if (response.data.text) {
                    $rootScope.errorKey = response.data.text;
                } else {
                    $rootScope.showErrorMsg = true; // general error message
                    $timeout(function () {
                        $rootScope.showErrorMsg = false;
                    }, 5000);
                }
            }

            return $q.reject(response);
        }
    };
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
});

var slideController = angular.module('slideController', []);
slideController.controller('slideshowCtrl', function ($scope) {
    $scope.parentobj = {};
    $scope.parentobj.slideshow = [];
});



/*
app.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/');

    $stateProvider
        .state('home', {
            url: '/',
            templateUrl: 'partial/login',
            controller: 'LoginController',
            data: {
                displayName: 'Home'
            }
        })
        .state('home.offers', {
            url: 'offers',

                    templateUrl: 'partial/offers',
                    controller: 'OffersHomeController',

            data: {
                displayName: 'Offers'
            }
        })
        .state('home.offers.experience', {
            url: 'offers/experience',

            templateUrl: 'partial/offers_experience',
            controller: 'offersExperienceController',

            data: {
                displayName: 'Experience Offers'
            }
        });
});
*/


app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/', {
            templateUrl: 'partial/login',
            controller: 'LoginController'
        }).
        when('/offers', {
            templateUrl: 'partial/offers',
            controller: 'OffersHomeController'
        }).
        when('/offers/experience', {
            templateUrl: 'partial/offers_experience',
            controller: 'offersExperienceController'
        }).
        otherwise({
            redirectTo: '/'
        });
}]);

