(function () {
    'use strict';

    angular
        .module('vk')
        .controller('vkController', vkController);

    vkController.$inject = ['$scope']; 

    function vkController($scope) {
        $scope.title = 'ВК';
        $scope.groupId = 0;

        $scope.auth = function () {

        }

        activate();

        function activate() { }
    }
})();
