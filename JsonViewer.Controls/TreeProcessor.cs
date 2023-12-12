using System;
using System.Collections.Generic;

namespace JsonViewer.Controls
{
  public static  class TreeProcessor
  {
    public static List<T> GetNodes<T>(this T  rootNode,  Func<T, IEnumerable<T>> getChildren)
    {
      var nodes = new List<T>();
      rootNode.GetNodes(getChildren, nodes);

      return nodes;
    }

    private static void GetNodes<T>(this T node, Func<T, IEnumerable<T>> getChildren, List<T> nodes )
    {
     nodes.Add(node);
     foreach (var child in getChildren(node))
       child.GetNodes(getChildren, nodes);
    }
  }
}
