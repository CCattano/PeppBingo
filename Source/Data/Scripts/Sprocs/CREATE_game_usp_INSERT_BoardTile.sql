SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/23/2022
-- Description:	Insert row into game.BoardTiles table
-- =============================================
CREATE PROCEDURE game.usp_INSERT_BoardTile
	@TileID int OUTPUT,
	@BoardID int,
	@Text varchar(50),
	@IsActive bit,
	@CreatedBy int,
	@ModBy int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO
			[game].[BoardTiles]
           ([BoardID], [Text], [IsActive], [CreatedBy], [ModBy])
		VALUES
			(@BoardID, @Text, @IsActive, @CreatedBy, @ModBy)

		SET @TileID = SCOPE_IDENTITY();

		-- Now that a new tile has been added for a board
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
