var offers241Controller = angular.module('offers241Controller', []);

var filterResults = function (expectedNames, key) {
    element.all(by.repeater(key + ' in offers').column(key + '.postcode')).then(function (arr) {
        arr.forEach(function (wd, i) {
            expect(wd.getText()).toMatch(expectedNames[i]);
        });
    });
};

offers241Controller.factory('Offer241GetAll', function ($resource, config) {
    return $resource(config.apiUrl + '/TwoForOne/GetAll');
});

offers241Controller.factory('Offer241GetById', function ($resource, config) {
    return $resource(config.apiUrl + '/TwoForOne/Get/:id');
});

offers241Controller.factory('Offer241Claim', function ($resource, config) {
    return $resource(config.apiUrl + '/TwoForOne/ClaimOffer/:id');
});

offers241Controller.controller('offers241Controller', function ($scope, Offer241GetAll, slideshow) {

    $scope.offers = {};
    $scope.searchText = '';
    $scope.global.slideshow = slideshow.offer241;

    Offer241GetAll.get(function (data) {
        $scope.offers = data.$values;
    });

    $scope.criteriaMatch = function (searchText) {
        return function (item) {
            s = searchText.toLowerCase();
            town = item.TownCity.toLowerCase();
            postcode = item.PostCode.toLowerCase();
            county = item.County.toLowerCase();

            return town.indexOf(s) > -1 ||
                   postcode.indexOf(s) > -1 ||
                   county.indexOf(s) > -1;
        };
    };

    $scope.submit = function () {
        if ($scope.keyword) {
            $scope.searchText = $scope.keyword;
        } else {
            $scope.errors = 'You must provide a location.';
        }
    };
});

offers241Controller.controller('offers241DetailsController', function ($scope, $sce, $window, $routeParams,
    Offer241GetById, Offer241Claim, slideshow) {

    $scope.id = $routeParams.id;
    $scope.offer = {};
    $scope.global.slideshow = slideshow.offer241details;
  
    Offer241GetById.get({ id: $scope.id }, function (data) {
        $scope.offer = {
            Address1: data.Address1,
            Address2: data.Address2,
            County: data.County,
            Description: data.Description,
            DisabledAccess: data.DisabledAccess,
            OutletName: data.OutletName,
            Phone: data.Phone,
            PostCode: data.PostCode,
            TownCity: data.TownCity,
            Website: $sce.trustAsHtml('<a href="http://' + data.Website + '" target="_blank">' + data.Website + '</a>')
        };
    });
    
    $scope.claim = function () {
        Offer241Claim.get({ id: $scope.id }, function (data) {
            debugger;
            if(!data.$resolved){
                alert('Something when wrong when claiming this offer.');
            }
        });
    };

});

offers241Controller.controller('offers241ClaimController', function ($scope, $sce, $routeParams, Offer241GetById, slideshow) {

    $scope.global.slideshow = slideshow.offer241claim;
    $scope.id = $routeParams.id;

    Offer241GetById.get({ id: $scope.id }, function (data) {

        var url = '';
        if (data.Website) {
            url = $sce.trustAsHtml('<a href="http://' + data.Website + '" target="_blank" class="button">Claim Reward</a>')
        }

        $scope.offer = {
            OutletName: data.OutletName,
            Website: url
        };
    });

});