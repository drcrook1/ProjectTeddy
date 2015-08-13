CREATE TABLE [dbo].[AnnotatedConversation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ConversationId] INT NOT NULL,
	[Response] NVARCHAR(MAX) NOT NULL,
	[ResponseSentenceTypeId] INT NOT NULL,
	[ResponseSentiment] FLOAT NOT NULL,
	[AnnotatedBy] NVARCHAR(50) NOT NULL,
	[AnnotatedOn] DATETIME NOT NULL, 
    CONSTRAINT [FK_AnnotatedConversation_Conversations] FOREIGN KEY ([ConversationId]) REFERENCES [Conversations]([Id]), 
    CONSTRAINT [FK_AnnotatedConversation_SentenceTypes] FOREIGN KEY ([ResponseSentenceTypeId]) REFERENCES [SentenceTypes]([Id])
)
