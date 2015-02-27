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

            this.GetUserID = function (user) {
                var updateUser = $q.defer();
                $http.get("/api/user", { params: { id: 1 } })
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