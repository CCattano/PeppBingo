SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/22/2022
-- Description:	Fetches all rows in table
-- =============================================
CREATE PROCEDURE [stats].usp_SELECT_AllLeaderboards
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[LeaderboardID],
		[BoardID]
	FROM
		[stats].[Leaderboard]
END
GO
