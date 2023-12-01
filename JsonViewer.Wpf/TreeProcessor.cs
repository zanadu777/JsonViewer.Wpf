using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonViewer.Wpf
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
