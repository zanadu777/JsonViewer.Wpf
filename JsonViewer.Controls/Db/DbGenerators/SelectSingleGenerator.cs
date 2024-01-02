using System.Collections.Generic;
using System.Linq;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db.DbGenerators
{
  public class SelectSingleGenerator:DbGenerator
  {
    public SelectSingleGenerator(DbInfo dbInfo) : base(dbInfo)
    {
    }

    public override List<GeneratedItem> Generate(List<JsonItem> jsonItems)
    {
      if (jsonItems == null || jsonItems.Count==0)
        return new List<GeneratedItem>();

      var jsonItem = jsonItems.First();

      var alias = DbInfo.TableName.FirstOrDefault().ToString().ToUpper();

      if (jsonItem.NodeType == "value")
      {
        var sql = $"""
                   select JSON_VALUE({alias}.{DbInfo.ColumnName}, '$.{jsonItem.Path}') as {jsonItem.Key}
                   from {DbInfo.TableName} {alias}
                   """;

        return new List<GeneratedItem> { new("Select Value", sql,  2) };
      }

      var sqlo = $"""
                  select JSON_QUERY({alias}.{DbInfo.ColumnName}, '$.{jsonItem.Path}') as {jsonItem.Key}
                  from {DbInfo.TableName} {alias}
                  """;

      return new List<GeneratedItem> { new("Select Json", sqlo, 2) };
    }
  }
}
