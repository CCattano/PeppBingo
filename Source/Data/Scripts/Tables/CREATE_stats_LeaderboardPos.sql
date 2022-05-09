CREATE TABLE [stats].LeaderboardPos(
	LeaderboardPosID int NOT NULL IDENTITY(1, 1),
	LeaderboardID int NOT NULL 
		CONSTRAINT 
			FK_LeaderboardPos_Leaderboard_LeaderBoardID 
		FOREIGN KEY REFERENCES 
			[stats].Leaderboards(LeaderboardID),
	UserID int NOT NULL,
	-- If a row is being created it's b/c someone previously 
	-- not on the leaderboard has gotten their first bingo
	-- So when we are inserting a row we want to default to
	--a BingoQty of 1, that's why we're here in this sproc
	BingoQty int NOT NULL CONSTRAINT D_LeaderboardPos_BingoQty DEFAULT(1),
	CONSTRAINT PK_LeaderboardPos_LeaderboardPosID PRIMARY KEY(LeaderboardPosID),
	CONSTRAINT UNQ_LeaderboardPos_LeaderboardID_UserID UNIQUE(LeaderboardID, UserID),
	INDEX IDX_LeaderboardPos_UserID (UserID)
)