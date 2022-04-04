SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/04/2022
-- Description:	Insert a user into the users.User table
-- =============================================
CREATE PROCEDURE usp_INSERT_user_User
	@UserID [int],
	@TwitchUserID [varchar](36),
	@DisplayName [varchar](25),
	@ProfileImageUri [varchar](1000)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO
			[user].[Users]
			(UserID, TwitchUserID, DisplayName, ProfileImageUri)
		VALUES
			(@UserID, @TwitchUserID, @DisplayName, @ProfileImageUri)

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
