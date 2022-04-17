SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/16/2022
-- Description: Fetches users who's DisplayName contains
--				the substring provided in the sproc param
-- =============================================
CREATE PROCEDURE [user].usp_SELECT_Users_ByDisplayName
	@DisplayName varchar(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		[UserID]
		[TwitchUserID],
		[DisplayName],
		[ProfileImageUri],
		[IsAdmin]
	FROM
		[PeppBingo].[user].[Users]
	WHERE
		DisplayName LIKE '%' + @DisplayName + '%'
END
GO
