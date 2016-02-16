angular.module('app').controller('LeaseDetailController', function ($scope, $stateParams, LeaseResource) {
    $scope.lease = LeaseResource.get({ id: $stateParams.id });

    $scope.saveLease = function () {
        $scope.lease.$update(function () {
            alert('save successful');
        });
    };
});