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

		DECLARE @BoardID int = (SELECT TOP(1) BoardID FROM game.BoardTiles WHERE TileID = @TileID);

		DELETE FROM
			game.BoardTiles
		WHERE
			TileID = @TileID;

		-- Now that a new tile has been deleted for a board
		-- We need to update that board's tile count
		UPDATE
			game.Boards
		SET
			TileCount = (
				SELECT
					COUNT(*)
				FROM
					game.BoardTiles
				WHERE
					BoardID = @BoardID
					AND IsActive = 1
			)
		WHERE
			BoardID = @BoardID;

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
