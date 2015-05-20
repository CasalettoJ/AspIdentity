(function () {
    'use strict';
    angular
    .module('MyApp')
    .service("UserRepositoryService",
        ['$http', '$q', '$window', function ($http, $q, $window) {
            this.GetUser = function () {
                var getUser = $q.defer();
                $http.get("/api/user")
                .success(function (data, status) {
                    getUser.resolve(data);
                })
                .error(function (data, status) {
                        getUser.resolve(data);
                });
                return getUser.promise;
            };

            this.RegisterUser = function (registeruser) {
                var registerUser = $q.defer();
                $http.post("/Home/Register", registeruser )
                .success(function (data) {
                    registerUser.resolve(data);
                    alert("Registered and logged in as user: " + registeruser.UserName);
                })
                .error(function (data, status) {
                    registerUser.resolve(data);
                });
                return registerUser.promise;
            };

            this.GetUserID = function (user) {
                var updateUser = $q.defer();
                $http.get("/api/user", { params: { id: "73f2347f-d3f0-4df5-baa0-cbb83a2f382a" } })
                .success(function (data) {
                    updateUser.resolve(data);
                })
                .error(function (data, status) {
                        updateUser.resolve(data);
                });
                return updateUser.promise;
            };

        }])
}());