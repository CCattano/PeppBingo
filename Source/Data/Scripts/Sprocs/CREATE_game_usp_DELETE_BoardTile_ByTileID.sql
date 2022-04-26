SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/26/2022
-- Description: Delete a single board tile by TileID
-- =============================================
CREATE PROCEDURE game.usp_DELETE_BoardTile_ByTileID
	@TileID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		
		DELETE FROM
			game.BoardTiles
		WHERE
			TileID = @TileID;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0
			BEGIN
				ROLLBACK TRANSACTION;
			END;
		THROW;
	END CATCH
END
GO
