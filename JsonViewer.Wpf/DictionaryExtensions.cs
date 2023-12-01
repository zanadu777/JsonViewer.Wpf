using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JsonViewer.Wpf
{
  public static class DictionaryExtensions
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<TKey, TValue>(this Dictionary<TKey, List<TValue>> index, TKey key,  TValue value)
    {
      if (!index.ContainsKey(key))
        index.Add(key, new List<TValue>{value});
      else
        index[key].Add(value);
    }
  }
}
