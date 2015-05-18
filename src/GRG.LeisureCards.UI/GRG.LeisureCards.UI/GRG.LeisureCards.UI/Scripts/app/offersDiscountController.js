var offersDiscountController = angular.module('offersDiscountController', []);

offersDiscountController.factory('OfferDiscountClaim', function ($resource, config) {
    return $resource(config.apiUrl + '/ShortBreaks/ClaimOffer/:slug');
});

offersDiscountController.controller('offersDiscountController', function ($scope, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscount;
    $scope.global.bodyclass = 'offer-discount';

});

offersDiscountController.controller('offersDiscountDetailsController', function ($scope, $routeParams, OfferDiscountClaim, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscountdetails;
    $scope.global.bodyclass = 'offer-discount-details';

    $scope.slug = $routeParams.slug;
    $scope.title = $scope.slug == 'cottage' ? 'Choose a Cottage' : 'Hoseasons';

    $scope.claim = function () {
        OfferDiscountClaim.get({ slug: $scope.slug }, function (data) {
            if (!data.$resolved) {
                alert('Something when wrong when claiming this offer.');
            }
        });
    };

});

offersDiscountController.controller('offersDiscountClaimController', function ($scope, $sce, $routeParams, slideshow) {

    $scope.global.slideshow = slideshow.offerdiscountclaim;
    $scope.global.bodyclass = 'offer-discount-claim';

    $scope.slug = $routeParams.slug;
    $scope.title = '';
    $scope.url = 'www.chooseacottage.co.uk/vof';
  
    if($scope.slug == 'cottage'){
        $scope.title = 'Choose a Cottage';
    }
    else if ($scope.slug == 'hoseasons') {
        $scope.title = 'Hoseasons';
        $scope.url = 'partners.hoseasons.co.uk/vof';
    }

    $scope.website = $sce.trustAsHtml('<a href="http://' + $scope.url + '" target="_blank" class="button">Claim Reward</a>');

});