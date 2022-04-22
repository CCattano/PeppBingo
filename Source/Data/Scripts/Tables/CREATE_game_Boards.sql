CREATE TABLE game.Boards(
	BoardID int NOT NULL IDENTITY(1,1),
	[Name] varchar(50) NOT NULL,
	[Description] varchar(150) NOT NULL,
	CreatedDateTime datetime NOT NULL CONSTRAINT D_Boards_CreateDateTime DEFAULT(GETUTCDATE()),
	CreatedBy int NOT NULL,
	ModDateTime datetime NOT NULL CONSTRAINT D_Boards_ModDateTime DEFAULT(GETUTCDATE()),
	ModBy int NOT NULL,
	CONSTRAINT PK_Boards_BoardID PRIMARY KEY(BoardID),
	-- Prevents Boards of the same name from being added
	CONSTRAINT UNQ_Boards_Name UNIQUE([Name])
)