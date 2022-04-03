CREATE TABLE twitch.AccessTokens(
	-- Twitch provided unique UserID
	-- Loose FK with membership.Users
	-- Col limit long enough to support GUIDs w/ hyphens
	UserID varchar(36) NOT NULL PRIMARY KEY,
	-- Token provided by Twitch
	AccessToken varchar(50) NOT NULL,
	-- Token provided by Twitch
	RefreshToken varchar(50) NOT NULL,
)