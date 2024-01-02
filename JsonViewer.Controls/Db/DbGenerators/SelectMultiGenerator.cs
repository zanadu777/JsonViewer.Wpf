using System;
using System.Collections.Generic;
using System.Linq;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db.DbGenerators
{
  public class SelectMultiGenerator : DbGenerator
  {
    public SelectMultiGenerator(DbInfo dbInfo) : base(dbInfo)
    {
    }

    public override List<GeneratedItem> Generate(List<JsonItem> jsonItems)
    {
      if (jsonItems == null || jsonItems.Count == 0)
        return new List<GeneratedItem>();

      Func<JsonItem, string> selectClause = j=> j.NodeType == "value"
        ? $@"JSON_VALUE({DbInfo.TableAlias}.{DbInfo.ColumnName}, '$.{j.Path}') as {j.Key}"
        : $@"JSON_QUERY({DbInfo.TableAlias}.{DbInfo.ColumnName}, '$.{j.Path}') as {j.Key}";

      var sql = $"""
                 select 
                 {string.Join(", \n", jsonItems.Select(selectClause))}
                 from {DbInfo.TableName} {DbInfo.TableAlias}";
                 """;

      return new List<GeneratedItem> { new("Select Multiple", sql, 2 + jsonItems.Count) };
    }
  }
}
