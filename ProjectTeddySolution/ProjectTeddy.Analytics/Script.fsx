#r @"C:\projects\projectteddy\ProjectTeddySolution\packages\FSharp.Data.HiveProvider.0.0.3\lib\net40\HiveTypeProvider.dll"
#r @"C:\projects\projectteddy\ProjectTeddySolution\packages\EntityFramework.6.1.3\lib\net45\EntityFramework.dll"
#r @"C:\projects\projectteddy\ProjectTeddySolution\ProjectTeddy.Core\bin\Debug\ProjectTeddy.Core.dll"
#r @"C:\projects\projectteddy\ProjectTeddySolution\ProjectTeddy.FSCore\bin\Debug\ProjectTeddy.FSCore.dll"
#r "System.configuration";;
open ProjectTeddy.FSCore
open ProjectTeddy.FSCore.EnglishComponents
open ProjectTeddy.Core.EntityFramework
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols
open Microsoft.FSharp.Linq.NullableOperators
open System.Linq

let StringToPartOfSpeech (part:string) =
    match part with
    | "Noun" -> Noun SpecificNoun.Noun //
    | "Pronoun" -> Noun SpecificNoun.Pronoun //
    | "PosesivePronoun" -> Noun SpecificNoun.PosesivePronoun //
    | "Adverb" ->  Modifier SpecificModifier.Adverb //
    | "Preposition" -> Modifier SpecificModifier.Preposition //
    | "ActionVerb" -> Verb SpecificVerb.ActionVerb //
    | "BeingVerb" -> Verb SpecificVerb.BeingVerb //
    | "Adjective" -> FSPartsOfSpeech.Adjective //
    | "Punctuation" -> FSPartsOfSpeech.Punctuation //
    | "HelpingVerb" -> Verb SpecificVerb.HelpingVerb //
    | "ProperPerson" -> Noun SpecificNoun.ProperPerson //
    | "ProperPlace" -> Noun SpecificNoun.ProperPlace //
    | "ProperThing" -> Noun SpecificNoun.ProperThing //
    | "QuantifierPronoun" -> Noun SpecificNoun.QuantifierPronoun //
    | "ConcatenatedWords" -> NeedsAttention ImproperWord.ConcatenatedWords //
    | "WordAndPunctuation" -> NeedsAttention ImproperWord.WordAndPunctuation //
    | "Emoticon" -> FSPartsOfSpeech.Emoticon //
    | "Probing" -> FSPartsOfSpeech.Probing //
    | "Contractions" -> FSPartsOfSpeech.Contraction //
    | "Number" -> FSPartsOfSpeech.Number //
    | _ -> Unknown //

let CoreAWordToFSAWord (aWord:AnnotatedWord) =
    let pos = aWord.PartsOfSpeech.PartOfSpeech |> StringToPartOfSpeech
    {Id = aWord.Id.ToString(); Text = aWord.Word.Text; WordId = aWord.Id.ToString(); 
    AnnotatedBy = aWord.AnnotatedBy; AnnotatedOn = aWord.AnnotatedOn; 
    Sentiment = float32(aWord.Sentiment); PartOfSpeech = pos }

let PredictBestEntities (number:int) (tokens:string seq) =
    let db = new AnnotatorModel()    
    tokens 
    |> Seq.map(fun t -> 
                let words = db.AnnotatedWords.Where(fun x -> x.Word.Text.Contains(t)).ToList()
                if(words |> Seq.length < 1) then None else Some (words |> Seq.head |> CoreAWordToFSAWord))
    |> Seq.choose(fun x -> 
                        x)
    |> Seq.filter(fun aw -> 
                    match aw.PartOfSpeech with | Noun s -> true | _ -> false)


let t = ["trump"]
        |> List.toSeq

let e = t |> PredictBestEntities 1
e |> Seq.iter(fun x -> 
                let s = "word: " + x.Text + "POS: " + x.PartOfSpeech.ToString()
                printfn "%s" s)