CREATE TABLE [dbo].[Junc_ConversationTweet]
(
	[ConversationId] INT NOT NULL , 
    [TweetId] INT NOT NULL, 
    PRIMARY KEY ([ConversationId], [TweetId]), 
    CONSTRAINT [FK_ConversationTweet_Tweet] FOREIGN KEY ([TweetId]) REFERENCES [Tweets]([Id]), 
    CONSTRAINT [FK_ConversationTweet_Conversation] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id])
)
