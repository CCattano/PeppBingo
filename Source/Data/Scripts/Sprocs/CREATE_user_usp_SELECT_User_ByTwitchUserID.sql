SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/09/2022
-- Description:	Fetches a User from user.Users by TwitchUserID
-- =============================================
CREATE PROCEDURE [user].[usp_SELECT_User_ByTwitchUserID]
	@TwitchUserID varchar(36)
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
		TwitchUserID = @TwitchUserID
END
GO


