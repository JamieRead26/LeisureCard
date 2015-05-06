
var app = angular.module('leisure', ['ngResource', 'ngRoute', 'ngCookies']);
var root = '//localhost:1623';

app.factory('authInterceptor', function ($rootScope, $q, $cookies, $location, $timeout) {
    return {
        request: function (config) {
            delete $rootScope.errorKey;
         
            config.headers = config.headers || {};
            if ($cookies.SessionToken) {
                debugger;
                config.headers['SessionToken'] = $cookies.SessionToken;
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

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/offers', {
            templateUrl: 'partials/phone-list.html',
            controller: 'OffersListCtrl'
        }).
        otherwise({
            redirectTo: '/offers'
        });
}]);

app.factory('OffersList', function ($resource) {
    return $resource(root + '/LeisureCard/Get/:id');
});

app.controller('OffersListCtrl', function ($scope, OffersList) {

});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
});