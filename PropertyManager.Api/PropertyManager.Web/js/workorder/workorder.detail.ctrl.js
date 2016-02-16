﻿angular.module('app').controller('WorkOrderDetailController', function ($scope, $stateParams, WorkOrderResource) {
    $scope.workOrder = WorkOrderResource.get({ id: $stateParams.id });

    $scope.saveWorkOrder = function () {
        $scope.workOrder.$update(function () {
            alert('save successful');
        });
    };
});