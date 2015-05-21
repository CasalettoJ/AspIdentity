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
                    var LoginViewModel = { UserName: $scope.loginUsername, Password: $scope.loginPassword, Email: null, RememberMe: false };
                    UserRepositoryService.Login(LoginViewModel);
                }
                //$scope.submitDeletion = function () {
                //    var LoginViewModel = { UserName: $scope.loginUsername, Password: $scope.loginPassword, Email: null, RememberMe: false };
                //    UserRepositoryService.DeleteUser(LoginViewModel);
                //}
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
                    UserRepositoryService.RegisterUser(RegisterViewModel).then(function (data) {
                        $scope.registerError = data;
                    });
                }
            }
        };
    }])
}());