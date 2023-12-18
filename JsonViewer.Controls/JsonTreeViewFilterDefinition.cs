using System;
using System.Collections.Generic;

namespace JsonViewer.Controls
{
  public class JsonTreeViewFilterDefinition
  {
    public bool IsCaseSensitive { get; set; }

    public bool IsCheckingKeys { get; set; }

    public bool IsCheckingValues { get; set; }

    public string FilterText { get; set; }

    public bool IsSubsetFilterOf(JsonTreeViewFilterDefinition previousDefinition)
    {
      if (previousDefinition == null)
        return false;

      if (previousDefinition.FilterText == null || !previousDefinition.FilterText.Contains(FilterText))
        return false;

      if (previousDefinition.IsCaseSensitive && !IsCaseSensitive)
        return false;

      if (!previousDefinition.IsCheckingKeys && IsCheckingKeys)
        return false;

      if (!previousDefinition.IsCheckingValues && IsCheckingValues)
        return false;

      return true;
    }

    public JsonTreeViewFilterDefinition Clone()
    {
      var clone = new JsonTreeViewFilterDefinition
      {
        IsCaseSensitive = IsCaseSensitive,
        IsCheckingKeys = IsCheckingKeys,
        IsCheckingValues = IsCheckingValues,
        FilterText = FilterText
      };

      return clone;
    }

    public JsonFilterResult Filter(Dictionary<string, JsonTreeViewItem> source)
    {
      var filtered = new JsonFilterResult();

      Func<string, string, bool> caseInsensitiveFilter = (text, filterText) => text != null && text.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) != -1;
      Func<string, string, bool> caseSensitiveFilter = (text, filterText) => text != null && text.Contains(filterText);
      Func<string, string, bool> filter = IsCaseSensitive ? caseSensitiveFilter : caseInsensitiveFilter;

      foreach (var node in source.Values)
      {
        if (IsCheckingKeys)
          if (filter(node.Key, FilterText))
          {
            filtered.Full[node.Path] = node;
            filtered.Key[node.Path] = node;
          }

        if (IsCheckingValues)
          if (filter(node.ValueAsText, FilterText))
          {
            filtered.Full[node.Path] = node;
            filtered.Value[node.Path] = node;
          }
      }

      return filtered;
    }
  }
}
