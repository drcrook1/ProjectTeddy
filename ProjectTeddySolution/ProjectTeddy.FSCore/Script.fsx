// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.



// Define your library scripting code here

type SpecificNoun =
    | Noun
    | NounPhrase
    | Pronoun
    | PosesivePronoun

type SpecificModifier =
    | Adverb //slowly, quickly, verb + ly (90% of the time)
    | Preposition //off, on, together, behind, before, between, above, with, below

type SpecificVerb =
    | ActionVerb
    | BeingVerb
    | PossesiveVerb
    | TransitiveVerb

type FSPartsOfSpeech =
    | Noun of SpecificNoun
    | Verb of SpecificVerb
    | Adjective
    | Punctuation
    | Modifier of SpecificModifier
    | Unknown

let dostuff (a:FSPartsOfSpeech) (b:FSPartsOfSpeech) =
    if(a = b)
    then printfn "match"
    else printfn "no match"

let a = Verb SpecificVerb.ActionVerb
let b = Modifier SpecificModifier.Adverb

dostuff a b