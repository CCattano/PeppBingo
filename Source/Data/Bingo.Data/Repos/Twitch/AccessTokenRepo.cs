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
        Task InsertTwitchAccessToken(AccessTokenEntity accessToken);
    }

    /// <inheritdoc cref="IAccessTokenRepo"/>
    public class AccessTokenRepo : BaseRepo, IAccessTokenRepo
    {
        public AccessTokenRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertTwitchAccessToken(AccessTokenEntity accessToken)
        {
            List<SqlParameter> parameters = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessTokenEntity.TwitchUserID)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = accessToken.TwitchUserID
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessTokenEntity.Token)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = accessToken.Token
                },
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessTokenEntity.RefreshToken)}",
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
