SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/17/2022
-- Description:	Get all Leaderboard Positions for a given LeaderboardID
-- =============================================
CREATE PROCEDURE [stats].usp_SELECT_LeaderBoardPos_ByLeaderboardID
	@LeaderboardID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[LeaderboardPosID],
		[LeaderboardID],
		[UserID],
		[BingoQty]
	FROM
		[PeppBingo].[stats].[LeaderboardPos]
	WHERE
		LeaderboardID = @LeaderboardID
END
GO
