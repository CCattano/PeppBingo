SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 04/22/2022
-- Description:	Inserts a new Board row into the game.Boards table
-- =============================================
CREATE PROCEDURE game.usp_INSERT_Board
	@BoardID int OUTPUT,
	@Name varchar,
	@Description varchar,
	@CreatedDateTime datetime,
	@CreatedBy int,
	@ModDateTime datetime,
	@ModBy int
AS
BEGIN	
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO
			[game].[Boards]
			([Name], [Description], [CreatedDateTime], [CreatedBy], [ModDateTime], [ModBy])
		VALUES
			(@Name, @Description, @CreatedDateTime, @CreatedBy, @ModDateTime, @ModBy);

		COMMIT TRANSACTION

		SET @BoardID = SCOPE_IDENTITY();
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
