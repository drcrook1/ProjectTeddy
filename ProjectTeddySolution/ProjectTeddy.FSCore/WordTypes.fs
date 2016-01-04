namespace ProjectTeddy.FSCore

open EnglishComponents
open System

[<CLIMutable>]
type FSWord =
    {Id:string; Word:string}
[<CLIMutable>]
type FSAnnotatedWord =
    {Id:string; Text:string; WordId:string; AnnotatedBy:string; AnnotatedOn:DateTime; Sentiment:float32; PartOfSpeech:FSPartsOfSpeech}
[<CLIMutable>]
type FSWordRelationship =
    {Id:string; WordOne:string; WordOneId:int; WordTwo:string; WordTwoId:int; rScore:float}

