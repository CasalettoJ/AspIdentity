(function () {
    'use strict';
    angular
    .module('MyApp')
    .controller('HomeController', [
        '$scope', 'UserRepositoryService',
        function ($scope, UserRepositoryService) {
            $scope.TestAPI = function () {
                UserRepositoryService.GetUser().then(function (data) {
                    if (data.Error) {
                        $scope.getResults = data.Error
                    }
                    else {
                        $scope.getResults = data;
                    }
                });
                UserRepositoryService.GetUserID().then(function (data) {
                    if (data.Error) {
                        $scope.getIDResults = data.Error
                    }
                    else {
                        $scope.getIDResults = data;
                    }
                });
            };
            $scope.Logout = function () {
                UserRepositoryService.Logout();
            }


        }
    ]);
}());