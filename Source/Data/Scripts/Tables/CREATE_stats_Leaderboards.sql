CREATE TABLE [stats].Leaderboards(
	LeaderboardID int NOT NULL IDENTITY(1, 1),
	BoardID int NOT NULL,
	CONSTRAINT PK_Leaderboards_LeaderboardID PRIMARY KEY(LeaderboardID),
	CONSTRAINT UNQ_Leaderboards_BoardID UNIQUE(BoardID),
	INDEX IDX_Leaderboards_BoardID (BoardID)
)