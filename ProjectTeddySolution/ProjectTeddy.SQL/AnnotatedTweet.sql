CREATE TABLE [dbo].[AnnotatedTweet]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TweetId] INT NOT NULL,
	[SentenceTypeId] INT NOT NULL,
	[Sentiment] Float NOT NULL,
	[AnnotatedBy] NVARCHAR(50) NOT NULL,
	[AnnotatedOn] DATETIME NOT NULL, 
    CONSTRAINT [FK_AnnotatedTweet_SentenceTypes] FOREIGN KEY ([SentenceTypeId]) REFERENCES [SentenceTypes]([Id]), 
    CONSTRAINT [FK_AnnotatedTweet_Tweet] FOREIGN KEY ([TweetId]) REFERENCES [Tweets]([Id])
)
