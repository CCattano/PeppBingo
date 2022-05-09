SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description:	Fetch Leaderboard data by BoardID
-- =============================================
CREATE PROCEDURE [stats].usp_SELECT_Leaderboard_ByBoardID
	@BoardID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[LeaderboardID],
		[BoardID]
	FROM
		[stats].[Leaderboards]
	WHERE
		BoardID = @BoardID
END
GO
