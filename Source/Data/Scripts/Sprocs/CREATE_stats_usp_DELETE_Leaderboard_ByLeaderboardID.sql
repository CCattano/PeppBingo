SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description: Delete a Leaderboard
-- =============================================
CREATE PROCEDURE [stats].usp_DELETE_Leaderboard_ByLeaderboardID
	@LeaderboardID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		
		DELETE FROM
			[stats].Leaderboards
		WHERE
			LeaderboardID = @LeaderboardID

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0
			BEGIN
				ROLLBACK TRANSACTION;
			END;
		THROW;
	END CATCH
END
GO
