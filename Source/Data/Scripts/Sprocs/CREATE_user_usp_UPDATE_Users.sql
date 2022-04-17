SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/09/2022
-- Description:	Updates a row by UserID in the user.Users table
-- =============================================
CREATE PROCEDURE [user].usp_UPDATE_User
	@UserID int,
	@TwitchUserID varchar(36),
	@DisplayName varchar(25),
	@ProfileImageUri varchar(1000),
	@IsAdmin bit
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		UPDATE
			[user].[Users]
		SET
			TwitchUserID = @TwitchUserID,
			DisplayName = @DisplayName,
			ProfileImageUri = @ProfileImageUri,
			IsAdmin = @IsAdmin
		WHERE
			UserID = @UserID

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
