SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/15/2022
-- Description:	Fetches a User from user.Users by UserID
-- =============================================
CREATE PROCEDURE [user].[usp_SELECT_User_ByUserID]
	@UserID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[UserID],
		[TwitchUserID],
		[DisplayName],
		[ProfileImageUri]
	FROM
		[user].Users
	WHERE
		[UserID] = @UserID
END
GO
