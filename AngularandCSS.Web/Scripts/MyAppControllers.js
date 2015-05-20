(function () {
    'use strict';
    angular
    .module('MyApp')
    .controller('HomeController', [
        '$scope', 'UserRepositoryService',
        function ($scope, UserRepositoryService) {
            $scope.TestAPI = function () {
                UserRepositoryService.GetUser().then(function (data) {
                    $scope.getResults = data;
                });
                UserRepositoryService.GetUserID().then(function (data) {
                    $scope.getIDResults = data;
                });
            };
            $scope.Logout = function () {
                UserRepositoryService.Logout().then(function (data) {
                    location.reload();
                });
            }
        }
    ]);
}());