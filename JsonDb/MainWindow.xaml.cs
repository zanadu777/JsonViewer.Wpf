using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows;
using DataSets;
using JsonDb.DataAccessLayer;

namespace JsonDb
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void mnuLoadBookstore(object sender, RoutedEventArgs e)
    {
      var bookstore = new Bookstore();
      var json = bookstore.Json;

      var tools = new NewtonSoft.JsonTools();
      if (tools.IsFistLevelAnArray(json))
      {
       var jsonItems =  tools.FistLevelAsIndividualJson(json);
       SaveJson(jsonItems);
      }
    }

    private void SaveJson(IEnumerable<string> jsons)
    {
      var swatch = Stopwatch.StartNew();
      var cstring = @"Data Source=Delphi;Initial Catalog=JsonTesting;Persist Security Info=True;User ID=sa;Password=****;TrustServerCertificate=True";
      using var connection = new SqlConnection(cstring);
      connection.Open();
      using var cmd = connection.CreateCommand();

      cmd.CommandText = "insert Bookstore select item from @items";
      cmd.Parameters.AddListVarcharMaxParameter("@items", jsons);
      cmd.ExecuteNonQuery();

      var seconds = swatch.Elapsed.TotalSeconds;

    }

    private async void mnuLoadFinancialNews(object sender, RoutedEventArgs e)
    {
      var swatch = Stopwatch.StartNew();
      var dir = new DirectoryInfo(@"D:\Dev\Datasets\US Financial News Articles");
      var files = dir.GetFiles("*.json", SearchOption.AllDirectories);
      var jsonTools = new NewtonSoft.JsonTools();

      foreach (var file in files)
      {
        var json = File.ReadAllText(file.FullName);
        var uuid = jsonTools.GetStringValue(json, "uuid");
        SaveFinancialNews(uuid, json);
      }
      var time = swatch.Elapsed.TotalSeconds;
    }

    private void SaveFinancialNews(string key, string value)
    {
      var cstring = @"Data Source=Delphi;Initial Catalog=JsonTesting;Persist Security Info=True;User ID=sa;Password=Iscandar2199;TrustServerCertificate=True";
      using var connection = new SqlConnection(cstring);
      connection.Open();
      using var cmd = connection.CreateCommand();

      cmd.CommandText = "INSERT INTO FinancialNews (Uuid ,Json) VALUES (@Uuid, @Json)";
      cmd.Parameters.AddWithValue("@Uuid", key);
      cmd.Parameters.AddWithValue("@Json", value);
      cmd.ExecuteNonQuery();
    }
  }
}
