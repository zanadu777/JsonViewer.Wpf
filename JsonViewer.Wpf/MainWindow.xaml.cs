using System.IO;
using System.Windows;
using JsonViewer.Controls;

namespace JsonViewer.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      var vm = new JsonViewerVm();
      this.DataContext= vm;
    }

    private void mnuLoadCountries_Click(object sender, RoutedEventArgs e)
    {
      var json = File.ReadAllText(@"D:\Dev\Datasets\Countries\countries.json");

      json = """
             {
               "title": "The Great Gatsby",
               "author": {
                 "name": "F. Scott Fitzgerald",
                 "birth_year": 1896,
                 "death_year": 1940
               },
               "publication_year": 1925,
               "genres": ["novel", "fiction", "classic"],
               "publisher": {
                 "name": "Charles Scribner's Sons",
                 "location": "New York"
               }
             }
             
             """;

      ;
      ((JsonViewerVm)this.DataContext).DisplayJson(json);
    }



    private void mnuLoadBookstore_Click(object sender, RoutedEventArgs e)
    {
      var bookstore = new DataSets.Bookstore();
      var json = bookstore.Json;
      ((JsonViewerVm) this.DataContext).DisplayJson(json);
      
    }

   
  }
}

