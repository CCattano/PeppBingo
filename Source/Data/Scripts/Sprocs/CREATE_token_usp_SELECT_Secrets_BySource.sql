SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cattano, Chris
-- Create date: 04/10/2022
-- Description:	Fetch Secret details for a given Source
-- =============================================
CREATE PROCEDURE [token].[usp_SELECT_Secrets_BySource]
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
		[PeppBingo].[token].[Secrets]
	WHERE
		[Source] = @Source
END
GO
