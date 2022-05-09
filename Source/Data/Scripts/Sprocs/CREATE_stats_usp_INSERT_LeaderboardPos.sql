SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description: Insert new leaderboard position information for a user
-- =============================================
CREATE PROCEDURE [stats].usp_INSERT_LeaderboardPos
	@LeaderboardID int,
	@UserID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		
		INSERT INTO
			[stats].[LeaderboardPos]
			(LeaderboardID, UserID)
		VALUES
			(@LeaderboardID, @UserID)

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
