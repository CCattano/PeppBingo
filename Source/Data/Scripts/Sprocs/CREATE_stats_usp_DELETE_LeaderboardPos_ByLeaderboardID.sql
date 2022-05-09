SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description: Delete all leaderboard positions for a given LeaderboardID
-- =============================================
CREATE PROCEDURE [stats].usp_DELETE_LeaderboardPos_ByLeaderboardID
	@LeaderboardID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		
		DELETE FROM
			[stats].LeaderboardPos
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
