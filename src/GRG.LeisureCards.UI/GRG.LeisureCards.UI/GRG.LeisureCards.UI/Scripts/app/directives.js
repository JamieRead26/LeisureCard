app.directive('slideit', function () {
    return {
        restrict: 'A',
        replace: true,
        scope: {
            slideit: '='
        },
        template: '<ul class="bxslider">' +
                    '<li ng-repeat="s in slides">' +
                        '<a href="{{ s.link }}" target="_blank">' +
                            '<img ng-src="{{ s.img }}" />' +
                        '</a>' +
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