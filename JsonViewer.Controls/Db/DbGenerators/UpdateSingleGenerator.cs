using System.Collections.Generic;
using System.Linq;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db.DbGenerators
{
  public class UpdateSingleGenerator:DbGenerator
  {
    public UpdateSingleGenerator(DbInfo dbInfo) : base(dbInfo)
    {
    }

    public override List<GeneratedItem> Generate(List<JsonItem> jsonItems)
    {
      if (jsonItems == null || jsonItems.Count == 0)
        return new List<GeneratedItem>();

      var jsonItem = jsonItems.First();

      var sqlValueType = DbInfo.SqlType(jsonItem.ValueType);
      var sql = $"""
              Declare  @value {sqlValueType} = {jsonItem.Value};
              update {DbInfo.TableName}
              set {DbInfo.ColumnName} = JSON_MODIFY({DbInfo.ColumnName}, '$.{jsonItem.Path}', @value)
              """;

      return new List<GeneratedItem> { new("Update Value", sql, 3) };
    }
  }
}
