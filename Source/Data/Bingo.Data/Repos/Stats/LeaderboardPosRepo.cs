using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Pepp.Web.Apps.Bingo.Data.Entities.Stats;

namespace Pepp.Web.Apps.Bingo.Data.Repos.Stats
{
    public interface ILeaderboardPosRepo
    {
        /// <summary>
        /// Inserts new Leaderboard position for a user into the table
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task CreateLeaderboardPosition(int userID, int leaderboardID);
        /// <summary>
        /// Get Leaderboard Position information in the table
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task<LeaderboardPosEntity> GetLeaderboardPosition(int userID, int leaderboardID);
        /// <summary>
        /// Update Leaderboard Position information in the table
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task UpdateLeaderboardPosition(int userID, int leaderboardID);
        /// <summary>
        /// Delete Leaderboard Position information in the table
        /// </summary>
        /// <param name="leaderboardID"></param>
        /// <returns></returns>
        Task DeleteAllLeaderboardPositions(int leaderboardID);
    }

    public class LeaderboardPosRepo : BaseRepo, ILeaderboardPosRepo
    {
        public LeaderboardPosRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }
        
        public async Task CreateLeaderboardPosition(int userID, int leaderboardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.LeaderboardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = leaderboardID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.UserID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = userID
                }
            };
            
            await base.Create(Sprocs.InsertLeaderboardPosition, @params);
        } 

        public async Task<LeaderboardPosEntity> GetLeaderboardPosition(int userID, int leaderboardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.LeaderboardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = leaderboardID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.UserID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = userID
                }
            };

            List<LeaderboardPosEntity> queryData = 
                await base.Read<LeaderboardPosEntity>(Sprocs.GetLeaderboardPosition, @params);
            return queryData?.SingleOrDefault();
        } 
        
        public async Task UpdateLeaderboardPosition(int userID, int leaderboardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.LeaderboardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = leaderboardID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.UserID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = userID
                }
            };
            
            await base.Update(Sprocs.UpdateLeaderboardPosition, @params);
        } 
        
        public async Task DeleteAllLeaderboardPositions(int leaderboardID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(LeaderboardPosEntity.LeaderboardID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = leaderboardID
                }
            };

            await base.Delete(Sprocs.DeleteLeaderboardPositionsByLeaderboardID, @params);
        }
        
        private struct Sprocs
        {
            public const string InsertLeaderboardPosition = "stats.usp_INSERT_LeaderboardPos";
            public const string GetLeaderboardPosition = "stats.usp_SELECT_LeaderboardPos_ByUserAndLeaderboardID";
            public const string UpdateLeaderboardPosition = "stats.usp_UPDATE_LeaderboardPos_ByUserAndLeaderboardID";
            public const string DeleteLeaderboardPositionsByLeaderboardID = "stats.usp_DELETE_LeaderboardPos_ByLeaderboardID";
        }
    }
}