namespace ProjectTeddy.Analytics

open ProjectTeddy.Core
open ProjectTeddy.FSCore
open ProjectTeddy.FSCore.EnglishComponents
open ProjectTeddy.Core.EntityFramework
open ProjectTeddy.HadoopHandler
open System.Linq
open System.Data

module PredictionEngine =
    open System
    //Given a word and a desired part of speech, 
    //returns the strongest correlated word of that part of speech
    let GetRandom (items:'a seq) =
        let r = new Random()
        let leng = items |> Seq.length
        let index = r.Next() % leng
        (items |> Seq.toArray).[index] 
    let GetBestPartOfSpeechWord (word:FSAnnotatedWord) (pos:FSPartsOfSpeech) =
        let db = new AnnotatorModel()        
        let words = db.AnnotatedWords.ToList() 
                    |> Seq.cast<AnnotatedWord>
                    |> Seq.map(fun w -> w |> CoreTypeMappers.CoreAWordToFSAWord)
                    |> Seq.filter(fun x -> 
                        match pos with
                        | FSPartsOfSpeech.Modifier m -> 
                            match x.PartOfSpeech with
                            | FSPartsOfSpeech.Modifier m' -> if(m = m') then true else false
                            | _ -> false
                        | FSPartsOfSpeech.Noun n -> 
                            match x.PartOfSpeech with
                            | FSPartsOfSpeech.Noun n' -> if(n = n') then true else false
                            | _ -> false
                        | FSPartsOfSpeech.Verb v -> 
                            match x.PartOfSpeech with
                            | FSPartsOfSpeech.Verb v' -> if(v = v') then true else false
                            | _ -> false
                        | _ -> if(pos = x.PartOfSpeech) then true else false)
        let relations = db.WordRelations.Where(fun x -> 
                                                (x.WordOne = word.Text || x.WordTwo = word.Text)
                                                && x.rScore > 0.0).ToList()
                        |> Seq.cast<WordRelation>
        if(relations |> Seq.length < 1) //if no relations found for word, just return the first annotated word we have.
        then words |> Seq.head
        else
            let wordsString = words |> Seq.map(fun x -> x.Text)
            let rSubList = relations //find the strongest relationship
                        |> Seq.filter(fun x -> if wordsString.Contains(x.WordOne) then true else false)
            if(rSubList |> Seq.length > 0)
            then
                let bestR = rSubList
                            |> Seq.sortBy(fun w -> w.rScore)
                            |> Seq.last
                words |> Seq.find(fun x -> x.Text = bestR.WordOne)
            else 
                words |> GetRandom
    //Given a word, return the most correlated word  
    let GetBestWord (word:Word) =
        let db = new AnnotatorModel()
        let words = db.Words.ToList() 
                    |> Seq.cast<Word> 
                    |> Seq.map(fun x -> x |> CoreTypeMappers.CoreWordToFSWord)
        let relations = db.WordRelations.Where(fun x -> 
                                                (x.WordOne = word.Text || x.WordTwo = word.Text)
                                                && (x.WordOne <> "days" || x.WordTwo <> "days")
                                                && x.rScore > 0.0).ToList()
                        |> Seq.cast<WordRelation>
        let bestR = relations
                    |> Seq.sortBy(fun w -> w.rScore)
                    |> Seq.last
        if(bestR.WordOne.CompareTo(word.Text) = 0) 
        then 
            let res = db.Words.Find(bestR.WordTwoId) 
            db.Dispose()
            res
        else 
            let res = db.Words.Find(bestR.WordOneId) 
            db.Dispose()
            res

    let predictBestResponseType (cHistory:string) =
        [SentenceType.Declarative; SentenceType.Exclamatory; SentenceType.Imperative; SentenceType.Interogative; SentenceType.Probing]
        |> List.toSeq
        |> GetRandom

    let PredictBestEntities (number:int) (tokens:string seq) =     
        let db = new AnnotatorModel()   
        let totalWords = tokens 
                            |> Seq.map(fun t -> //Transforms into a list of annotated words per token
                                        let words = db.AnnotatedWords.Where(fun x -> x.Word.Text.Contains(t)).ToList()
                                                    |> Seq.cast<AnnotatedWord>
                                        if(words |> Seq.length < 1) 
                                        then None 
                                        else Some (words |> Seq.map(fun w -> CoreTypeMappers.CoreAWordToFSAWord(w))))
                            |> Seq.choose(fun x -> x) //removes entities that did not have matching anotated words
                            |> Seq.map(fun awl -> //reduces each tokens list of matching annotated words to a single word that is a noun.
                                        let nouns = awl |> Seq.filter(fun aw -> 
                                                                        match aw.PartOfSpeech with 
                                                                        | Noun s -> true | _ -> false)
                                        if(nouns |> Seq.length < 1) 
                                        then None else Some(nouns |> Seq.head))
                            |> Seq.choose(fun x -> x)
        if(totalWords |> Seq.length < 1)
        then 
            None
        else 
            if(totalWords |> Seq.length < number) 
            then  Some totalWords 
            else Some (totalWords |> Seq.take(number))
    let PredictBestTemplate (incoming:string) =
        let rType = predictBestResponseType(incoming)
        match rType with
        | SentenceType.Declarative -> 
            let d = new SentenceTemplates.DeclarativeSentence() :> SentenceTemplates.ISentence           
            let t = d.GetAllTemplates() |> GetRandom
            (rType, t)
        | SentenceType.Imperative -> 
            let d = new SentenceTemplates.ImperativeSentence() :> SentenceTemplates.ISentence
            let t = d.GetAllTemplates() |> GetRandom
            (rType, t)
        | SentenceType.Interogative -> 
            let d = new SentenceTemplates.InterogativeSentence() :> SentenceTemplates.ISentence
            let t = d.GetAllTemplates() |> GetRandom
            (rType, t)
        | _ -> 
            let d = new SentenceTemplates.ProbingSentence() :> SentenceTemplates.ISentence
            let t = d.GetAllTemplates() |> GetRandom
            (rType, t)

