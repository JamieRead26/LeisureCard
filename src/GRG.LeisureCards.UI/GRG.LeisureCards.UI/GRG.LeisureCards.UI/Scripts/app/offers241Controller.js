﻿var offers241Controller = angular.module('offers241Controller', []);

var filterResults = function (expectedNames, key) {
    element.all(by.repeater(key + ' in offers').column(key + '.postcode')).then(function (arr) {
        arr.forEach(function (wd, i) {
            expect(wd.getText()).toMatch(expectedNames[i]);
        });
    });
};

offers241Controller.factory('Offer241GetAll', function ($http, config) {
    return $http.get(config.apiUrl + '/TwoForOne/GetAll');
});

offers241Controller.factory('Offer241GetById', function ($resource, config) {
    return $resource(config.apiUrl + '/TwoForOne/Get/:id');
});

offers241Controller.factory('Offer241Claim', function ($resource, config) {
    return $resource(config.apiUrl + '/TwoForOne/ClaimOffer/:id');
});

offers241Controller.controller('offers241Controller', function ($scope, Offer241GetAll, slideshow, $http, $location, $routeParams, config) {

    $scope.offers = {};
    $scope.global.bodyclass = 'offer-241';
    $scope.global.slideshow = slideshow.offer241;

    $scope.options = [{
        name: '1 mile',
        value: 1
    },
    {
        name: '5 miles',
        value: 5
    },
    {
        name: '10 miles',
        value: 10
    },
    {
        name: '15 miles',
        value: 15
    },
    {
        name: '25 miles',
        value: 25
    },
    {
        name: 'All',
        value: 5000
    }];

    $scope.location = $routeParams.location;
    $scope.miles = $.grep($scope.options, function (o) { return o.value == $routeParams.miles; });
    
    if($scope.miles.length == 0){
        $scope.miles = $scope.options[2];
    }

    if ($scope.location && $scope.miles) {
        $location.hash('241search');
        $scope.miles = $scope.miles[0];

        var url = config.apiUrl + '/TwoForOne/FindByLocation/' + $scope.location + '/' + $scope.miles.value;
        $http.get(url).then(function (r) {
            $scope.offers = r.data.$values;
        });
    }

    $scope.submit = function () {
        if ($scope.location) {

            $location.path('/offers/241').search({ location: $scope.location, miles: $scope.miles.value });

        } else {
            $scope.errors = 'You must provide a location.';
        }
    };

    $scope.goBack = function() {
        window.history.back();
    }
});

offers241Controller.controller('offers241DetailsController', function ($scope, $sce, $window, $routeParams,
    Offer241GetById, Offer241Claim, slideshow, $location) {

    $scope.id = $routeParams.id;
    $scope.offer = {};
    $scope.global.bodyclass = 'offer-241-details';
    $scope.global.slideshow = slideshow.offer241details;
    
    Offer241GetById.get({ id: $scope.id }, function (data) {
    
        var website = data.Website ? $sce.trustAsHtml('<a href="http://' + data.Website + '" target="_blank">' + data.Website + '</a>') : '';

        $scope.offer = {
            
            County: data.County,
            Description: data.Description,
            DisabledAccess: data.DisabledAccess,
            OutletName: data.OutletName,
            Phone: data.Phone,
            Website: website,
            CategoryKey: data.CategoryKey,
            Address1: data.Address1,
            Address2: data.Address2,
            TownCity: data.TownCity,
            County: data.County,
            PostCode: data.PostCode
        };
    });
    
    $scope.removeNull = function (items) {
        var result = {};
        angular.forEach(items, function (value, key) { 
            if (value) {
                result[key] = value;
            }
        });
        return result;
    }

    $scope.claim = function (url) {
        debugger;
        Offer241Claim.get({ id: $scope.id }, function (data) {
            if(!data.$resolved){
                return alert('Something went wrong when claiming this offer.');
            }
            $window.open('/#' + url + $scope.id);
        });
    };

});

offers241Controller.controller('offers241ClaimController', function ($scope, $sce, $routeParams, $cookies, Offer241GetById, Offer241Claim, slideshow) {

    debugger;
    var now = new Date();
    now.setDate(now.getDate() + 14);

    var strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/"), [AddZero(now.getHours()), AddZero(now.getMinutes())].join(":"), now.getHours() >= 12 ? "PM" : "AM"].join(" ");

    //Pad given value to the left with "0"
    function AddZero(num) {
        return (num >= 0 && num < 10) ? "0" + num : num + "";
    }

    $scope.global.slideshow = slideshow.offer241claim;
    $scope.global.bodyclass = 'offer-241-claim';
    $scope.id = $routeParams.id;
    $scope.validUntil = strDateTime;
    $scope.SessionToken = $cookies.SessionToken;

    Offer241GetById.get({ id: $scope.id }, function (data) {

        var url = data.Website ? $sce.trustAsHtml('<a href="http://' + data.Website + '" target="_blank" class="button">Claim Reward</a>') : '';
       
        $scope.offer = {
            Id : data.Id,
            OutletName: data.OutletName,
            BookingInstructions: data.BookingInstructions,
            ClaimCode: data.ClaimCode,
            Website: url
        };
    });

    $scope.print = function () {
        Window.print();
    };

    $scope.claim = function (url) {
        Offer241Claim.get({ id: $scope.id }, function (data) {
            if (!data.$resolved) {
                return alert('Something went wrong when claiming this offer.');
            }
            debugger;
            $location.path(url + $scope.id);
        });
    };
});

