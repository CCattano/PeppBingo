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
    /// Repo used to interface with data stored in the game.Board table
    /// </summary>
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
        Task<List<BoardEntity>> GetBoards();

        /// <summary>
        /// Fetches all Boards with BoardIDs in the
        /// <paramref name="boardIDs"/> list provided
        /// </summary>
        /// <returns></returns>
        Task<List<BoardEntity>> GetBoards(List<int> boardIDs);

        /// <summary>
        /// Updated Board information in the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateBoard(BoardEntity entity);

        /// <summary>
        /// Delete Board information in the table
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task DeleteBoard(int boardID);
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

        public async Task<List<BoardEntity>> GetBoards()
        {
            List<BoardEntity> queryData =
                await base.Read<BoardEntity>(Sprocs.GetAllBoards);
            return queryData;
        }

        public async Task<List<BoardEntity>> GetBoards(List<int> boardIDs)
        {
            string sprocParamName = nameof(boardIDs)[..1].ToUpper() + nameof(boardIDs)[1..];
            string udtName = $"game.{sprocParamName}";
            string udtColName = sprocParamName[..^1];

            DataTable udt = new();
            udt.Columns.Add(udtColName, typeof(int));
            boardIDs.ForEach(id =>
            {
                DataRow row = udt.NewRow();
                row.SetField(udtColName, id);
                udt.Rows.Add(row);
            });

            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{sprocParamName}",
                    TypeName = udtName,
                    Value = udt
                }
            };

            List<BoardEntity> queryData =
                await base.Read<BoardEntity>(Sprocs.GetBoardsByUserIDs, @params);

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

        public async Task DeleteBoard(int boardID)
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

            await base.Delete(Sprocs.DeleteBoardByBoardID, @params);
        }

        private struct Sprocs
        {
            public const string InsertBoard = "game.usp_INSERT_Board";
            public const string GetBoardByBoardID = "game.usp_SELECT_Board_ByBoardID";
            public const string GetAllBoards = "game.usp_SELECT_AllBoards";
            public const string GetBoardsByUserIDs = "game.usp_SELECT_Boards_ByBoardIDs";
            public const string UpdateBoard = "game.usp_UPDATE_Board";
            public const string DeleteBoardByBoardID = "game.usp_DELETE_Board_ByBoardID";
        }
    }
}