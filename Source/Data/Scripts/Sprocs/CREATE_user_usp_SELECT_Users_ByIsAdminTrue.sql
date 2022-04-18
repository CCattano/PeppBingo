SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/18/2022
-- Description:	Fetches all users with a IsAdmin value of 1
-- =============================================
CREATE PROCEDURE [user].usp_SELECT_Users_ByIsAdminTrue
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
		IsAdmin = 1
END
GO
