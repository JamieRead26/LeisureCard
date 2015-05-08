var app = angular.module('leisureApp', [
    'ngResource',
    'ngRoute',
    'ngCookies',
    'ng-breadcrumbs',
    'offersExperienceController',
    'offers241Controller',
    'offersHomeController',
    'slideController',
    'globalController',
    'offers241DetailsController',
    'logoutController',
    'loginController'
]);

app.constant('config', {
    apiUrl: '//localhost:1623'
});

app.value('user', {
    card: {}
});

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
                    $cookies.SessionToken = '';
                    $location.path('/');
                }, 1000);

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

var globalController = angular.module('globalController', []);
globalController.controller('globalCtrl', function ($scope, breadcrumbs, user) {
    $scope.user = user;
    $scope.breadcrumbs = breadcrumbs;
});

var slideController = angular.module('slideController', []);
slideController.controller('slideshowCtrl', function ($scope) {
    $scope.global = {};
    $scope.global.slideshow = [];
});

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/', {
            templateUrl: 'partial/login',
            controller: 'LoginController',
            label: 'Home'
        }).
        when('/offers', {
            templateUrl: 'partial/offers',
            controller: 'OffersHomeController',
            label: 'Offers'
        }).
        when('/offers/experience', {
            templateUrl: 'partial/offers_experience',
            controller: 'offersExperienceController',
            label: 'Experiences'
        }).
        when('/offers/241', {
            templateUrl: 'partial/offers_241',
            controller: 'offers241Controller',
            label: '241'
        }).
        when('/offers/241/:id', {
            templateUrl: 'partial/offers_241_details',
            controller: 'offers241DetailsController',
            label: 'Offer Details'
        }).
        when('/logout', {
            template: '',
            controller: 'logoutController'
        }).
        otherwise({
            redirectTo: '/'
        });
}]);


/*
// this is for breadcrumbs... to-do
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


