(function () {

    var wordFactory = function ($http) {

        var getRandomTweet = function () {
            return $http.get("/api/TweetsData/RandomTweet")
                .then(function (response) {
                    return response.data;
                });
        };

        var getWordsFromTweet = function (tweetId) {
            //alert("TweetID: " + tweetId);
            return $http.get("/api/WordsData/WordsFromTweet/" + tweetId)
            .then(function (response) {
                return response.data;
            });
        };

        var saveAnnotatedWord = function (wordData) {
            return $http.post("/api/AnnotatedWordsData", wordData)
            .then(function (response) {
                return response.data;
            });
        };

        return {
            getRandomTweet: getRandomTweet,
            getWordsFromTweet: getWordsFromTweet,
            saveAnnotatedWord: saveAnnotatedWord
        };

    };

    var module = angular.module("app");
    module.factory("wordFactory", wordFactory);

}());