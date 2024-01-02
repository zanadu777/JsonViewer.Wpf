namespace JsonViewer.Controls.Extensions
{
  public static class JsonItemExtensions
  {
    public static JsonTreeViewItem ToTreeViewItem(this JsonItem item)
    {
      var treeViewItem = new JsonTreeViewItem(item);
      return treeViewItem;
    }
  }
}
