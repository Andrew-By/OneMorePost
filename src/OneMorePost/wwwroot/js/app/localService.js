(function () {
    'use strict';

    angular
        .module('local', [])
        .factory('localService', localService);

    localService.$inject = ['$http'];

    function localService($http) {
        var service = {
            register: register
        };

        return service;

        function register(userId, authToken) {
            return $http.get('/api/VK/Auth?Code=' + authToken);
        }
    }
})();