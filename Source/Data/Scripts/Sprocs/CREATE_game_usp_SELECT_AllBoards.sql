SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/22/2022
-- Description:	Fetches all rows in table
-- =============================================
CREATE PROCEDURE game.usp_SELECT_AllBoards
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[BoardID],
		[Name],
		[Description],
		[CreatedDateTime],
		[CreatedBy],
		[ModDateTime],
		[ModBy]
	FROM
		[game].[Boards]
END
GO
