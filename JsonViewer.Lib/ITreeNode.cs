using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonViewer.Lib
{
    public interface ITreeNode<T> 
    {
      T Parent { get; set; }

      IList<T> Children { get; set; }
    }
}
