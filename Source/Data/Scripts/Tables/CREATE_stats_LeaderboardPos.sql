CREATE TABLE [stats].LeaderboardPos(
	LeaderboardPosID int NOT NULL IDENTITY(1, 1),
	LeaderboardID int NOT NULL 
		CONSTRAINT 
			FK_LeaderboardPos_Leaderboard_LeaderBoardID 
		FOREIGN KEY REFERENCES 
			[stats].Leaderboard(LeaderboardID),
	UserID int NOT NULL,
	BingoQty int NOT NULL CONSTRAINT D_Leaderboard_BingoQty DEFAULT(0),
	CONSTRAINT PK_LeaderboardPos_LeaderboardPosID PRIMARY KEY(LeaderboardPosID),
	CONSTRAINT UNQ_LeaderboardPos_LeaderboardID_UserID UNIQUE(LeaderboardID, UserID),
	INDEX IDX_LeaderboardPos_UserID (UserID)
)