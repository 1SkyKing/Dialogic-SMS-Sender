using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Dlgc.Data.DataEngine
{
    public class DataCommandAsync
    {
        private SqlCommand _cmd;
        internal static string Provider { get; set; }
        public DataCommandAsync(string strCommand, CommandType cmdType, DbContext context)
        {
            _cmd =(SqlCommand)context.Database.GetDbConnection().CreateCommand();
            _cmd.CommandType = cmdType;
            _cmd.CommandText = strCommand;
        }
        public async Task<int> ExecuteNonQueryAsync()
        {
            try
            {
                if (_cmd.Connection.State == ConnectionState.Open)
                {
                    return await _cmd.ExecuteNonQueryAsync();
                }
                else
                {
                    await _cmd.Connection.OpenAsync();
                    return await _cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message;
                throw ex;
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync()
        {
            try
            {
                if (_cmd.Connection.State == ConnectionState.Open)
                {
                    return await _cmd.ExecuteReaderAsync();
                }
                else
                {
                    await _cmd.Connection.OpenAsync();
                    return await _cmd.ExecuteReaderAsync();
                }

            }
            catch (Exception ex)
            {
                //Logger.LogFile(LogTipi.Hata, "GateWay.Data ExecuteReader HATA: " + ex);
                throw ex;
            }
        }
        #region Dispose
        public void Close()
        {
            _cmd.Connection.Close();
            _cmd.Dispose();
        }
        #endregion


        #region CommandText
        public string CommandText
        {
            get
            {
                return _cmd.CommandText;
            }
            set
            {
                _cmd.CommandText = value;
            }
        }
        #endregion

        #region CommandType
        public CommandType CommandType
        {
            get
            {
                return _cmd.CommandType;
            }
            set
            {
                _cmd.CommandType = value;
            }
        }
        #endregion
        #region CommandTimeout
        public int CommandTimeout
        {
            get
            {
                return _cmd.CommandTimeout;
            }
            set
            {
                _cmd.CommandTimeout = value;
            }
        }
        #endregion

        public void AddParameter(string paramName, object val)
        {
            _cmd.Parameters.Add(new SqlParameter(paramName, val));

        }
    }
}
