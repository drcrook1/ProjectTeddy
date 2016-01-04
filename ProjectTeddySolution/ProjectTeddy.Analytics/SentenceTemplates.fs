namespace ProjectTeddy.Analytics

open ProjectTeddy.FSCore.EnglishComponents

module SentenceTemplates =  
    let GetRandomItem(items: array<_>) =
        let rnd = System.Random()
        let value = rnd.Next() % items.Length
        items.[value]

    [<Interface>]
    type ISentence =
        abstract member GetSentenceType : unit -> SentenceType
        abstract member GetSingleTemplate :  int -> FSPartsOfSpeech[]
        abstract member GetAllTemplates : unit -> seq<FSPartsOfSpeech[]>

    type DeclarativeSentence() =    
        let templateA = [| FSPartsOfSpeech.Noun(SpecificNoun.Noun); FSPartsOfSpeech.Modifier(SpecificModifier.Adverb); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Adjective |]
        let templateB = [| FSPartsOfSpeech.Noun(SpecificNoun.PosesivePronoun); FSPartsOfSpeech.Noun(SpecificNoun.Noun); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Adjective |]
        let allTemplates = [| templateA; templateB |]
        interface ISentence with
            member this.GetSentenceType () =
                SentenceType.Declarative
            member this.GetSingleTemplate (index:int) =
                allTemplates.[index]
            member this.GetAllTemplates () =
                allTemplates |> Array.toSeq

    type ImperativeSentence() =    
        let templateA = [| FSPartsOfSpeech.Modifier(SpecificModifier.Adverb); FSPartsOfSpeech.Verb(SpecificVerb.ActionVerb); FSPartsOfSpeech.Noun(SpecificNoun.PosesivePronoun); FSPartsOfSpeech.Noun(SpecificNoun.Noun)|]
        let templateB = [| FSPartsOfSpeech.Verb(SpecificVerb.ActionVerb); FSPartsOfSpeech.Noun(SpecificNoun.PosesivePronoun); FSPartsOfSpeech.Noun(SpecificNoun.Noun); FSPartsOfSpeech.Modifier(SpecificModifier.Preposition); FSPartsOfSpeech.Noun(SpecificNoun.Noun) |]
        let allTemplates = [| templateA; templateB |]
        interface ISentence with
            member this.GetSentenceType() =
                SentenceType.Imperative
            member this.GetSingleTemplate (index:int) =
                allTemplates.[index]
            member this.GetAllTemplates() =
                allTemplates |> Array.toSeq

    type InterogativeSentence() =    
        let templateA = [| FSPartsOfSpeech.Modifier(SpecificModifier.Adverb); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Noun(SpecificNoun.ProperPerson); FSPartsOfSpeech.Verb(SpecificVerb.ActionVerb) |]
        let templateB = [| FSPartsOfSpeech.Modifier(SpecificModifier.Adverb); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Noun(SpecificNoun.ProperPlace); FSPartsOfSpeech.Verb(SpecificVerb.ActionVerb) |]
        let allTemplates = [| templateA; templateB |]
        interface ISentence with
            member this.GetSentenceType() =
                SentenceType.Interogative
            member this.GetSingleTemplate (index:int) =
                allTemplates.[index]
            member this.GetAllTemplates() =
                allTemplates |> Array.toSeq

    type ExclamatorySentence() =    
        let templateA = [| FSPartsOfSpeech.Noun(SpecificNoun.ProperThing); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Adjective |]
        let templateB = [| FSPartsOfSpeech.Noun(SpecificNoun.ProperThing); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Adjective |]
        let allTemplates = [| templateA; templateB |]
        interface ISentence with
            member this.GetSentenceType() =
                SentenceType.Exclamatory
            member this.GetSingleTemplate (index:int) =
                allTemplates.[index]
            member this.GetAllTemplates() =
                allTemplates |> Array.toSeq

    type ProbingSentence() =    
        let templateA = [| FSPartsOfSpeech.Noun(SpecificNoun.Noun); FSPartsOfSpeech.Verb(SpecificVerb.BeingVerb); FSPartsOfSpeech.Adjective |]
        let templateB = [| FSPartsOfSpeech.Adjective; FSPartsOfSpeech.Noun(SpecificNoun.QuantifierPronoun); FSPartsOfSpeech.Verb(SpecificVerb.ActionVerb); FSPartsOfSpeech.Modifier(SpecificModifier.Adverb)|]
        let templateC = [| FSPartsOfSpeech.Adjective; FSPartsOfSpeech.Noun(SpecificNoun.QuantifierPronoun); FSPartsOfSpeech.Verb(SpecificVerb.ActionVerb); FSPartsOfSpeech.Modifier(SpecificModifier.Preposition)|]
        let allTemplates = [| templateA; templateB; templateC |]
        interface ISentence with
            member this.GetSentenceType() =
                SentenceType.Probing
            member this.GetSingleTemplate (index:int) =
                allTemplates.[index]
            member this.GetAllTemplates() =
                allTemplates |> Array.toSeq