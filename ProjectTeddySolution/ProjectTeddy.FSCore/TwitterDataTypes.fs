namespace ProjectTeddy.FSCore
open System
open EnglishComponents

[<CLIMutable>]
type FSTweet =
    {Id:string; Text:string; ReplyToId:string; Coordinates:string; CreatedOn:DateTime}  
[<CLIMutable>]
type FSAnnotatedTweet = 
    {Id:string; TweetId:string; SentenceType:SentenceType; Sentiment:float32; AnnotatedBy:string; AnnotatedOn:DateTime }