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

offers241Controller.controller('offers241Controller', function ($scope, Offer241GetAll) {

    $scope.offers = {};
    $scope.searchText = '';
    $scope.global.slideshow = [
        {
            img: 'http://placehold.it/1140x300',
            link: 'http://google.co.uk'
        }
    ];

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
