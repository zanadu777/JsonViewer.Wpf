using System.Collections.Generic;
using JsonViewer.Controls.Core;

namespace JsonViewer.Controls
{
  public class JsonItem
  {
    private object value;
    public string Path { get; set; }
    public int Depth { get; set; }
    public int ChildRank { get; set; }

    public string NodeType { get; set; }

    public JsonValueType ValueType { get; set; }
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

    public bool IsEmptyArray { get; set; }

    public List<JsonItem> Children { get; set; } = new();

    public override string ToString()
    {
      return $"{Path}";
    }
  }
}
