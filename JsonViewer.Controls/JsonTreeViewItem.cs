using System.Windows.Controls;

namespace JsonViewer.Controls
{
  public class JsonTreeViewItem: TreeViewItem
  {
    private object value;
    public string Path { get; set; }
    public int Depth { get; set; }
    public int ChildRank { get; set; }

    public string Key { get; set; }

    public object Value
    {
      get => value;
      set
      {
        this.value = value;
        if (value is string strValue)
          ValueAsText = strValue;
        else 
          ValueAsText = value.ToString();
      }
    }

    public string ValueAsText { get; private set; }

    public JsonTreeViewItem ShallowClone()
    {
      var clone = new JsonTreeViewItem();
      clone.Value = Value;
      clone.Path = Path;
      clone.Depth = Depth;
      clone.ChildRank = ChildRank;
      clone.Key = Key;
      clone.Header = Header;
      return clone;
    }
  }
}
