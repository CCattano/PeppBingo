SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/23/2022
-- Description:	Update an existing BoardTile
-- =============================================
CREATE PROCEDURE game.usp_UPDATE_BoardTile
	@TileID int,
	@Text varchar(50),
	@IsFreeSpace bit,
	@IsActive bit,
	@ModBy int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @BoardID int = (SELECT TOP (1) BoardID FROM game.BoardTiles WHERE TileID = @TileID);

		-- Can only have one free space
		IF @IsFreeSpace = 1
			UPDATE
				game.BoardTiles
			SET
				IsFreeSpace = 0
			WHERE
				BoardID = @BoardID
				AND IsFreeSpace = 1

		UPDATE
			[game].[BoardTiles]
		SET
			[Text] = @Text,
			[IsFreeSpace] = @IsFreeSpace,
			[IsActive] = @IsActive,
			[ModDateTime] = GETUTCDATE(),
			[ModBy] = @ModBy
		WHERE
			[TileID] = @TileID;

		-- When a tile has been updated update the
		-- Tile Count for its associated board to ensure parity
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
			BoardID = @BoardID

		COMMIT TRANSACTION
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
