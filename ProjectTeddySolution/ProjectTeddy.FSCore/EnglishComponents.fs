namespace ProjectTeddy.FSCore

module EnglishComponents =
    type SentenceType =
        | Imperative
        | Declarative
        | Interogative
        | Exclamatory
        | Probing

    type SpecificNoun =
        | Noun
        | Pronoun
        | PosesivePronoun
        | ProperPerson
        | ProperPlace
        | ProperThing
        | QuantifierPronoun

    type SpecificModifier =
        | Adverb //slowly, quickly, verb + ly (90% of the time)
        | Preposition //off, on, together, behind, before, between, above, with, below

    type SpecificVerb =
        | ActionVerb
        | BeingVerb
        | HelpingVerb

    type ImproperWord =
        | ConcatenatedWords
        | WordAndPunctuation

    type FSPartsOfSpeech =
        | Noun of SpecificNoun
        | Verb of SpecificVerb
        | Modifier of SpecificModifier
        | NeedsAttention of ImproperWord
        | Adjective
        | Punctuation
        | Emoticon
        | Contraction
        | Number
        | Probing
        | Unknown

