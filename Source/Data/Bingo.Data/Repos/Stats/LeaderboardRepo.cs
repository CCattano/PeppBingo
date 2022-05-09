using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;

namespace Pepp.Web.Apps.Bingo.Data.Repos.Stats
{
    /// <summary>
    /// Repo used to interface with data stored in the game.Board table
    /// </summary>
    public interface ILeaderboardRepo
    {
        /// <summary>
        /// Inserts Leaderboard information into the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertLeaderboard(LeaderboardEntity entity);

        /// <summary>
        /// Fetches all Leaderboard information stored in the table
        /// </summary>
        /// <returns></returns>
        Task<List<LeaderboardEntity>> GetAllLeaderboards();

        /// <summary>
        /// Get Leaderboard information stored in the table
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns></returns>
        Task<LeaderboardEntity> GetLeaderboard(int boardID);

        /// <summary>
        /// Delete Leaderboard information in the table
        /// </summary>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task DeleteLeaderboard(int leaderboardID);
    }

    /// <inheritdoc cref="ILeaderboardRepo"/>
    public class LeaderboardRepo : BaseRepo, ILeaderboardRepo
    {
        public LeaderboardRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertLeaderboard(LeaderboardEntity entity)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardEntity.LeaderboardID)}",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = entity.BoardID
                }
            };

            int newPrimaryKey = await base.CreateWithPrimaryKey(Sprocs.InsertLeaderboard, @params);
            entity.LeaderboardID = newPrimaryKey;
        }

        public async Task<List<LeaderboardEntity>> GetAllLeaderboards()
        {
            List<LeaderboardEntity> queryData =
                await base.Read<LeaderboardEntity>(Sprocs.GetAllLeaderboards);
            return queryData;
        }

        public async Task<LeaderboardEntity> GetLeaderboard(int boardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardEntity.BoardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = boardID
                }
            };

            List<LeaderboardEntity> queryData =
                await base.Read<LeaderboardEntity>(Sprocs.GetLeaderboardByBoardID, @params);
            return queryData?.SingleOrDefault();
        }

        public async Task DeleteLeaderboard(int leaderboardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardEntity.LeaderboardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = leaderboardID
                }
            };

            await base.Delete(Sprocs.DeleteLeaderboardByLeaderboardID, @params);
        }

        private struct Sprocs
        {
            public const string InsertLeaderboard = "stats.usp_INSERT_Leaderboard";
            public const string GetAllLeaderboards = "stats.usp_SELECT_AllLeaderboards";
            public const string GetLeaderboardByBoardID = "stats.usp_SELECT_Leaderboard_ByBoardID";
            public const string DeleteLeaderboardByLeaderboardID = "stats.usp_DELETE_Leaderboard_ByLeaderboardID";
        }
    }
}