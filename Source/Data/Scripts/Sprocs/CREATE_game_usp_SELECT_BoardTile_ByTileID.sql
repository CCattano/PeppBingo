SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/23/2022
-- Description:	Select a specific row in BoardTile by TileID
-- =============================================
CREATE PROCEDURE game.usp_SELECT_BoardTile_ByTileID
	@TileID int
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
		TileID = @TileID
END
GO
