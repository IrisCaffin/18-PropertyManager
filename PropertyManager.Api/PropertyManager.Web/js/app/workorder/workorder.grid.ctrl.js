angular.module('app').controller('WorkOrderGridController', function ($scope, WorkOrderResource) {

    function activate() {
        $scope.workOrders = WorkOrderResource.query();
    }

    activate();

});