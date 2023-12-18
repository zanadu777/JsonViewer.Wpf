using System.Collections.Generic;

namespace JsonViewer.Controls
{
  public class JsonFilterResult
  {
    public Dictionary<string, JsonTreeViewItem> Full { get; set; } = [];
    public Dictionary<string, JsonTreeViewItem> Key { get; set; } = [];
    public Dictionary<string, JsonTreeViewItem> Value { get; set; } = [];
  }
}
