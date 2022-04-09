CREATE TABLE twitch.AccessTokens(
	-- Twitch provided unique UserID
	-- Loose FK with membership.Users
	-- Col limit long enough to support GUIDs w/ hyphens
	TwitchUserID varchar(36) NOT NULL PRIMARY KEY,
	-- Access Token provided by Twitch
	Token varchar(50) NOT NULL,
	-- Token provided by Twitch
	RefreshToken varchar(50) NOT NULL,
)