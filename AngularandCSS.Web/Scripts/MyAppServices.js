(function () {
    'use strict';
    angular
    .module('MyApp')
    .service("UserRepositoryService",
        ['$http', '$q', '$window', '$location', function ($http, $q, $window, $location) {
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
                    return data;
                }, function (data, status) {
                    return data;
                });
            };

            this.Recover = function (email) {
                return $http.get("/api/recover", { params: { email: email }}).then(function (data) {
                    return data;
                }, function (data, status) {
                    return data;
                });
            };

            this.RecoverPassword = function (password, userID, recoveryToken) {
                return $http.get("/api/recover/request", { params: { password: password, userID: userID, recoveryToken: recoveryToken } }).then(function (data) {
                    return data;
                }, function (data, status) {
                    return data;
                });
            };



            //this.DeleteUser = function (LoginViewModel) {
            //    return $http.post("/api/delete", LoginViewModel).then(function (data) {
            //        document.location.href = "/";
            //    }, function (data) {
            //        //document.location.href = "/";
            //    });
            //};

            this.Logout = function () {
                return $http.get("/api/logout").then(function (data) {
                    document.location.href = "/";
                });
            }

            this.Login = function (LoginViewModel) {
                return $http.post("/api/login", LoginViewModel).then(function (data) {
                    location.reload();
                }, function (data) {
                    return data;
                });
            }

        }])
}());