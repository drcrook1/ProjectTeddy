(function () {

    //get the module
    var app = angular.module("app");

    //controller actions
    app.controller("tweetController", function ($scope, $log, tweetFactory) {

        $scope.$log = $log;
        $scope.loading = true;

        var onGetNewTweet = function (data) {
            $scope.newTweet = data;
            $scope.loading = false;
            $log.log("tweet loaded");
        };

        var onUpdateTweet = function (data) {
            $log.log("tweet updated");
            $scope.alertSuccess = true;
            $scope.successMsg = "Tweet " + data.TweetId + " Updated";
            tweetFactory.getNewTweetByUser()
                .then(onGetNewTweet, onError);
        };

        var onError = function (reason) {
            alert("error");
            $scope.error = reason;
        };

        $scope.getDatetime = new Date();

        tweetFactory.getNewTweetByUser()
            .then(onGetNewTweet, onError);

        $scope.tweetSubmit = function (tweetId, sentenceTypeId, userID) {

            //get values
            var annotatedTweet = {
                TweetId: tweetId,
                SentenceTypeId: sentenceTypeId,
                AnnotatedBy: userID,
                AnnotatedOn: new Date()
            };

            //save them 
            tweetFactory.saveAnnotatedTweet(annotatedTweet)
                .then(onUpdateTweet, onError);

            //debugging 
            //alert(tweetId + ' - ' + tweetAnnoType + ' - ' + userID);

        };

    });

})();