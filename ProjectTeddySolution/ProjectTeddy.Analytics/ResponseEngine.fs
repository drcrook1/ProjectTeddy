namespace ProjectTeddy.Analytics

open ProjectTeddy.Core
open ProjectTeddy.FSCore
open ProjectTeddy.FSCore.EnglishComponents
open ProjectTeddy.Core.EntityFramework
open ProjectTeddy.HadoopHandler
open System.Linq
open System.Data
open System.Threading.Tasks
open SentenceTemplates
open System

module ResponseEngine =       
    let GetBestResponse (incoming:string) =       
        let tRes = PredictionEngine.PredictBestTemplate(incoming)
        let template = snd(tRes)
        let tSize = template |> Seq.length
        let entities = incoming.Split(' ') 
                            |> Array.toSeq 
                            |> PredictionEngine.PredictBestEntities tSize
        match entities with
        | Some s -> 
            let res = template |> Seq.map(fun pos -> 
                                let e = s |> PredictionEngine.GetRandom
                                pos |> PredictionEngine.GetBestPartOfSpeechWord e)
                                |> Seq.fold(fun acc w -> acc + " " + w.Text) ""
            match fst(tRes) with
            | SentenceType.Exclamatory -> (res + "!", fst(tRes))
            | SentenceType.Interogative -> (res + "?", fst(tRes))
            | _ -> (res, fst(tRes) )
        | None -> ("All of your base are belong to us.", SentenceType.Declarative)