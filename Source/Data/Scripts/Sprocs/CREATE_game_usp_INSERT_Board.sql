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
	@Name varchar(50),
	@Description varchar(150),
	@CreatedBy int,
	@ModBy int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		INSERT INTO
			[game].[Boards]
			([Name], [Description], [CreatedBy], [ModBy])
		VALUES
			(@Name, @Description, @CreatedBy, @ModBy);

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
