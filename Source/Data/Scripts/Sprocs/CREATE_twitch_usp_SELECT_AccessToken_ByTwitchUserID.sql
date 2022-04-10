SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/09/2022
-- Description:	Fetches an AccessToken from twitch.AccessTokens by TwitchUserID
-- =============================================
CREATE PROCEDURE twitch.usp_SELECT_AccessToken_ByTwitchUserID
	@TwitchUserID varchar(36)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[TwitchUserID],
		[Token],
		[RefreshToken]
	FROM
		twitch.AccessTokens
	WHERE
		TwitchUserID = @TwitchUserID
END
GO
