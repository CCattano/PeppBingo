CREATE TABLE [user].Users(
	-- Internal UserID
	-- used in JWT's to identify users
	-- Ensures we do not expose sensitive Twitch information in the browser
	UserID int NOT NULL IDENTITY(1, 1),
	-- Twitch provided unique UserID
	-- Loose FK with twitch.AccessTokens
	-- Never returned to browser, only used internally
	TwitchUserID varchar(36) NOT NULL,
	-- Twitch provided display name
	DisplayName varchar(25) NOT NULL,
	-- Twitch provided URI to fetch user profile image
	ProfileImageUri varchar(1000) NOT NULL,
	-- Whether or not this user is an application administrator
	IsAdmin bit NOT NULL CONSTRAINT D_Users_IsAdmin DEFAULT (0),
	CONSTRAINT PK_Users_UserID PRIMARY KEY (UserID),
	CONSTRAINT UNQ_Users_UserID_TwitchUserID UNIQUE (UserID, TwitchUserID)
)