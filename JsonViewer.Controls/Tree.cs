using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace JsonViewer.Controls;

//Generic tree class    
public class Tree<T>(T node)
{
  public T Node { get;  } = node;
  public List<Tree<T>> Children { get; set; } = new();

  
  public ObservableCollection<JsonTreeViewItem> ToTreeViewItems(Func<T ,JsonTreeViewItem> convert)
  {
    var treeViewItems = new ObservableCollection<JsonTreeViewItem>();
    var treeViewItem =convert(Node);
    treeViewItems.Add(treeViewItem);
    foreach (var child in Children)
    {
      var childTreeViewItems = child.ToTreeViewItems(convert);
      foreach (var childTreeViewItem in childTreeViewItems)
      {
        treeViewItem.Items.Add(childTreeViewItem);
      }
    }
    return treeViewItems;
  }


  public ObservableCollection<TreeViewItem> ToTreeViewItemsNonRecursive(Func<T, JsonTreeViewItem> convert)
  {
    var treeViewItems = new ObservableCollection<TreeViewItem>();
    var treeViewItem = convert(Node);
    treeViewItems.Add(treeViewItem);
    var queue = new Queue<Tree<T>>();
    queue.Enqueue(this);
    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      foreach (var child in current.Children)
      {
        var childTreeViewItem = convert(child.Node);
        treeViewItem.Items.Add(childTreeViewItem);
        queue.Enqueue(child);
      }
    }
    return treeViewItems;
  }


}
