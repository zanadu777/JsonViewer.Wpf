using System.Collections.Generic;
using System.Linq;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db.DbGenerators
{
  public class SingleToColumnGenerator : DbGenerator
  {
    public SingleToColumnGenerator(DbInfo dbInfo) : base(dbInfo)
    {
    }

    public override List<GeneratedItem> Generate(List<JsonItem> jsonItems)
    {
      if (jsonItems == null || jsonItems.Count == 0)
        return new List<GeneratedItem>();

      var jsonItem = jsonItems.First();

      var upperFirstKey = jsonItem.Key.FirstOrDefault().ToString().ToUpper() + jsonItem.Key.Substring(1);
      var sqlValueType = DbInfo.SqlType(jsonItem.ValueType);

      var sql = $"""
              ALTER TABLE {DbInfo.TableName}  ADD {upperFirstKey} AS
              CAST(JSON_VALUE({DbInfo.ColumnName}, '$.{jsonItem.Path}') AS
              {sqlValueType}) PERSISTED
              """;

      return new List<GeneratedItem> { new("Extracted To Column", sql, 3) };
    }
  }
}



