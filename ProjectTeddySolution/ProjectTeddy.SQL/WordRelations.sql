CREATE TABLE [dbo].[WordRelations]
(
	[Id] INT PRIMARY KEY NOT NULL IDENTITY,
    [WordOne] NVARCHAR(50) NOT NULL, 
    [WordTwo] NVARCHAR(50) NOT NULL, 
    [WordOneId] INT NOT NULL, 
    [WordTwoId] INT NOT NULL, 
	[PairId] as ([WordOneId] + [WordTwoId]) PERSISTED NOT NULL,	 --sharding on this, cannot be IDENTITY, it is also computed anyways
    [rScore] FLOAT NOT NULL

)
