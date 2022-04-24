SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/22/2022
-- Description:	Update an existing Bingo Board
-- =============================================
CREATE PROCEDURE [game].usp_UPDATE_Board
	@BoardID int,
	@Name varchar(50),
	@Description varchar(150),
	@ModBy int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		UPDATE
			[game].[Boards]
		SET
			[Name] = @Name,
			[Description] = @Description,
			[ModBy] = @ModBy
		WHERE
			BoardID = @BoardID;

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
