using System;
using System.Linq;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db
{
  public class DbInfo
  {
    public string TableName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;

    public string TableAlias => TableName.FirstOrDefault().ToString().ToUpper();
    public string ColumnAlias => ColumnName.FirstOrDefault().ToString().ToUpper();

    public static string SqlType(JsonValueType valueType)
    {
        switch (valueType)
        {
          case JsonValueType.None:
            break;
          case JsonValueType.String:
            return "varchar(max)";
          case JsonValueType.Integer:
            return "int";
          case JsonValueType.Float:
            return "float";
          case JsonValueType.Number:
            break;
          case JsonValueType.DateTime:
            return "datetime2(7)";
          case JsonValueType.Boolean:
            return "bit";
          case JsonValueType.Bytes:
            return "varbinary(max)";
          case JsonValueType.Object:
            break;
          case JsonValueType.Array:
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
        }
        return string.Empty;
    }
  }
}
