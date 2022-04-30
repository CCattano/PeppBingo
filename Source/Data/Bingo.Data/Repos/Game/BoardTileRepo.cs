using Pepp.Web.Apps.Bingo.Data.Entities.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Data.Repos.Game
{
    /// <summary>
    /// Repo used to interface with data stored in the game.BoardTile table
    /// </summary>
    public interface IBoardTileRepo
    {
        /// <summary>
        /// Inserts Tile information into the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertBoardTile(BoardTileEntity entity);
        /// <summary>
        /// Fetches Board Tile information stored in the table
        /// </summary>
        /// <param name="tileID"></param>
        /// <returns></returns>
        Task<BoardTileEntity> GetBoardTile(int tileID);
        /// <summary>
        /// Gets all tiles for a board from the table
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task<List<BoardTileEntity>> GetBoardTiles(int boardID);
        /// <summary>
        /// Updated Board Tile information in the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateBoardTile(BoardTileEntity entity);
        /// <summary>
        /// Delete Board Tile information in the table
        /// </summary>
        /// <param name="tileID"></param>
        /// <returns></returns>
        Task DeleteBoardTile(int tileID);
        /// <summary>
        /// Delete Board Tile information in the table
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task DeleteAllBoardTilesForBoard(int boardID);
    }

    public class BoardTileRepo : BaseRepo, IBoardTileRepo
    {
        public BoardTileRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertBoardTile(BoardTileEntity entity)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.TileID)}",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.BoardID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.Text)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = entity.Text
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.IsFreeSpace)}",
                    SqlDbType = SqlDbType.Bit,
                    Value = entity.IsFreeSpace
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.IsActive)}",
                    SqlDbType = SqlDbType.Bit,
                    Value = entity.IsActive
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.CreatedBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.CreatedBy
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.ModBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.ModBy
                }
            };

            int newPrimaryKey = await base.CreateWithPrimaryKey(Sprocs.InsertBoardTile, @params);
            entity.TileID = newPrimaryKey;
            entity.CreatedDateTime = entity.ModDateTime = DateTime.UtcNow;
        }

        public async Task<BoardTileEntity> GetBoardTile(int tileID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.TileID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = tileID
                }
            };

            List<BoardTileEntity> queryData =
                await base.Read<BoardTileEntity>(Sprocs.GetTileByTileID, @params);
            return queryData?.SingleOrDefault();
        }

        public async Task<List<BoardTileEntity>> GetBoardTiles(int boardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = boardID
                }
            };

            List<BoardTileEntity> queryData =
                await base.Read<BoardTileEntity>(Sprocs.GetAllTilesByBoardID, @params);
            return queryData;
        }

        public async Task UpdateBoardTile(BoardTileEntity entity)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.TileID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.TileID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.Text)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = entity.Text
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.IsFreeSpace)}",
                    SqlDbType = SqlDbType.Bit,
                    Value = entity.IsFreeSpace
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.IsActive)}",
                    SqlDbType = SqlDbType.Bit,
                    Value = entity.IsActive
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.ModBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.ModBy
                }
            };
            await base.Update(Sprocs.UpdateBoardTile, @params);
            entity.ModDateTime = DateTime.UtcNow;
        }

        public async Task DeleteBoardTile(int tileID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.TileID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = tileID
                }
            };

            await base.Delete(Sprocs.DeleteBoardTileByTileID, @params);
        }

        public async Task DeleteAllBoardTilesForBoard(int boardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardTileEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = boardID
                }
            };

            await base.Delete(Sprocs.DeleteBoardTilesByBoardID, @params);
        }

        private struct Sprocs
        {
            public const string InsertBoardTile = "game.usp_INSERT_BoardTile";
            public const string GetTileByTileID = "game.usp_SELECT_BoardTile_ByTileID";
            public const string GetAllTilesByBoardID = "game.usp_SELECT_BoardTiles_ByBoardID";
            public const string UpdateBoardTile = "game.usp_UPDATE_BoardTile";
            public const string DeleteBoardTileByTileID = "game.usp_DELETE_BoardTile_ByTileID";
            public const string DeleteBoardTilesByBoardID = "game.usp_DELETE_BoardTiles_ByBoardID";
        }
    }
}
