using Pepp.Web.Apps.Bingo.Data.Entities.User;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    }

    public class UserRepo : BaseRepo, IUserRepo
    {
        public UserRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertUser(UserEntity user)
        {
            List<SqlParameter> parameters = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(UserEntity.UserID)}",
                    SqlDbType = SqlDbType.VarChar,
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
            };

            await base.Create(StoredProcedures.InsertUser, parameters);
        }

        private struct StoredProcedures
        {
            public const string InsertUser = "usp_INSERT_user_User";
        }
    }
}
