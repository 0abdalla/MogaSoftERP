using Microsoft.Data.SqlClient;
using System.Data;

namespace mogaERP.Domain.Interfaces.Common;
public interface ISQLHelper
{
    Task<DataTable> ExecuteDataTableAsync(string commandText, params SqlParameter[] Parameters);
    DataSet ExecuteDataset(string commandText, SqlParameter[] commandParameters);
    List<TElement> SQLQuery<TElement>(string commandText, params SqlParameter[] parameters);
    Task<DataTable> ExecuteTextCommandAsync(string query, params SqlParameter[] parameters);
    Task<int> ExecuteScalarAsync(string procName, params SqlParameter[] sqlParameters);
}
