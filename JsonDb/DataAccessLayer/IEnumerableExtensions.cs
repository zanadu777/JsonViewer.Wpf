using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDb.DataAccessLayer
{
  public static class IEnumerableExtensions
  {
    public static DataTable ToDataTable<T>(this IEnumerable<T> items)
    {
      var dt = new DataTable();
      dt.Columns.Add(new DataColumn("item", typeof(T)));
      foreach (var item in items)
      {
        var row = dt.NewRow();
        row[0] = item;
        dt.Rows.Add(row);
      }

      return dt;
    }
  }
}
