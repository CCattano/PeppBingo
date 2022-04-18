using Pepp.Web.Apps.Bingo.Data.Entities.User;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Data.Repos.User
{
    /// <summary>
    /// The repo for interacting with the user.Users table
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        /// Inserts User information into the table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task InsertUser(UserEntity user);
        /// <summary>
        /// Fetches User information from the table
        /// </summary>
        /// <param name="twitchUserID"></param>
        /// <returns></returns>
        Task<UserEntity> GetUser(string twitchUserID);
        /// <summary>
        /// Fetches User information from the table
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserEntity> GetUser(int userID);
        /// <summary>
        /// Fetches User information from the table
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        Task<List<UserEntity>> GetUsers(string displayName);
        /// <summary>
        /// Fetches all users with an IsAdmin value of 1
        /// </summary>
        /// <returns></returns>
        Task<List<UserEntity>> GetAdminUsers();
        /// <summary>
        /// Updates User information in the table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUser(UserEntity user);
    }

    /// <inheritdoc cref="IUserRepo"/>
    public class UserRepo : BaseRepo, IUserRepo
    {
        public UserRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertUser(UserEntity user)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.UserID)}",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.TwitchUserID)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.TwitchUserID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.DisplayName)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.DisplayName
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.ProfileImageUri)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.ProfileImageUri
                }
            };

            int newPrimaryKey = await base.CreateWithPrimaryKey(Sprocs.InsertUser, @params);
            user.UserID = newPrimaryKey;
        }

        public async Task<UserEntity> GetUser(string twitchUserID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.TwitchUserID)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = twitchUserID
                }
            };

            List<UserEntity> queryData =
                await base.Read<UserEntity>(Sprocs.GetUserByTwitchUserID, @params);

            return queryData?.SingleOrDefault();
        }

        public async Task<UserEntity> GetUser(int userID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.UserID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = userID
                }
            };

            List<UserEntity> queryData =
                await base.Read<UserEntity>(Sprocs.GetUserByUserID, @params);

            return queryData?.SingleOrDefault();
        }

        public async Task<List<UserEntity>> GetUsers(string displayName)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.DisplayName)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = displayName
                }
            };

            List<UserEntity> queryData =
                await base.Read<UserEntity>(Sprocs.GetUsersByDisplayName, @params);

            return queryData;
        }

        public async Task<List<UserEntity>> GetAdminUsers()
        {
            List<UserEntity> queryData = await base.Read<UserEntity>(Sprocs.GetAdminUsers);
            return queryData;
        }

        public async Task UpdateUser(UserEntity user)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.UserID)}",
                    SqlDbType = SqlDbType.Int,
                    Value = user.UserID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.TwitchUserID)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.TwitchUserID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.DisplayName)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.DisplayName
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.ProfileImageUri)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.ProfileImageUri
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.IsAdmin)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = user.IsAdmin
                }
            };

            await base.Update(Sprocs.UpdateUser, @params);
        }

    

        private struct Sprocs
        {
            public const string InsertUser = "user.usp_INSERT_User";
            public const string GetUserByTwitchUserID = "user.usp_SELECT_User_ByTwitchUserID";
            public const string GetUserByUserID = "user.usp_SELECT_User_ByUserID";
            public const string GetUsersByDisplayName = "user.usp_SELECT_Users_ByDisplayName";
            public const string GetAdminUsers = "user.usp_SELECT_Users_ByIsAdminTrue";
            public const string UpdateUser = "user.usp_UPDATE_User";
            //
        }
    }
}
