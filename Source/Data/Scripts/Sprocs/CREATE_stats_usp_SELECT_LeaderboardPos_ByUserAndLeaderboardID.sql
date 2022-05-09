SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description: Fetch a user's Leaderboard Position by User and Board ID
-- =============================================
CREATE PROCEDURE [stats].usp_SELECT_LeaderboardPos_ByUserAndLeaderboardID
	@UserID int,
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
		[stats].[LeaderboardPos]
	WHERE
		UserID = @UserID
		AND LeaderboardID = @LeaderboardID
END
GO
