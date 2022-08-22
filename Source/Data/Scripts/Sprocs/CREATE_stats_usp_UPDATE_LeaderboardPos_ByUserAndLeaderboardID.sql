SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description: Update a user's BingoQty for a leaderboard they are on
-- =============================================
CREATE PROCEDURE [stats].usp_UPDATE_LeaderboardPos_ByUserAndLeaderboardID
	@UserID int,
	@LeaderboardID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		
		UPDATE
			[stats].[LeaderboardPos]
		SET
			BingoQty = BingoQty + 1,
			LastBingoDateTime = GETUTCDATE()
		WHERE
			UserID = @UserID
			AND LeaderboardID = @LeaderboardID

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
