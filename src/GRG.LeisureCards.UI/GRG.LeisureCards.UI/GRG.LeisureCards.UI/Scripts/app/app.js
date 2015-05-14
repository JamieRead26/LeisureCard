var app = angular.module('leisureApp', [
    'ngResource',
    'ngRoute',
    'ngStorage',
    'ngCookies',
    'ngSanitize',
    'ngCsv',
    'ng-breadcrumbs',
    'offersExperienceController',
    'offers241Controller',
    'offersHomeController',
    'globalController',
    'logoutController',
    'loginController',
    'adminController'
]);

app.factory('authInterceptor', function ($rootScope, $q, $cookies, $location, $timeout, $localStorage) {
    return {
        request: function (config) {
            delete $rootScope.errorKey;

            config.headers = config.headers || {};
            if ($cookies.SessionToken) {
                config.headers['SessionToken'] = $cookies.SessionToken;

                if (config.url == 'partial/login' || (config.url == 'partial/admin' && !$localStorage.user.IsAdmin)) {
                    $location.path('/offers');
                }

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
                    $localStorage.$reset();
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
globalController.controller('globalCtrl', function ($scope, breadcrumbs, $localStorage, config) {
    $localStorage.tenant = config.tenant;
    $scope.$storage = $localStorage;
    $scope.breadcrumbs = breadcrumbs;
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
        when('/admin', {
            templateUrl: 'partial/admin',
            controller: 'AdminReportController',
            label: 'Admin'
        }).
        when('/about', {
            templateUrl: 'partial/about',
            label: 'About'
        }).
        when('/terms', {
            templateUrl: 'partial/terms',
            label: 'Terms'
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
            label: '241 Offers'
        }).
        when('/offers/241/:id', {
            templateUrl: 'partial/offers_241_details',
            controller: 'offers241DetailsController',
            label: '241 Offer Details'
        }).
        when('/offers/241/claim/:id', {
            templateUrl: 'partial/offers_241_claim',
            controller: 'offers241ClaimController',
            label: 'hide'
        }).
        when('/logout', {
            template: '',
            controller: 'logoutController'
        }).
        otherwise({
            redirectTo: '/'
        });
}]);