angular.module('app').factory('WorkOrderResource', function (apiUrl, $resource) {
    return $resource(apiUrl + '/workOrders/:OrderId', { workOrderId: '@WorkOrderId' },
    {
        'update': {
            method: 'PUT'
        }
    })
});