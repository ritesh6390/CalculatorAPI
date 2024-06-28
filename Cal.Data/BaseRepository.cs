using Cal.Model.Config;
using Microsoft.Extensions.Options;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace Cal.Data
{
    public abstract class BaseRepository
    {
        #region Fields
        public readonly IOptions<DataConfig> _connectionString;
        #endregion

        #region Constructor
        public BaseRepository(IOptions<DataConfig> connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region SQL Methods

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string decryptedConn = _connectionString.Value.DefaultConnection;
            using (SqlConnection con = new SqlConnection(decryptedConn))
            {
                await con.OpenAsync();
                try
                {
                    return await con.QueryFirstOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string decryptedConn = _connectionString.Value.DefaultConnection;
            using (SqlConnection con = new SqlConnection(decryptedConn))
            {
                await con.OpenAsync();
                return await con.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> ExecuteAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string decryptedConn = _connectionString.Value.DefaultConnection;
            using (SqlConnection con = new SqlConnection(decryptedConn))
            {
                await con.OpenAsync();
                return await con.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string decryptedConn = _connectionString.Value.DefaultConnection;
            using (SqlConnection con = new SqlConnection(decryptedConn))
            {
                await con.OpenAsync();
                return await con.QueryMultipleAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        #endregion
    }
}
