CREATE TABLE [stats].Leaderboard(
	LeaderboardID int NOT NULL IDENTITY(1, 1),
	BoardID int NOT NULL,
	CONSTRAINT PK_Leaderboard_LeaderboardID PRIMARY KEY(LeaderboardID),
	CONSTRAINT UNQ_Leaderboard_BoardID UNIQUE(BoardID),
	INDEX IDX_Leaderboard_BoardID (BoardID)
)