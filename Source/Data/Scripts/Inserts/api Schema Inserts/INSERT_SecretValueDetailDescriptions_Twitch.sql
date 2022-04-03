USE [PeppBingo]
GO

INSERT INTO
	[api].[SecretValueDetailDescription]
	([Source], [Type], [Value], [Description])
VALUES
	('Twitch', 'ClientID', '[REDACTED_FOR_VC_COMMIT]', 'Provided by Twitch. Passed to authorization endpoints to identify the Pepp Bingo application'),
	('Twitch', 'ClientSecret', '[REDACTED_FOR_VC_COMMIT]', 'Provided by Twitch. Passed to the token exchange endpoints to obtain a token. You must keep this confidential.')
GO


