SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/23/2022
-- Description:	Fetch all board tiles for a specific board
-- =============================================
CREATE PROCEDURE game.usp_SELECT_BoardTiles_ByBoardID
	@BoardID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[TileID],
		[BoardID],
		[Text],
		[IsActive],
		[CreatedDateTime],
		[CreatedBy],
		[ModDateTime],
		[ModBy]
	FROM
		[game].[BoardTiles]
	WHERE
		BoardID = @BoardID
END
GO
