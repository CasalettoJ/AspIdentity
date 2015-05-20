(function () {
    'use strict';
    angular
    .module('MyApp')
    .directive('login', [function () {
        return {
            restrict: 'E',
            templateUrl: '/Partials/_login.html',
            controller: function ($scope, $http, $window, UserRepositoryService) {
                $scope.loginUsername = "";
                $scope.loginPassword = "";
                $scope.submitLogin = function () {
                    alert("Ayy");
                }
            }
        };
    }])
    .directive('register', [function () {
        return {
            restrict: 'E',
            templateUrl: '/Partials/_register.html',
            controller: function ($scope, $http, $window, UserRepositoryService) {
                $scope.registerUsername = "";
                $scope.registerPassword = "";
                $scope.registerEmail = "";
                $scope.submitRegister = function () {
                    var RegisterViewModel = { UserName: $scope.registerUsername, Email: $scope.registerEmail, Password: $scope.registerPassword };
                    UserRepositoryService.RegisterUser(RegisterViewModel).then(function () {
                        alert("AyyLmao");
                    });
                }
            }
        };
    }])
}());