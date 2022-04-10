using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Data.Repos.API
{
    /// <summary>
    /// Repo used to interface with data stored in the api.SecretValueDetailDescription table
    /// </summary>
    public interface ISecretRepo
    {
        /// <summary>
        /// Given a Source value that must come from a strongly-typed Enum,
        /// fetch all ValueDetails from the api.SecretValueDetailDescription table
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<List<SecretEntity>> GetSecrets<TEnum>(TEnum source) where TEnum : Enum;
    }

    /// <inheritdoc cref="ISecretRepo"/>
    public class SecretRepo : BaseRepo, ISecretRepo
    {
        public SecretRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task<List<SecretEntity>> GetSecrets<TEnum>(TEnum source) where TEnum : Enum
        {
            List<SqlParameter> @params = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(SecretEntity.Source)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Enum.GetName(typeof(TEnum), source)
                }
            };

            List<SecretEntity> valueDetailDescriptions =
                await base.Read<SecretEntity>(Sprocs.GetAPISecretsBySource, @params);

            return valueDetailDescriptions;
        }

        private struct Sprocs
        {
            public const string GetAPISecretsBySource = "api.usp_SELECT_Secrets_BySource";
        }
    }
}
