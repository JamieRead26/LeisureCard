var adminClientController = angular.module('adminClientController', []);

adminClientController.controller('AdminClientDetailsController', function ($scope) {
    $scope.global.bodyclass = 'admin';

    $scope.clientId = [
        {
            name: '1 mile',
            value: 1
        },
        {
            name: 'all',
            value: 5000
        }];
});
//# sourceMappingURL=adminClientController.js.map
