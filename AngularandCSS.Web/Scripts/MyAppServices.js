﻿(function () {
    'use strict';
    angular
    .module('MyApp')
    .service("UserRepositoryService",
        ['$http', '$q', '$window', function ($http, $q, $window) {
            this.GetUser = function () {
                return $http.get("/api/user").then(function (data, status) {
                    return { Data: data, Status: status, Error: null };
                }, function (data, status) {
                        return { Data: data, Status: status, Error: "You don't have permission to view this." };
                });
            };

            this.GetUserID = function (user) {
                return $http.get("/api/user", { params: { id: "73f2347f-d3f0-4df5-baa0-cbb83a2f382a" } }).then(function (data, status) {
                    var error = null;
                    if (data == null) {
                        error = "Unable to find user.";
                    }
                    return { Data: data, Status: status, Error: error };
                }, function (data, status) {
                    return { Data: data, Status: status, Error: "You don't have permission to view this." };
                });
            };

            this.RegisterUser = function (registeruser) {
                return $http.post("/api/register", registeruser).then(function (data) {
                    location.reload();
                }, function (data, status) {
                    return data;
                });
            };

            this.Logout = function () {
                return $http.get("/api/logout").then(function (data) {
                    location.reload();
                });
            }

            this.Login = function (LoginViewModel) {
                return $http.post("/api/login", LoginViewModel).then(function (data) {
                    location.reload();
                });
            }

        }])
}());