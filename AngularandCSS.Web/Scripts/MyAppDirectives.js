(function () {
    'use strict';
    angular
    .module('MyApp')
    .directive('login', [function () {
        return {
            restrict: 'E',
            templateUrl: '/Partials/_login.html',
            controller: function ($scope, $http, $window, UserRepositoryService) {
                $scope.username = "";
                $scope.password = "";
                $scope.submitLogin = function () {
                    alert("Ayy");
                }
            }
        };
    }
    ])
}());