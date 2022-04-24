using Pepp.Web.Apps.Bingo.Data.Entities.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Data.Repos.Game
{
    public interface IBoardRepo
    {
        /// <summary>
        /// Inserts Board information into the table
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        Task InsertBoard(BoardEntity board);
        /// <summary>
        /// Fetches Board information stored in the table
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task<BoardEntity> GetBoard(int boardID);
        /// <summary>
        /// Fetches all Board information in the table
        /// </summary>
        /// <returns></returns>
        Task<List<BoardEntity>> GetAllBoards();
        /// <summary>
        /// Updated Board information in the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateBoard(BoardEntity entity);
    }

    public class BoardRepo : BaseRepo, IBoardRepo
    {
        public BoardRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertBoard(BoardEntity entity)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.Name)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = entity.Name
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.Description)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = entity.Description
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.CreatedBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.CreatedBy
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.ModBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.ModBy
                }
            };

            int newPrimaryKey = await base.CreateWithPrimaryKey(Sprocs.InsertBoard, @params);
            entity.BoardID = newPrimaryKey;
            entity.CreatedDateTime = entity.ModDateTime = DateTime.UtcNow;
        }

        public async Task<BoardEntity> GetBoard(int boardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = boardID
                }
            };

            List<BoardEntity> queryData =
                await base.Read<BoardEntity>(Sprocs.GetBoardByBoardID, @params);
            return queryData?.SingleOrDefault();
        }

        public async Task<List<BoardEntity>> GetAllBoards()
        {
            List<BoardEntity> queryData =
                await base.Read<BoardEntity>(Sprocs.GetAllBoards);
            return queryData;
        }

        public async Task UpdateBoard(BoardEntity entity)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.BoardID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.Name)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = entity.Name
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.Description)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = entity.Description
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.ModBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.ModBy
                }
            };
            await base.Update(Sprocs.UpdateBoard, @params);
            entity.ModDateTime = DateTime.UtcNow;
        }

        private struct Sprocs
        {
            public const string InsertBoard = "game.usp_INSERT_Board";
            public const string GetBoardByBoardID = "game.usp_SELECT_Board_ByBoardID";
            public const string GetAllBoards = "game.usp_SELECT_AllBoards";
            public const string UpdateBoard = "game.usp_UPDATE_Board";
        }
    }
}
