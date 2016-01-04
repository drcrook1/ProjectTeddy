(function () {

    //get the module
    var app = angular.module("app");

    //controller actions    
    app.controller("wordController", function ($scope, $log, wordFactory) {

        $scope.$log = $log;
        $scope.loading = true;

        var onGetRandomTweet = function (data) {
            $scope.Tweet = data;
            $scope.loading = false;
            $log.log("random tweet loaded | id :" + $scope.Tweet.Id);

            wordFactory.getWordsFromTweet($scope.Tweet.Id)
                .then(onGetWordsFromTweet, onError);
        };

        var onGetWordsFromTweet = function (data) {
            $log.log("Words: " + data);
            $scope.TweetWords = data.$values;
            //alert("Got words from tweet!");
        };

        var onUpdateWord = function (wordId) {
            
            //reset values
            $scope.selectedWordText = null;
            $scope.wordType = null;

            //remove word from scope
            var wordToDelete = $scope.TweetWords[$scope.selectedWordIdx];
            $scope.TweetWords.splice($scope.selectedWordIdx, 1);
            
            //if no words left lets reload a new random tweet
            var iWordsLeft = $scope.TweetWords.length;
            $log.log("Words Left: " + iWordsLeft);
            if (iWordsLeft == 0) {
                $log.log("Loading new tweet... ");
                wordFactory.getRandomTweet()
                    .then(onGetRandomTweet, onError);
            };

        };

        var onError = function (reason) {
            alert("error");
            $scope.error = reason;
        };
      
        //get the random tweet first
        wordFactory.getRandomTweet()
            .then(onGetRandomTweet, onError);

        $scope.loadNewTweet = function () {

            //reset values
            $scope.selectedWordText = null;
            $scope.wordType = null;

            //load the new tweet
            wordFactory.getRandomTweet()
                .then(onGetRandomTweet, onError);
        };

        $scope.selectWord = function (wordText, wordId, wordIdx) {
            $scope.selectedWordText = wordText;
            $scope.selectedWordId = wordId;
            $scope.selectedWordIdx = wordIdx;
            //alert("word: " + wordText + ' '+ wordId);
        }

        $scope.wordSubmit = function (wordId, wordType, userId) {

            //get values
            var annotatedWord = {
                WordId: wordId,
                AnnotatedBy: userId,
                AnnotatedOn: new Date(),
                PartOfSpeechId: wordType
            };

            //save them 
            wordFactory.saveAnnotatedWord(annotatedWord)
                .then(onUpdateWord(wordId), onError);

        };



        
    });

}());