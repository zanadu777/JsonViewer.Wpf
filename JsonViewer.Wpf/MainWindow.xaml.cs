using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using JsonTools;
using JsonViewer.Controls;
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

      DisplayJson(json);
     
    }


    private void DisplayJson(string json)
    {
      Stopwatch swatch = Stopwatch.StartNew();
      using var stringReader = new StringReader(json);
      using var jsonReader = new JsonTextReader(stringReader);
      var root = JToken.Load(jsonReader);
      var allNodes = root.GetNodes<JToken>(t => t.Children());
      Debug.WriteLine(swatch.Elapsed.TotalSeconds);

      TreeView tree = this.TreeView.TreeView;
      var itemsSource = ConvertTokens(allNodes);
      tree.ItemsSource = itemsSource;
    }
    public ObservableCollection<TreeViewItem> ConvertTokens(IEnumerable<JToken> tokens)
    {
      var nodes = new ObservableCollection<TreeViewItem>();

      Dictionary<string, List<JToken>> tokenDict = new();
      foreach (JToken token in tokens)
        tokenDict.Add(token.Path, token);


      Dictionary<string, TreeViewItem> nodeDict = new Dictionary<string, TreeViewItem>();
      foreach (JToken token in tokens)
      {

        if (string.IsNullOrWhiteSpace(token.Path))
        {
          var root = new TreeViewItem { Header = "{" };
          //root.IsExpanded = true;
          nodes.Add(root);
          nodeDict.Add(token.Path, root);
        }


        else if (token is JProperty)
        {
          if (tokenDict[token.Path].Count == 2)
          {
            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JValue)
            {
              var prop = tokenDict[token.Path][0] as JProperty;
              var value = tokenDict[token.Path][1] as JValue;

              string header = $@"""{prop.Name}"" = {ValueText(value)}";

              var valueNode = new TreeViewItem { Header = header };

              AddNode(nodeDict, token, valueNode, nodes);
            }

            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JObject)
            {
              var prop = tokenDict[token.Path][0] as JProperty;
              var header = $@"""{prop.Name}"" ";

              var valueNode = new TreeViewItem { Header = header };
              AddNode(nodeDict, token, valueNode, nodes);
            }

            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JArray)
            {
              var prop = tokenDict[token.Path][0] as JProperty;

              var header = $@"""{prop.Name}"" ";

              var valueNode = new TreeViewItem { Header = header };

              AddNode(nodeDict, token, valueNode, nodes);
            }
          }
        }
        
        else if (token is JValue && token.Parent is JArray)
        {
          var valueNode = new TreeViewItem { Header = $"{token.Path.ExtractJsonPathArrayPosition()} = {ValueText((JValue)token)}" };

          AddNode(nodeDict, token, valueNode, nodes);
        }


        else if (token is JObject && token.Parent is JArray)
        {
          
          var node = new TreeViewItem {Header = $"{ token.Path.ExtractJsonPathArrayPosition() }" };
        
          AddNode(nodeDict, token, node, nodes);
        }

        else if (token is JValue || token is JObject || token is JArray)
        {

        }
        else
        {
          var node = new TreeViewItem { Header = token.Path };
          nodeDict.Add(token.Path, node);
          nodes.Add(node);
        }

      }

      return nodes;
    }

    private string ValueText(JValue valueNode)
    {
      switch (valueNode.Type)
      {
        case JTokenType.None:
          break;
        case JTokenType.Object:
          break;
        case JTokenType.Array:
          break;
        case JTokenType.Constructor:
          break;
        case JTokenType.Property:
          break;
        case JTokenType.Comment:
          break;
        case JTokenType.Integer:
          return $"{valueNode.Value}";
        case JTokenType.Float:
          return $"{valueNode.Value}";
        case JTokenType.String:
          return $@"""{valueNode.Value}""";
        case JTokenType.Boolean:
          break;
        case JTokenType.Null:
          break;
        case JTokenType.Undefined:
          break;
        case JTokenType.Date:
          break;
        case JTokenType.Raw:
          break;
        case JTokenType.Bytes:
          break;
        case JTokenType.Guid:
          break;
        case JTokenType.Uri:
          break;
        case JTokenType.TimeSpan:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return valueNode.ToString();


    }

    private static void AddNode(Dictionary<string, TreeViewItem> nodeDict, JToken token, TreeViewItem valueNode, ObservableCollection<TreeViewItem> nodes)
    {
      nodeDict.Add(token.Path, valueNode);
      if (nodeDict.ContainsKey(token.Parent.Path))
      {
        nodeDict[token.Parent.Path].Items.Add(valueNode);
        //nodeDict[token.Parent.Path].IsExpanded = true;
      }
      else
        nodes.Add(valueNode);
    }


    private void AddNnode(JToken node)
    {
      if (node is JObject jObject)
      {
        Debug.WriteLine($"JObject   Path={node.Path} {node.Type}");
      }
      else if (node is JArray jArray)
      {
        Debug.WriteLine($"JArray  Path={node.Path} {node.Type}");
      }
      else if (node is JProperty jProperty)
      {
        Debug.WriteLine($"JProperty  Path={node.Path} {jProperty.Name} {node.Type}");
      }
      else if (node is JValue jValue)
      {
        Debug.WriteLine($"JValue Path={node.Path}  {jValue.Type} {jValue.Value}");
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

    private void GetNodes(JToken node, List<JToken> nodeList)
    {
      if (node is JObject jObject)
      {
        Debug.WriteLine($"JObject   Path={node.Path} {node.Type}");
      }
      else if (node is JArray jArray)
      {
        Debug.WriteLine($"JArray  Path={node.Path} {node.Type}");
      }
      else if (node is JProperty jProperty)
      {
        Debug.WriteLine($"JProperty  Path={node.Path} {jProperty.Name} {node.Type}");
      }
      else if (node is JValue jValue)
      {
        Debug.WriteLine($"JValue Path={node.Path}  {jValue.Type} {jValue.Value}");
      }
      else
      {
        Debug.WriteLine($"{node.GetType().Name}  {node.Type} -- Unknown");
      }
      nodeList.Add(node);

      foreach (var child in node.Children())
      {
        AddNnode(child);
      }

    }

    private void mnuLoadBookstore_Click(object sender, RoutedEventArgs e)
    {
      var bookstore = new DataSets.Bookstore();
      var json = bookstore.Json;

      //json = """
      //       [{"id":1, "url":"https://books.toscrape.com/catalogue/a-light-in-the-attic_1000/index.html", "title":"It's hard to imagine a world without A Light in the Attic. This now-classic collection of poetry and drawings from Shel Silverstein celebrates its 20th anniversary with this special edition. Silverstein's humorous and creative verse can amuse the dowdiest of readers. Lemon-faced adults and fidgety kids sit still and read these rhythmic words and laugh and smile and love th It's hard to imagine a world without A Light in the Attic. This now-classic collection of poetry and drawings from Shel Silverstein celebrates its 20th anniversary with this special edition. Silverstein's humorous and creative verse can amuse the dowdiest of readers. Lemon-faced adults and fidgety kids sit still and read these rhythmic words and laugh and smile and love that Silverstein. Need proof of his genius? RockabyeRockabye baby, in the treetopDon't you know a treetopIs no safe place to rock?And who put you up there,And your cradle, too?Baby, I think someone down here'sGot it in for you. Shel, you never sounded so good. ...more", "upc":"a897fe39b1053632", "product_type":"books", "price_excl_tax":52.0, "price_incl_tax":52.0, "tax":0.0, "price":52.0, "availability":22, "num_reviews":0, "stars":3, "category":"poetry", "description":"(\"It's hard to imagine a world without A Light in the Attic. This now-classic collection of poetry and drawings from Shel Silverstein celebrates its 20th anniversary with this special edition. Silverstein's humorous and creative verse can amuse the dowdiest of readers. Lemon-faced adults and fidgety kids sit still and read these rhythmic words and laugh and smile and love th It's hard to imagine a world without A Light in the Attic. This now-classic collection of poetry and drawings from Shel Silverstein celebrates its 20th anniversary with this special edition. Silverstein's humorous and creative verse can amuse the dowdiest of readers. Lemon-faced adults and fidgety kids sit still and read these rhythmic words and laugh and smile and love that Silverstein. Need proof of his genius? RockabyeRockabye baby, in the treetopDon't you know a treetopIs no safe place to rock?And who put you up there,And your cradle, too?Baby, I think someone down here'sGot it in for you. Shel, you never sounded so good. ...more\",)"},
      //       {"id":2, "url":"https://books.toscrape.com/catalogue/scott-pilgrims-precious-little-life-scott-pilgrim-1_987/index.html", "title":"Scott Pilgrim's life is totally sweet. He's 23 years old, he's in a rockband, he's \"between jobs\" and he's dating a cute high school girl. Nothing could possibly go wrong, unless a seriously mind-blowing, dangerously fashionable, rollerblading delivery girl named Ramona Flowers starts cruising through his dreams and sailing by him at parties. Will Scott's awesome life get Scott Pilgrim's life is totally sweet. He's 23 years old, he's in a rockband, he's \"between jobs\" and he's dating a cute high school girl. Nothing could possibly go wrong, unless a seriously mind-blowing, dangerously fashionable, rollerblading delivery girl named Ramona Flowers starts cruising through his dreams and sailing by him at parties. Will Scott's awesome life get turned upside-down? Will he have to face Ramona's seven evil ex-boyfriends in battle? The short answer is yes. The long answer is Scott Pilgrim, Volume 1: Scott Pilgrim's Precious Little Life ...more", "upc":"3b1c02bac2a429e6", "product_type":"books", "price_excl_tax":52.0, "price_incl_tax":52.0, "tax":0.0, "price":52.0, "availability":19, "num_reviews":0, "stars":5, "category":"sequential art", "description":"('Scott Pilgrim\\'s life is totally sweet. He\\'s 23 years old, he\\'s in a rockband, he\\'s \"between jobs\" and he\\'s dating a cute high school girl. Nothing could possibly go wrong, unless a seriously mind-blowing, dangerously fashionable, rollerblading delivery girl named Ramona Flowers starts cruising through his dreams and sailing by him at parties. Will Scott\\'s awesome life get Scott Pilgrim\\'s life is totally sweet. He\\'s 23 years old, he\\'s in a rockband, he\\'s \"between jobs\" and he\\'s dating a cute high school girl. Nothing could possibly go wrong, unless a seriously mind-blowing, dangerously fashionable, rollerblading delivery girl named Ramona Flowers starts cruising through his dreams and sailing by him at parties. Will Scott\\'s awesome life get turned upside-down? Will he have to face Ramona\\'s seven evil ex-boyfriends in battle? The short answer is yes. The long answer is Scott Pilgrim, Volume 1: Scott Pilgrim\\'s Precious Little Life ...more',)"}]
      //       """;
      DisplayJson(json);
    }





  }
}

