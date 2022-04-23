SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/23/22
-- Description:	Select all users within the list of UserIDs provided
-- =============================================
CREATE PROCEDURE [user].usp_SELECT_Users_ByUserIDs
	@UserIDs [user].UserIDs READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[UserID],
		[TwitchUserID],
		[DisplayName],
		[ProfileImageUri],
		[IsAdmin]
	FROM
		[user].[Users]
	WHERE
		UserID IN (SELECT UserID FROM @UserIDs)
END
GO
