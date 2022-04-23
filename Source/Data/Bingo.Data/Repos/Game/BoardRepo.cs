using Pepp.Web.Apps.Bingo.Data.Entities.Game;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        /// Fetches all Board information in the table
        /// </summary>
        /// <returns></returns>
        Task<List<BoardEntity>> GetAllBoards();
    }

    public class BoardRepo : BaseRepo, IBoardRepo
    {
        public BoardRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertBoard(BoardEntity board)
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
                    Value = board.Name
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.Description)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = board.Description
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.CreatedBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = board.CreatedBy
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(BoardEntity.ModBy)}",
                    SqlDbType = SqlDbType.Int,
                    Value = board.ModBy
                }
            };

            int newPrimaryKey = await base.CreateWithPrimaryKey(Sprocs.InsertBoard, @params);
            board.BoardID = newPrimaryKey;
            board.CreatedDateTime = board.ModDateTime = DateTime.UtcNow;
        }

        public async Task<List<BoardEntity>> GetAllBoards()
        {
            List<BoardEntity> queryData =
                await base.Read<BoardEntity>(Sprocs.GetAllBoards);
            return queryData;
        }


        private struct Sprocs
        {
            public const string InsertBoard = "game.usp_INSERT_Board";
            public const string GetAllBoards = "game.usp_SELECT_AllBoards";
        }
    }
}
