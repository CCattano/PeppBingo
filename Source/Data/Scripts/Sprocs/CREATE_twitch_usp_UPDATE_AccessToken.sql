SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/09/2022
-- Description: Updates a row by TwitchUserID in the twitch.AccessTokens table
-- =============================================
CREATE PROCEDURE twitch.usp_UPDATE_AccessToken
	@TwitchUserID varchar(36),
	@Token varchar(50),
	@RefreshToken varchar(50)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		UPDATE
			twitch.AccessTokens
		SET
			Token = @Token,
			RefreshToken = @RefreshToken
		WHERE
			TwitchUserID = @TwitchUserID

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
