namespace ProjectTeddy.CorrelationBuilder

open ProjectTeddy.Core.EntityFramework
open System.Data.Entity.Infrastructure
open System.Data
open System
open System.Linq
open ProjectTeddy.HadoopHandler

module WordsDB =    
    let calcRStrength (wordOne:Word) (wordTwo:Word) (tweets:string seq) =
        let total = tweets 
                    |> Seq.map(fun t -> t.Split(' '))
                    |> Seq.fold(fun acc tweet -> if (tweet.Contains(wordOne.Text) && tweet.Contains(wordTwo.Text)) then acc + 1 else acc) 0
        float(total) / float(tweets |> Seq.length)
    let CreateWordRelation (w1:Word) (w2:Word) (tweets:string seq)=
        let rStrength = calcRStrength w1 w2 tweets
        let id = w1.Id.ToString() + w2.Id.ToString()
        let nWordR = new WordRelation();
        nWordR.PairId <- int(id)
        nWordR.WordOne <- w1.Text
        nWordR.WordOneId <- w1.Id
        nWordR.WordTwo <- w2.Text
        nWordR.WordTwoId <- w2.Id
        nWordR.rScore <- rStrength
        nWordR
    let CreateWordRelationDB () =
        let db = new AnnotatorModel()
        db.Database.ExecuteSqlCommand("DELETE FROM [WordRelations]") |> ignore
        printfn "%s" "Entered Function and created db v8"
        let tweets = db.Tweets.ToList() |> Seq.map(fun t -> t.Text.ToLower())
        let words = db.Words.ToList() |> Seq.map(fun w -> w)
        db.Dispose()
        words 
        |> Seq.fold(fun acc w1 ->                             
                            let filtacc = acc 
                                          |> Seq.filter(fun (w2:Word) -> w1.Text <> w2.Text)
                            let adds = filtacc 
                                        |> Seq.map(fun w2 -> CreateWordRelation w1 w2 tweets)
                                        |> Seq.filter(fun wr -> if (wr.rScore > 0.0) then true else false)
                            if(adds |> Seq.length > 0)
                            then
                                using(new AnnotatorModel()) (fun db2 ->
                                adds 
                                |> Seq.iter(fun w -> 
                                            try               
                                                db2.WordRelations.Add(w) |> ignore
                                                db2.SaveChanges() |> ignore
                                            with
                                                | error -> 
                                                    let s = w.WordTwo + " error: " + error.Message
                                                    printfn "%s" s
                                )                                
                                printfn "%s" w1.Text
                                )
                            filtacc
                        ) words
        |> ignore               
    let createWordsDB () = 
        let db = new AnnotatorModel()
        let uniqueWords = db.Tweets.ToList()
                            |> Seq.cast<Tweet>
                            |> Seq.map(fun t -> 
                                        t.Text.Split(' ') 
                                        |> Array.map(fun w -> 
                                                        let lW = w.ToLower()
                                                        new Word(lW))
                                        |> Array.toSeq)
                            |> Seq.collect(fun t -> t)
                            |> Seq.distinctBy(fun w -> w.Text)
                            |> Seq.filter(fun w -> if(String.IsNullOrEmpty(w.Text) || String.IsNullOrWhiteSpace(w.Text)) then false else true)
        let words = db.Words.ToList() |> Seq.cast<Word>      
        let uWords = uniqueWords 
                        |> Seq.filter(fun w -> 
                            Operators.not (words |> Seq.exists(fun w2 -> w2.Text = w.Text)))
        db.Words.AddRange(uWords) |> ignore
        db.SaveChanges() |> ignore
        db.Database.ExecuteSqlCommand("delete from [Words] where Text = ''") |> ignore
        db.Dispose()