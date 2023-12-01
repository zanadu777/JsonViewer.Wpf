using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSets
{
    public class Bookstore
    {
      public string URL => @"https://www.kaggle.com/datasets/bishop36/bookstore/";

      public string Json => File.ReadAllText(@"Bookstore\Books.json");
      public string Cvs => "";

      public string Sql => ";";
  }
}
