using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Data.Repos.Twitch
{
    /// <summary>
    /// The repo for interacting with the twitch.AccessTokens table
    /// </summary>
    public interface IAccessTokenRepo
    {
        /// <summary>
        /// Inserts Twitch-provided access token information into the table
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task InsertTwitchAccessToken(AccessToken accessToken);
    }

    /// <inheritdoc cref="IAccessTokenRepo"/>
    public class AccessTokenRepo : BaseRepo, IAccessTokenRepo
    {
        public AccessTokenRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertTwitchAccessToken(AccessToken accessToken)
        {
            List<SqlParameter> parameters = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessToken.UserID)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = accessToken.UserID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessToken.Token)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = accessToken.Token
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessToken.RefreshToken)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = accessToken.RefreshToken
                }
            };

            await base.Create(StoredProcedures.InsertAccessToken, parameters);
        }

        private struct StoredProcedures
        {
            public const string InsertAccessToken = "usp_INSERT_twitch_AccessToken";
        }
    }
}
