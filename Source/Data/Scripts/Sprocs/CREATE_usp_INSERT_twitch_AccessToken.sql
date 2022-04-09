SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/03/2022
-- Description:	Inserts Twitch Access Token data into the twitch.AccessTokens table
-- =============================================
CREATE PROCEDURE usp_INSERT_twitch_AccessToken
	@TwitchUserID varchar(36),
	@Token varchar(50),
	@RefreshToken varchar(50)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO
			twitch.AccessTokens
			(TwitchUserID, Token, RefreshToken)
		VALUES
			(@TwitchUserID, @Token, @RefreshToken)

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
