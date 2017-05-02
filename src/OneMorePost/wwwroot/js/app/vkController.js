(function () {
    'use strict';

    angular
        .module('vk', ['local'])
        .controller('vkController', vkController);

    vkController.$inject = ['$scope', 'localService']; 

    function vkController($scope, localService) {
        $scope.title = 'ВК';
        $scope.userId = 1;
        $scope.authToken = 0;

        $scope.auth = function () {
            var url = $("#vkframe").get(0).contentWindow.location;
            localService.register($scope.userId, $scope.authToken);
        }

        activate();

        function activate() { }
    }
})();
