var offersDiscountController = angular.module('offersDiscountController', []);

offersDiscountController.factory('OfferDiscountClaim', function ($resource, config) {
    return $resource(config.apiUrl + '/ShortBreaks/ClaimOffer/:title');
});

offersDiscountController.controller('offersDiscountController', function ($scope, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscount;
    $scope.global.bodyclass = 'offer-discount';

});

offersDiscountController.controller('offersDiscountHoseasonsController', function ($scope,
    OfferDiscountClaim, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscounthoseasons;
    $scope.global.bodyclass = 'offer-discount-hoseasons';

    $scope.claim = function () {
        OfferDiscountClaim.get({ title: 'hoseasons' }, function (data) {
            if (!data.$resolved) {
                alert('Something when wrong when claiming this offer.');
            }
        });
    };

});

offersDiscountController.controller('offersDiscountCottageController', function ($scope,
    OfferDiscountClaim, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscountcottage;
    $scope.global.bodyclass = 'offer-discount-cottage';

    $scope.claim = function () {
        OfferDiscountClaim.get({ title: 'cottage' }, function (data) {
            if (!data.$resolved) {
                alert('Something when wrong when claiming this offer.');
            }
        });
    };

});


offersDiscountController.controller('offersDiscountClaimController', function ($scope, $sce, $routeParams, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscountclaim;
    $scope.global.bodyclass = 'offer-discount-claim';

    $scope.title = $routeParams.title;
    $scope.page_title = '';
    $scope.url = 'www.chooseacottage.co.uk/DAY';
  
    if ($scope.title == 'cottage') {
        $scope.page_title = 'Choose a Cottage';
    }
    else if ($scope.title == 'hoseasons') {
        $scope.page_title = 'Hoseasons';
        $scope.url = 'partners.hoseasons.co.uk/DAY';
    }

    $scope.website = $sce.trustAsHtml('<a href="http://' + $scope.url + '" target="_blank" class="button">Claim Reward</a>');

});