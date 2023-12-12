using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace JsonViewer.Controls
{
  public static class DictionaryExtensions
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<TKey, TValue>(this Dictionary<TKey, List<TValue>> index, TKey key, TValue value)
    {
      if (!index.ContainsKey(key))
        index.Add(key, new List<TValue> { value });
      else
        index[key].Add(value);
    }

    public static string Summary(this Dictionary<string, JsonTreeViewItem> dictionary)
    {
      var sb = new StringBuilder();

      var pairs = dictionary.ToPairs();

      var sortedPairs = from pair in pairs
        select pair ;

      foreach (var pair in sortedPairs)
      {
        sb.AppendLine(pair.Key);
      }

      return sb.ToString();
    }


    public static List<KeyValuePair<TKey, TValue>> ToPairs<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
      var pairs = new List<KeyValuePair<TKey, TValue>>();
      foreach (var pair in dictionary)
      {
        pairs.Add(pair);
      }
      return pairs;
    }

  }
}
