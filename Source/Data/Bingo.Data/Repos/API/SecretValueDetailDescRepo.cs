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
    public interface ISecretValueDetailDescRepo
    {
        /// <summary>
        /// Given a Source value that must come from a strongly-typed Enum,
        /// fetch all ValueDetails from the api.SecretValueDetailDescription table
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        Task<List<ValueDetailDesc>> GetValueDetailDescriptions<TEnum>(TEnum source) where TEnum : Enum;
    }

    /// <inheritdoc cref="ISecretValueDetailDescRepo"/>
    public class SecretValueDetailDescRepo : BaseRepo, ISecretValueDetailDescRepo
    {
        public SecretValueDetailDescRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        public async Task<List<ValueDetailDesc>> GetValueDetailDescriptions<TEnum>(TEnum source) where TEnum : Enum
        {
            List<SqlParameter> parameters = new()
            {
                new SqlParameter()
                {
                    ParameterName = $"@{nameof(ValueDetailDesc.Source)}",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Enum.GetName(typeof(TEnum), source)
                }
            };

            DataTable queryData =
                await base.Read(StoredProcedures.GetValueDetailDescriptionsBySource, parameters);

            if (queryData.Rows.Count == 0)
                return null;

            List<ValueDetailDesc> valueDetailDescriptions = new();

            foreach (DataRow row in queryData.Rows)
            {
                ValueDetailDesc entity = new()
                {
                    Source = row.Field<string>(nameof(ValueDetailDesc.Source)),
                    Type = row.Field<string>(nameof(ValueDetailDesc.Type)),
                    Value = row.Field<string>(nameof(ValueDetailDesc.Value)),
                    Description = row.Field<string>(nameof(ValueDetailDesc.Description))
                };
                valueDetailDescriptions.Add(entity);
            }

            return valueDetailDescriptions;
        }

        private struct StoredProcedures
        {
            public const string GetValueDetailDescriptionsBySource = "usp_SELECT_ValueDetailDescription_BySource";
        }
    }
}
