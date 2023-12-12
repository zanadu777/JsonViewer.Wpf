using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JsonViewer.Controls
{
  public class JsonTreeViewFilter
  {
    public Dictionary<string, JsonTreeViewItem> FullNodes { get; set; }

    public JsonTreeViewFilterDefinition Definition { get; } = new ();
    public JsonTreeViewFilterDefinition PreviousDefinition { get; set; }

    public Dictionary<string, JsonTreeViewItem> Filter()
    {

      Debug.WriteLine(Definition.IsSubsetFilterOf(PreviousDefinition));
      var activeNodeSource = FullNodes;

      var filtered = Definition.Filter(activeNodeSource);

      PreviousDefinition = Definition.Clone();
      return filtered;
    }
  }
}
