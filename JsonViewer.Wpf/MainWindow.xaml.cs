using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JsonViewer.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

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
      //JObject obj = JObject.Parse(json);

      //foreach (KeyValuePair<string, JToken> item in obj)
      //{
      //  if (item.Value is JValue)
      //  {
      //    Debug.WriteLine($"{item.Key} JValue" );
      //  }

      //  Debug.WriteLine(item.Key);
      //  Debug.WriteLine(item.Value.Type );
      //}


      using (var stringReader = new StringReader(json))
      {
        using (var jsonReader = new JsonTextReader(stringReader))
        {
          var root = JToken.Load(jsonReader);
          var rootCount = root.Children().Count();

          //foreach (var child in root.Children())
          //{
            AddNnode(root);

            //if (child is JValue)
            //{
            //  //    // Add a node for a value
            //  //    //var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
            //  //    childNode.Tag = token;
            //}
            //else if (child is JObject)
            //{
            //  //    // Add a node for an object
            //  //    var obj = token as JObject;
            //  //    foreach (var property in obj.Properties())
            //  //    {
            //  //      // Add a node for each property
            //  //      //var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
            //  //      childNode.Tag = property.Value;

            //  //      // Recursively add nodes for the property value
            //  //      AddNode(property.Value, childNode);
            //  //    }
            //}
            //else if (child is JArray)
            //{
            //  //    // Add a node for an array
            //  //    var array = token as JArray;
            //  //    for (int i = 0; i < array.Count; i++)
            //  //    {
            //  //      // Add a node for each array element
            //  //      //var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
            //  //      childNode.Tag = array[i];

            //  //      // Recursively add nodes for the array element
            //  //      AddNode(array[i], childNode);
            //}
          //}

        }

      }


      //using (var reader = new StreamReader(json))
      //{
      //  using (var jsonReader = new JsonTextReader(reader))
      //  {
      //    var root = JToken.Load(jsonReader);



      //  }


      //}


    }

    private void AddNnode(JToken node)
    {
      if (node is JObject jObject)
      {
        Debug.WriteLine($"JObject {node.Type}");
      }else if (node is JArray jArray)
      {
        Debug.WriteLine($"JArray {node.Type}");
      }
      else if (node is JProperty jProperty)
      {
        Debug.WriteLine($"JProperty {jProperty.Name} {node.Type}");
      }
      else
      {
        Debug.WriteLine($"{node.GetType().Name}  {node.Type} -- Unknown");
      }

      
      foreach (var child in node.Children())
      {
        AddNnode(child);
      }

    }


    //private void AddNode(JToken token, ITreeNode<string> parent)
    //{
    //  if (token is JValue)
    //  {
        
    //    // Add a node for a value
    //    var childNode = parent.Items[parent.Nodes.Add(new TreeNode(token.ToString()))];
    //    childNode.Tag = token;
    //  }
    //  else if (token is JObject)
    //  {
    //    // Add a node for an object
    //    var obj = token as JObject;
    //    foreach (var property in obj.Properties())
    //    {
    //      // Add a node for each property
    //      var childNode = parent.Items[parent.Nodes.Add(new TreeNode(property.Name))];
    //      childNode.Tag = property.Value;

    //      // Recursively add nodes for the property value
    //      AddNode(property.Value, childNode);
    //    }
    //  }
    //  else if (token is JArray)
    //  {
    //    // Add a node for an array
    //    var array = token as JArray;
    //    for (int i = 0; i < array.Count; i++)
    //    {
    //      // Add a node for each array element
    //      var childNode = parent.Items   [parent.Nodes.Add(new TreeNode(i.ToString()))];
    //      childNode.Tag = array[i];

    //      // Recursively add nodes for the array element
    //      AddNode(array[i], childNode);
    //    }
    //  }
    //}



  }
}

