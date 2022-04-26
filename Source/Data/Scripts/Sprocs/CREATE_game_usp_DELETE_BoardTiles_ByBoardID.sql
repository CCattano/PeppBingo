SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/26/2022
-- Description: Delete all board tiles for a given BoardID
-- =============================================
CREATE PROCEDURE game.usp_DELETE_BoardTiles_ByBoardID
	@BoardID int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
		
		DELETE FROM
			game.BoardTiles
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
