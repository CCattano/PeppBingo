USE [PeppBingo]
GO

INSERT INTO
	[token].[Secrets]
	([Source], [Type], [Value], [Description])
VALUES
	('JWT', 'SHA256Key', '[REDACTED_FOR_VC_COMMIT]', 'NEVER share this value with ANYONE! The initialization value for the SHA256 signing protocol.'),
	('JWT', 'SigningSecret', '[REDACTED_FOR_VC_COMMIT]', 'NEVER share this value with ANYONE! The signing secret applied to all calculated signatures.')