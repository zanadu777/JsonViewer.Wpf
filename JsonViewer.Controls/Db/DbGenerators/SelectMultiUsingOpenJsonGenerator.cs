using System.Collections.Generic;
using System.Linq;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db.DbGenerators
{
  internal class SelectMultiUsingOpenJsonGenerator : DbGenerator
  {
    public SelectMultiUsingOpenJsonGenerator(DbInfo dbInfo) : base(dbInfo)
    {
    }

    public override List<GeneratedItem> Generate(List<JsonItem> jsonItems)
    {
      if (jsonItems == null || jsonItems.Count == 0)
        return new List<GeneratedItem>();

      var parts = string.Join( ",\n    ",jsonItems.Select(j => $"{j.Key} {DbInfo.SqlType(j.ValueType)} '$.{j.Path}'"));

      var sql = $"""
                SELECT {DbInfo.ColumnAlias}.*
                FROM {DbInfo.TableName}
                CROSS APPLY OPENJSON({DbInfo.ColumnName}) WITH (
                    {parts}
                ) as {DbInfo.ColumnAlias};
                """;

      return new List<GeneratedItem> { new("Select Multiple Using OpenJson", sql, 4 + jsonItems.Count) };
    }
  }
}
