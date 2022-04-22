CREATE TABLE game.BoardTiles(
	TileID int NOT NULL IDENTITY(1,1),
	BoardID int NOT NULL,
	[Text] varchar(50) NOT NULL,
	CreatedDateTime datetime NOT NULL CONSTRAINT D_BoardTiles_CreateDateTime DEFAULT(GETUTCDATE()),
	CreatedBy int NOT NULL,
	ModDateTime datetime NOT NULL CONSTRAINT D_BoardTiles_ModDateTime DEFAULT(GETUTCDATE()),
	ModBy int NOT NULL,
	CONSTRAINT PK_BoardTiles_TileID PRIMARY KEY(TileID),
	CONSTRAINT FK_BoardTiles_Boards_BoardID FOREIGN KEY (BoardID) REFERENCES game.Boards(BoardID),
	-- Prevents the same tile from being added for the same board
	CONSTRAINT UNQ_Text_BoardID_Name UNIQUE([Text], BoardID),
	-- For speeding up searches for all tiles by BoardID
	INDEX IDX_BoardsTiles_BoardID (BoardID)
)