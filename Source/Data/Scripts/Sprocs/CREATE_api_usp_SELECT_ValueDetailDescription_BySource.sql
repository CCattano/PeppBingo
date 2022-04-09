SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cattano, Chris
-- Create date: 04/02/2022
-- Description:	Fetch ValueDetailDescriptions for a given Source
-- =============================================
CREATE PROCEDURE usp_SELECT_ValueDetailDescription_BySource
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
