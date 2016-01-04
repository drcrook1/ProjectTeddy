(function () {

    var tweetFactory = function ($http) {

        var getNewTweetByUser = function () {
            return $http.get("/api/TweetsData/RandomTweetByUser")
                .then(function(response){
                    return response.data;
                });
        };

        var saveAnnotatedTweet = function (tweetData) {
            return $http.post("/api/AnnotatedTweetsData", tweetData)
            .then(function (response) {
                return response.data;
            });
        };

        var testThis = function () {
            return "Hola Sir!";
        };

        return {
            getNewTweetByUser: getNewTweetByUser,
            saveAnnotatedTweet: saveAnnotatedTweet,
            testThis: testThis
        };
        
    };

    var module = angular.module("app");
    module.factory("tweetFactory", tweetFactory);

}());