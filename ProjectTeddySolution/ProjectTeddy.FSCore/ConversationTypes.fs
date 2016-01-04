namespace ProjectTeddy.FSCore

open EnglishComponents
open System

[<CLIMutable>]
type FSConversation =
    {Id:string; Tweets:seq<FSTweet>}
[<CLIMutable>]
type FSAnnotatedConversation = 
    {Id:string; ConversationId:string; SuggestedResponse:string; SugRespSentenceType:SentenceType; SugRespSentiment:float32; AnnotatedBy:string; AnnotatedOn:DateTime}

