SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/23/2022
-- Description:	Fetches a Board by its BoardID
-- =============================================
CREATE PROCEDURE game.usp_SELECT_Board_ByBoardID
	@BoardID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[BoardID],
		[Name],
		[Description],
		[TileCount],
		[CreatedDateTime],
		[CreatedBy],
		[ModDateTime],
		[ModBy]
	FROM
		[game].[Boards]
	WHERE
		[BoardID] = @BoardID
END
GO
