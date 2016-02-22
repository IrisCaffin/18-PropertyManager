angular.module('app').controller('PropertyDetailController', function ($scope, $stateParams, PropertyResource) {
    $scope.property = PropertyResource.get({ id: $stateParams.id });

    $scope.saveProperty = function () {
        $scope.property.$update(function () {
            alert('save successful');
        });
    };
});