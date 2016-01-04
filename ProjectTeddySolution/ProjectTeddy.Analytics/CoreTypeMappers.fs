namespace ProjectTeddy.Analytics

open ProjectTeddy.FSCore
open ProjectTeddy.FSCore.EnglishComponents
open ProjectTeddy.Core.EntityFramework
open System.Linq

module CoreTypeMappers =
    let IntToPartOfSpeech (part:int) =
        match part with
        | 0 -> Noun SpecificNoun.Noun //
        | 1 -> Noun SpecificNoun.Pronoun //
        | 2 -> Noun SpecificNoun.PosesivePronoun //
        | 7 ->  Modifier SpecificModifier.Adverb //
        | 8 -> Modifier SpecificModifier.Preposition //
        | 9 -> Verb SpecificVerb.ActionVerb //
        | 10 -> Verb SpecificVerb.BeingVerb //
        | 14 -> FSPartsOfSpeech.Adjective //
        | 15 -> FSPartsOfSpeech.Punctuation //
        | 11 -> Verb SpecificVerb.HelpingVerb //
        | 3 -> Noun SpecificNoun.ProperPerson //
        | 4 -> Noun SpecificNoun.ProperPlace //
        | 5 -> Noun SpecificNoun.ProperThing //
        | 6 -> Noun SpecificNoun.QuantifierPronoun //
        | 12 -> NeedsAttention ImproperWord.ConcatenatedWords //
        | 13 -> NeedsAttention ImproperWord.WordAndPunctuation //
        | 16 -> FSPartsOfSpeech.Emoticon //
        | 19 -> FSPartsOfSpeech.Probing //
        | 17 -> FSPartsOfSpeech.Contraction //
        | 18 -> FSPartsOfSpeech.Number //
        | _ -> Unknown //

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
        let pos = aWord.PartOfSpeechId |> IntToPartOfSpeech
        {Id = aWord.Id.ToString(); Text = aWord.Word.Text; WordId = aWord.Id.ToString(); 
        AnnotatedBy = aWord.AnnotatedBy; AnnotatedOn = aWord.AnnotatedOn; 
        Sentiment = float32(aWord.Sentiment); PartOfSpeech = pos }
    
    let CoreWordToFSWord(word:Word) =
        {Id = word.Id.ToString(); Word = word.Text;}

    let StringTokensToWords (incoming : string seq) =
        let db = new AnnotatorModel()
        let words = incoming 
                    |> Seq.map(fun t -> 
                                let words = db.Words.Where(fun w -> if(w.Text.ToLower().CompareTo(t.ToLower()) = 0) then true else false) 
                                            |> Seq.cast<Word>
                                if(words |> Seq.length > 0)
                                then words |> Seq.head
                                else db.Words.Add(new Word(t)))
        db.SaveChanges() |> ignore
        words

    let WordsToAnnotatedWords(words: Word seq) =
        let db = new AnnotatorModel()
        words
        |> Seq.map(fun w -> 
                    let aWords = db.AnnotatedWords.Where(fun aw -> if(aw.Word.Id = w.Id) then true else false) |> Seq.cast<AnnotatedWord>
                    if(aWords |> Seq.length < 1)
                    then Some(aWords |> Seq.head)
                    else None)
        |> Seq.choose(fun x -> x)