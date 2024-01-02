using System.Collections.Generic;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls.Db.DbGenerators
{
  public abstract class DbGenerator
  {
    public DbGenerator(DbInfo dbInfo)
    {
      DbInfo = dbInfo;
    }

    public DbInfo DbInfo { get; set; }

    public virtual List<GeneratedItem> Generate(JsonItem jsonItem)
    {
      return Generate(new List<JsonItem> {jsonItem});
    } 

    public abstract  List<GeneratedItem> Generate(List<JsonItem> jsonItems);
  }
}
