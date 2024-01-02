using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JsonDb.DataAccessLayer
{
  public static class SqlParameterCollections
  {
    public static SqlParameter AddTableParameter(this SqlParameterCollection parameterCollection,  string paramName, string tableTypeName  , DataTable data )
    {
      SqlParameter parameter = parameterCollection.AddWithValue(paramName, data);
      parameter.TypeName= tableTypeName;
      parameter.SqlDbType = SqlDbType.Structured;
      return parameter;
    }


    public static SqlParameter AddListParameter<T>(this SqlParameterCollection parameterCollection, string paramName, string  tableTypeName , IEnumerable<T> items)
    {
      var dt = items.ToDataTable();
      return parameterCollection.AddTableParameter(paramName, tableTypeName,dt);
    }

    public static SqlParameter AddListVarcharMaxParameter(this SqlParameterCollection parameterCollection, string paramName,  IEnumerable<string> items)
    {
     return parameterCollection.AddListParameter(paramName, "dbo.ListofVarcharMax", items);
    }
  }
}
