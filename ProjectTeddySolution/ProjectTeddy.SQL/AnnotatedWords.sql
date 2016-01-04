CREATE TABLE [dbo].[AnnotatedWords]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WordId] INT NOT NULL, 
    [AnnotatedBy] NVARCHAR(255) NOT NULL, 
    [AnnotatedOn] DATETIME NOT NULL, 
	[Sentiment] Float NOT NULL DEFAULT 0.0,
	[PartOfSpeechId] INT NOT NULL, 
    CONSTRAINT [FK_AnnotatedWords_PartsOfSpeech] FOREIGN KEY ([PartOfSpeechId]) REFERENCES [PartsOfSpeech]([Id]), 
    CONSTRAINT [FK_AnnotatedWords_Words] FOREIGN KEY ([WordId]) REFERENCES [Words]([Id])
)