using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace JsonViewer.Controls
{
  public class JsonTreeViewItem : TreeViewItem
  {
    private static SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);
    private static  SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
    private static SolidColorBrush hilightBrush = new SolidColorBrush(Color.FromRgb(255,176,148));
    private object value;
    public string Path { get; set; }
    public int Depth { get; set; }
    public int ChildRank { get; set; }

    public string NodeType { get; set; }
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

    public bool IsEmptyArray { get; set; }

    public bool IsKeyHilighted { get; set; }
    public bool IsValueHilighted { get; set; }

    public void GenerateHeader()
    {
      switch (NodeType)
      {
        case "value":
          var valueHeader = new TextBlock();
          valueHeader.Inlines.Add(new Run($" {Key} "){Background = IsKeyHilighted? hilightBrush:null });
          valueHeader.Inlines.Add(new Run("="));
          var valueValueRun = new Run($" {HeaderValueText(Value.GetType())} "){Background=IsValueHilighted ?hilightBrush:null};
          valueHeader.Inlines.Add(valueValueRun);
         // Header = $"{Key} = {HeaderValueText(Value.GetType())}";
          Header = valueHeader;
          break;
        case "array":
          var arrayHeader = new TextBlock();
          arrayHeader.Inlines.Add(new Run(Key) { FontWeight = FontWeights.Bold, Foreground = grayBrush });
          if (IsEmptyArray)
            arrayHeader.Inlines.Add(new Run(" = []") { Foreground = grayBrush });
          Header = arrayHeader;
          break;
        case "object":
          var objHeader = new TextBlock();
          objHeader.Inlines.Add(new Run(Key) { FontWeight = FontWeights.Bold });
          Header = objHeader;
          break;
      }
    }

    private string HeaderValueText(Type valueType)
    {
      if (valueType == typeof(string))
        return $"""
                "{ValueAsText}"
                """;
      else if (valueType == typeof(long) || valueType == typeof(double))
        return ValueAsText;
      else if (valueType == typeof(DateTime))
        return $"""
                "{ValueAsText}"
                """;

      return string.Empty;
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
      clone.NodeType = NodeType;
      clone.IsKeyHilighted= IsKeyHilighted;
      clone.IsValueHilighted = IsValueHilighted;
      clone.IsEmptyArray= IsEmptyArray;
      return clone;
    }
  }
}
