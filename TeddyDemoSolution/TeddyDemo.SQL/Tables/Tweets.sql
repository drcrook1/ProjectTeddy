CREATE TABLE [dbo].[Tweets]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    [ReplyToId] NVARCHAR(MAX) NULL, 
    [Coordinates] NVARCHAR(50) NULL, 
    [CreatedOn] DATETIME NULL
)