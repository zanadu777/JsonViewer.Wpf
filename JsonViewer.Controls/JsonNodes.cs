using System.Collections.Generic;
using System.Collections.ObjectModel;
using JsonViewer.Controls.Extensions;

namespace JsonViewer.Controls
{
  public class JsonNodes
  {
    public JsonNodes(  Tree<JsonItem>  root)
    {
     Items = root.ToTreeViewItems(x=>x.ToTreeViewItem());
     ItemsDictionary = GetItemsDictionary(Items[0]);
     foreach (var value in ItemsDictionary.Values)
       value.GenerateHeader();
    }
    public ObservableCollection<JsonTreeViewItem> Items { get; set; }
    public Dictionary<string, JsonTreeViewItem> ItemsDictionary { get; set; }

    private Dictionary<string, JsonTreeViewItem> GetItemsDictionary(JsonTreeViewItem root)
    {
      var itemsDictionary = new Dictionary<string, JsonTreeViewItem>();
      var queue = new Queue<JsonTreeViewItem>();
      foreach (var item in root.Items)
      {
        queue.Enqueue((JsonTreeViewItem)item);
      }

      while (queue.Count > 0)
      {
        var current = queue.Dequeue();
        itemsDictionary.Add(current.Path, current);
        foreach (var child in current.Items)
        {
          queue.Enqueue((JsonTreeViewItem)child);
        }
      }
      return itemsDictionary;
    }
  }
}
