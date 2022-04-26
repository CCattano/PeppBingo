using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.Data.Repos
{
    public abstract class BaseRepo
    {
        private readonly BaseDataService _dataSvc;

        public BaseRepo(BaseDataService dataSvc) => _dataSvc = dataSvc;

        protected virtual async Task<int> CreateWithPrimaryKey(string sproc, List<SqlParameter> sprocParams)
        {
            SqlParameter outputParam = sprocParams.FirstOrDefault(param => param.Direction == ParameterDirection.Output);
            if(outputParam == null)
                throw new InvalidOperationException("Cannot insert table row utlizing PrimaryKey when no Output parameter is specified.");
            using SqlConnection conn = _dataSvc.GetConnection();
            await conn.OpenAsync();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            cmd.Parameters.AddRange(sprocParams.ToArray());
            await cmd.ExecuteNonQueryAsync();
            int newPrimaryKey = (int)outputParam.Value;
            await conn.CloseAsync();
            return newPrimaryKey;
        }

        protected virtual async Task Create(string sproc, List<SqlParameter> sprocParams)
        {
            using SqlConnection conn = _dataSvc.GetConnection();
            await conn.OpenAsync();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            cmd.Parameters.AddRange(sprocParams.ToArray());
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        protected virtual async Task<List<T>> Read<T>(string sproc, List<SqlParameter> sprocParams = null) where T : new()
        {
            using SqlConnection conn = _dataSvc.GetConnection();
            await conn.OpenAsync();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            if(sprocParams != null)
                cmd.Parameters.AddRange(sprocParams.ToArray());
            using SqlDataReader queryResults = await cmd.ExecuteReaderAsync();
            DataTable resultTable = new();
            resultTable.Load(queryResults);
            await conn.CloseAsync();
            List<T> response = ParseQueryData<T>(resultTable);
            return response;
        }

        protected virtual async Task Update(string sproc, List<SqlParameter> sprocParams)
        {
            using SqlConnection conn = _dataSvc.GetConnection();
            await conn.OpenAsync();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            cmd.Parameters.AddRange(sprocParams.ToArray());
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        protected virtual async Task Delete(string sproc, List<SqlParameter> sprocParams)
        {
            using SqlConnection conn = _dataSvc.GetConnection();
            await conn.OpenAsync();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            cmd.Parameters.AddRange(sprocParams.ToArray());
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        private static List<T> ParseQueryData<T>(DataTable data) where T : new()
        {
            if (data.Rows.Count == 0)
                return null;

            List<T> response = new();

            FieldInfo[] fields = typeof(T).GetFields();

            foreach (DataRow row in data.Rows)
            {
                T entity = new();
                foreach (FieldInfo field in fields)
                {
                    object value = row.Field<object>(field.Name);
                    field.SetValue(entity, value);
                }
                response.Add(entity);
            }

            return response;
        }
    }
}