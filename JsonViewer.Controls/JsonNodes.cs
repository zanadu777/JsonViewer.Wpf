using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JsonViewer.Controls
{
  public class JsonNodes
  {
    public ObservableCollection<JsonTreeViewItem> Items { get; set; }
    public Dictionary<string, JsonTreeViewItem> ItemsDictionary { get; set; }
  }
}
