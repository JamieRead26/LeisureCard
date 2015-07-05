var app = angular.module('leisureApp', [
    'ngResource',
    'ngRoute',
    'ngStorage',
    'ngCookies',
    'ngSanitize',
    'ngCsv',
    'ngDialog',
    'autocomplete',
    'ng-breadcrumbs',
    'offersExperienceController',
    'offersDiscountController',
    'offers241Controller',
    'offersHomeController',
    'generalContentController',
    'globalController',
    'logoutController',
    'loginController',
    'adminReportsController',
    'adminClientController'
]);

app.factory('authInterceptor', function ($rootScope, $q, $cookies, $location, $timeout, $localStorage) {
    return {
        request: function (config) {
            delete $rootScope.errorKey;

            config.headers = config.headers || {};
            if ($cookies.SessionToken) {
                config.headers['SessionToken'] = $cookies.SessionToken;
               
                if (config.url == 'partial/login' || (config.url.indexOf('admin') != -1 && !$localStorage.user.IsAdmin)) {
                    $location.path('/offers');
                }

            }
            else if (config.url != 'partial/terms' || config.url != 'partial/contactus') {
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

app.factory('PushImportToArray', function ($resource, config) {

    return {
        push: function ($scope, data, default_key) {
            var _push = function (data) {

                // Find any existing import records of the same UploadKey
                var i = -1;
                $.grep($scope.imports, function (item, index) {
                    if (item.UploadKey == default_key)
                        i = index;
                });

                // If any existing records were found of the same UploadKey then remove them
                if (i != -1)
                    $scope.imports.splice(i, 1);

                // Now add the data to the list of records.
                return $scope.imports.push(data);
            };

            if (!data.LastRun) {
                // default 
                return _push({
                    LastRun: null,
                    UploadKey: default_key,
                    Success: null,
                    Message: null,
                    FileName: null,
                    Status: null
                });
            }

            return _push(data);
        }
    }

});

var globalController = angular.module('globalController', []);
globalController.controller('globalCtrl', function ($scope, breadcrumbs, $location, $anchorScroll, $localStorage, config) {
    $localStorage.tenant = config.tenant;
    $scope.$storage = $localStorage;
    $scope.breadcrumbs = breadcrumbs;
    $scope.global = {};
    $scope.global.slideshow = [];
    $scope.global.bodyclass = '';

    $scope.go_back = function () {
        window.history.back();
    };

    $scope.to_top = function () {
        $location.hash('ng-app');
        $anchorScroll();
    };

    $scope.redirectTo = function (url) {
        $location.path(url);
    };

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
        when('/admin/client-details/:key', {
            templateUrl: 'partial/admin_client_details',
            controller: 'AdminClientDetailsController',
            label: 'Client Details'
        }).
        when('/about', {
            templateUrl: 'partial/about',
            label: 'About'
        }).
        when('/terms', {
            templateUrl: 'partial/terms',
            controller: 'TermsController',
            label: 'Terms'
        }).
        when('/contactus', {
             templateUrl: 'partial/contactus',
             controller: 'ContactUsController',
             label: 'Contact Us'
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
        when('/offers/discount', {
            templateUrl: 'partial/offers_discount',
            controller: 'offersDiscountController',
            label: 'Discount Offers'
        }).
        when('/offers/discount/cottage', {
            templateUrl: 'partial/offers_discount_cottage',
            controller: 'offersDiscountCottageController',
            label: 'Discount Offer Cottage'
        }).
        when('/offers/discount/hoseasons', {
            templateUrl: 'partial/offers_discount_hoseasons',
            controller: 'offersDiscountHoseasonsController',
            label: 'Discount Offer Hoseasons'
        }).
        when('/offers/discount/claim/:title', {
            templateUrl: 'partial/offers_discount_claim',
            controller: 'offersDiscountClaimController',
            label: 'hide'
        }).
        when('/logout', {
            template: 'partial/logout',
            controller: 'logoutController'
        }).
        otherwise({
            redirectTo: '/'
        });
}]);