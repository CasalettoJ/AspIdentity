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
                $scope.recoverRequest = "";
                $scope.submitLogin = function () {
                    var LoginViewModel = { UserName: $scope.loginUsername, Password: $scope.loginPassword, Email: null, RememberMe: false };
                    UserRepositoryService.Login(LoginViewModel);
                }
                $scope.submitRecoveryRequest = function () {
                    UserRepositoryService.Recover($scope.recoverRequest).then(function (data) {
                        $scope.recoverData = data;
                    });
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
                    var RegisterViewModel = { UserName: $scope.registerUsername, Email: $scope.registerEmail, Password: $scope.registerPassword, CaptchaResponse: document.getElementById("g-recaptcha-response").value };
                    UserRepositoryService.RegisterUser(RegisterViewModel).then(function (data) {
                        $scope.registerError = data;
                    });
                }
            }
        };
    }])
    .directive('recover', [function () {
        return {
            restrict: 'E',
            templateUrl: '/Partials/_recover.html',
            controller: function ($scope, $http, $window, UserRepositoryService, $location) {
                $scope.recoverPassword = "";
                var queries = location.search.slice(1).split('&');
                var userID = queries[0].split('=')[1];
                var recoveryToken = queries[1].split('=')[1];
                $scope.submitRecover = function () {
                    UserRepositoryService.RecoverPassword($scope.recoverPassword, userID, recoveryToken).then(function (data) {
                        $scope.recoverData = data;
                    });
                }
            }
        };
    }])
}());