using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      if (!previousDefinition.FilterText.Contains(FilterText))
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

    public Dictionary<string, JsonTreeViewItem> Filter(Dictionary<string, JsonTreeViewItem> source)
    {
      var filtered = new Dictionary<string, JsonTreeViewItem>();

      if (IsCaseSensitive)
      {
        foreach (var node in source.Values)
        {
          if (IsCheckingKeys)
            if (node.Key != null && node.Key.Contains(FilterText))
              filtered[node.Path] = node;

          if (IsCheckingValues)
            if (node.Value != null && node.ValueAsText.Contains(FilterText))
              filtered[node.Path] = node;
        }
      }
      else
      {
        foreach (var node in source.Values)
        {
          if (IsCheckingKeys)
            if (node.Key != null && node.Key.IndexOf(FilterText,StringComparison.OrdinalIgnoreCase)!= -1)
              filtered[node.Path] = node;

          if (IsCheckingValues)
            if (node.Value != null && node.ValueAsText.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) != -1)
              filtered[node.Path] = node;
        }
      }


      return filtered;
    }

    public Dictionary<string, JsonTreeViewItem> FilterInPlace(Dictionary<string, JsonTreeViewItem> source)
    {
      List<string> keysToRemove = new List<string>();

      foreach (var node in source.Values)
      {
        if (IsCheckingKeys && IsCheckingValues)
          if (!node.Key.Contains(FilterText) && !node.ValueAsText.Contains(FilterText))
            keysToRemove.Add(node.Key);

        if (IsCheckingKeys && !IsCheckingValues)
          if (!node.Key.Contains(FilterText))
            keysToRemove.Add(node.Key);

        if (!IsCheckingKeys && IsCheckingValues)
          if (!node.ValueAsText.Contains(FilterText))
            keysToRemove.Add(node.Key);
      }

      foreach (var key in keysToRemove)
        source.Remove(key);

      return source;
    }
  }
}
