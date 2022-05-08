SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chris Cattano
-- Create date: 05/08/22
-- Description:	Select all Boards within the list of BoardIDs provided
-- =============================================
CREATE PROCEDURE [game].usp_SELECT_Boards_ByBoardIDs
	@BoardIDs [game].BoardIDs READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[BoardID],
		[Name],
		[Description],
		[TileCount],
		[CreatedDateTime],
		[CreatedBy],
		[ModDateTime],
		[ModBy]
	FROM
		[PeppBingo].[game].[Boards]
	WHERE
		BoardID IN (SELECT BoardID FROM @BoardIDs)
END
GO
