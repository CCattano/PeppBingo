SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cattano, Chris
-- Create date: 04/02/2022
-- Description:	Fetch Secret details for a given Source
-- =============================================
ALTER PROCEDURE api.usp_SELECT_Secrets_BySource
	@Source varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Source],
		[Type],
		[Value],
		[Description]
	FROM
		[PeppBingo].[api].[SecretValueDetailDescription]
	WHERE
		[Source] = @Source
END
GO
