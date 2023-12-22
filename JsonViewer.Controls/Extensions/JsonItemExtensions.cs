using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonViewer.Controls.Extensions
{
  public static class JsonItemExtensions
  {
    public static JsonTreeViewItem ToTreeViewItem(this JsonItem item)
    {
      var treeViewItem = new JsonTreeViewItem(item);
      foreach (var child in item.Children)
      {
        var childTreeViewItem = child.ToTreeViewItem();
        treeViewItem.Items.Add(childTreeViewItem);
      }
      return treeViewItem;
    }
  }
}
