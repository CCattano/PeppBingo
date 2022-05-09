SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/09/2022
-- Description: Create a Leaderboard
-- =============================================
CREATE PROCEDURE [stats].usp_INSERT_Leaderboard
	@LeaderboardID int OUTPUT,
	@BoardID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO
			[stats].[Leaderboards]
           ([BoardID])
		VALUES
			(@BoardID)

		SET @LeaderboardID = SCOPE_IDENTITY();
	
		COMMIT TRANSACTION

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
