using Pepp.Web.Apps.Bingo.Data.Entities.Twitch;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
        Task InsertAccessToken(AccessTokenEntity accessToken);
        /// <summary>
        /// Fetches Twitch-provided access token information from the table
        /// </summary>
        /// <param name="twitchUserID"></param>
        /// <returns></returns>
        Task<AccessTokenEntity> GetAccessToken(string twitchUserID);
        /// <summary>
        /// Updates Twitch-provided access token information in the table
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task UpdateAccessToken(AccessTokenEntity accessToken);
    }

    /// <inheritdoc cref="IAccessTokenRepo"/>
    public class AccessTokenRepo : BaseRepo, IAccessTokenRepo
    {
        public AccessTokenRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task InsertAccessToken(AccessTokenEntity accessToken)
        {
            List<SqlParameter> @params = new()
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

            await base.Create(Sprocs.InsertAccessToken, @params);
        }

        public async Task<AccessTokenEntity> GetAccessToken(string twitchUserID)
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(AccessTokenEntity.TwitchUserID)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = twitchUserID
                }
            };

            List<AccessTokenEntity> queryData = 
                await base.Read<AccessTokenEntity>(Sprocs.GetAccessTokenByUserID, @params);

            return queryData?.SingleOrDefault();
        }

        public async Task UpdateAccessToken(AccessTokenEntity accessToken)
        {
            List<SqlParameter> @params = new()
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

            await base.Update(Sprocs.UpdateAccessToken, @params);
        }

        private struct Sprocs
        {
            public const string InsertAccessToken = "twitch.usp_INSERT_AccessToken";
            public const string GetAccessTokenByUserID = "twitch.usp_SELECT_AccessToken_ByTwitchUserID";
            public const string UpdateAccessToken = "twitch.usp_UPDATE_AccessToken";
        }
    }
}
