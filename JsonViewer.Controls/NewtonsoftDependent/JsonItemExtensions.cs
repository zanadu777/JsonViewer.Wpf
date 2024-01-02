namespace JsonViewer.Controls.NewtonsoftDependent
{
  public static class JsonItemExtensions
  {
    public static Tree<JsonItem> ToTree(this JsonItem item)
    {
      var tree = new Tree<JsonItem>(item);
      return tree;
    }
  }
}
