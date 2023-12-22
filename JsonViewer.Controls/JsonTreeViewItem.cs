using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace JsonViewer.Controls
{
  public class JsonTreeViewItem : TreeViewItem
  {
    private static SolidColorBrush grayBrush = new(Colors.Gray);
    private static  SolidColorBrush blackBrush = new(Colors.Black);
    private static SolidColorBrush hilightBrush = new(Color.FromRgb(255,176,148));
    public static SolidColorBrush selectedFill = new SolidColorBrush(Colors.Gainsboro);

    public JsonTreeViewItem()
    {
      
    }

    public JsonTreeViewItem(JsonItem jsonItem)
    {
      JsonItem = jsonItem;
    }

    private JsonItem JsonItem { get; set; } = new();

    public string Path
    {
      get => JsonItem.Path;
      set => JsonItem.Path = value;
    }

    public int Depth
    {
      get => JsonItem.Depth;
      set => JsonItem.Depth = value;
    }

    public int ChildRank
    {
      get => JsonItem.ChildRank;
      set => JsonItem.ChildRank = value;
    }

    public string NodeType
    {
      get => JsonItem.NodeType;
      set => JsonItem.NodeType = value;
    }

    public string Key
    {
      get => JsonItem.Key;
      set => JsonItem.Key = value;
    }

    public object Value
    {
      get => JsonItem.Value;
      set => JsonItem.Value = value;
    }

    public string ValueAsText
    {
      get=> JsonItem.ValueAsText;
    }

    public bool IsEmptyArray
    {
      get => JsonItem.IsEmptyArray;
      set => JsonItem.IsEmptyArray = value;
    }

    public bool IsKeyHilighted { get; set; }
    public bool IsValueHilighted { get; set; }

    public bool IsSelected { get; set; }

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

          Header = Wrap(valueHeader);
          break;
        case "array":
          var arrayHeader = new TextBlock();
          arrayHeader.Inlines.Add(new Run(Key) { FontWeight = FontWeights.Bold, Foreground = grayBrush });
          if (IsEmptyArray)
            arrayHeader.Inlines.Add(new Run(" = []") { Foreground = grayBrush });
          Header = Wrap(arrayHeader);
          break;
        case "object":
          var objHeader = new TextBlock();
          objHeader.Inlines.Add(new Run(Key) { FontWeight = FontWeights.Bold , Foreground=blackBrush});
          Header = Wrap(objHeader);
          break;
      }
    }

    public Border Wrap(TextBlock textBlock)
    {
      var border = new Border
      {
        Child = textBlock,
        Margin = new Thickness(1, 1, 1, 1),
        BorderThickness = new Thickness(1, 1, 1, 1),
        BorderBrush = IsSelected ? grayBrush : null,
        Background = IsSelected ? selectedFill : null
      };
      return border;
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
