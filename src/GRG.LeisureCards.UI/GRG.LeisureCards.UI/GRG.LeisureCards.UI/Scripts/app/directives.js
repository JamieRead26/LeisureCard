﻿app.directive('slideit', function () {
    return {
        restrict: 'A',
        replace: true,
        scope: {
            slideit: '='
        },
        template: '<ul class="bxslider">' +
                    '<li ng-repeat="s in slides">' +
                        '<a href="{{ s.link }}" target="_blank" ng-if="s.link">' +
                            '<img ng-src="{{ s.img }}" />' +
                        '</a>' +
                        '<img ng-src="{{ s.img }}" ng-if="!s.link" />' +
                    '</li>' +
                   '</ul>',
        link: function (scope, elm, attrs) {
            elm.ready(function () {
                scope.$apply(function () {
                    scope.slides = scope.slideit;
                    scope.bxslider = $('.bxslider');
                });

                scope.$watch('slideit', function (newValue, oldValue) {
                    if (newValue) {
                        scope.slides = newValue;
                        setTimeout(function () {
                            scope.bxslider.reloadSlider();
                        }, 0);
                    }
                }, true);

                scope.bxslider.bxSlider();
            });
        }
    };
});

app.directive('a', function () {
    return {
        restrict: 'E',
        link: function (scope, elem, attrs) {
            if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                elem.on('click', function (e) {
                    e.preventDefault();
                });
            }
        }
    };
});


// Lazy loading of Google Map API
app.service('loadGoogleMapAPI', ['$window', '$q', 'config',
    function ($window, $q, config) {

        var deferred = $q.defer();

        // Load Google map API script
        function loadScript() {
            // Use global document since Angular's $document is weak
            var script = document.createElement('script');
            script.src = '//maps.googleapis.com/maps/api/js?sensor=false&language=en&callback=initMap&key=' + config.googleAPiKey;

            document.body.appendChild(script);
        }

        // Script loaded callback, send resolve
        $window.initMap = function () {
            deferred.resolve();
        }

        loadScript();

        return deferred.promise;
    }]);

// Google Map
app.directive('googleMap', ['$rootScope', 'loadGoogleMapAPI',
    function ($rootScope, loadGoogleMapAPI) {

        return {
            restrict: 'C',
            scope: {
                mapId: '@id',
                postcode: '@',   
            },
            link: function ($scope, elem, attrs) {

                // Check if latitude and longitude are specified
                if (angular.isDefined($scope.postcode)) {

                    // Initialize the map
                    $scope.initialize = function () {

                        $scope.location = new google.maps.LatLng(-34.397, 150.644); // placeholder location
                        $scope.mapOptions = {
                            zoom: 12,
                            center: $scope.location
                        };

                        $scope.map = new google.maps.Map(document.getElementById($scope.mapId), $scope.mapOptions);

                        new google.maps.Marker({
                            position: $scope.location,
                            map: $scope.map,
                        });

                        function codeAddress() {
                            var geocoder = new google.maps.Geocoder();

                            geocoder.geocode({ 'address': $scope.postcode }, function (results, status) {
                                if (status == google.maps.GeocoderStatus.OK) {
                                    $scope.map.setCenter(results[0].geometry.location);
                                    var marker = new google.maps.Marker({
                                        map: $scope.map,
                                        position: results[0].geometry.location
                                    });
                                } else {
                                    console.error('Geocode was not successful for the following reason: ' + status);
                                }
                            });
                        }

                        setTimeout(function () {
                            codeAddress();
                        }, 1000);
                    }

                    // Loads google map script
                    loadGoogleMapAPI.then(function () {
                        // Promised resolved
                        $scope.initialize();
                    }, function () {
                        // Promise rejected
                    });
                }
            }
        };
    }]);


app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);


app.service('fileUpload', ['$http', function ($http) {
    this.uploadFileToUrl = function (file, uploadUrl) {
        var fd = new FormData();
        fd.append('file', file);
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
        .success(function (d) {

            if (!d.Message) {
                d.Message = 'Success';
            }

            alert('File upload successful, server responded: ' + d.Message);
        })
        .error(function (d) {
            alert('File upload failed: ' + d.ExceptionMessage);
        });
    }
}]);