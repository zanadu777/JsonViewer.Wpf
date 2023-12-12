using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonViewer.Controls
{
  public class JsonNodes
  {
    public ObservableCollection<JsonTreeViewItem> Items { get; set; }
    public Dictionary<string, JsonTreeViewItem> ItemsDictionary { get; set; }
  }
}
