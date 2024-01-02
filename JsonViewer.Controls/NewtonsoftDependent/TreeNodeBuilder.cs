using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsonViewer.Controls.Core;
using JsonViewer.Controls.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonViewer.Controls.NewtonsoftDependent
{
  public class TreeNodeBuilder
  {
    public JsonNodes Build(string json)
    {
      using var stringReader = new StringReader(json);
      using var jsonReader = new JsonTextReader(stringReader);
      var root = JToken.Load(jsonReader);
      var allNodes = root.GetNodes<JToken>(t => t.Children());

      var itemsSource = ConvertTokens(allNodes);
      return itemsSource;
    }

    public JsonNodes ConvertTokens(IEnumerable<JToken> tokens)
    {
      var nodes = new List<Tree<JsonItem>>();

      Dictionary<string, List<JToken>> tokenDict = new();
      foreach (JToken token in tokens)
        tokenDict.Add(token.Path, token);

      Dictionary<string, Tree<JsonItem>> nodeDict = new Dictionary<string, Tree<JsonItem>>();
      foreach (JToken token in tokens)
      {
        if (string.IsNullOrWhiteSpace(token.Path))
        {
          var item = new JsonItem { Path = token.Path, Depth = 0, Key = string.Empty, Value = string.Empty, NodeType = "root" };
          var root =new Tree<JsonItem>(item);

          item.NodeType = "object";
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
              var valueType = GetValueType(value);
              var valueItem  = new JsonItem { Path = token.Path, Key = prop.Name, Value = value.Value, NodeType = "value", ValueType=valueType};
              var valueNode = new Tree<JsonItem>(valueItem);  

              AddNode(nodeDict, token, valueNode, nodes);
            }

            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JObject)
            {
              var prop = tokenDict[token.Path][0] as JProperty;

              var valueNode = new JsonItem {   Path = token.Path, Key = prop.Name, Value = string.Empty , NodeType = "object"};
             
              AddNode(nodeDict, token, valueNode.ToTree(), nodes);
            }

            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JArray)
            {
              var prop = tokenDict[token.Path][0] as JProperty;
              var isEmptyArray = ! tokenDict[token.Path][1].Children().Any();

              var valueNode = new JsonItem
              {
                Path = token.Path,
                Key = prop.Name,
                Value = string.Empty,
                NodeType = "array",
                IsEmptyArray = isEmptyArray
              };
             
              AddNode(nodeDict, token, valueNode.ToTree(), nodes);
            }
          }
        }

        else if (token is JValue && token.Parent is JArray)
        {
          var header = token.Path.ExtractJsonPathArrayPosition();
          var valueNode = new JsonItem
          {
            Path = token.Path,
            Key = header.Trim('[', ']'),
            Value = ((JValue)token).Value,
            NodeType = "value"
          };
          AddNode(nodeDict, token, valueNode.ToTree(), nodes);
        }

        else if (token is JObject && token.Parent is JArray)
        {
          var node = new JsonItem {  Path = token.Path, Key = token.Path.ExtractJsonPathArrayPosition(), Value = string.Empty , NodeType = "arrayItem"};
          AddNode(nodeDict, token, node.ToTree(), nodes);
        }
        else if (token is JValue || token is JObject || token is JArray)
        {
        }
        else
          throw new Exception("Unexpected token");
      }

      return new JsonNodes(nodes[0]) ;
    }

    private static void AddNode(Dictionary<string, Tree<JsonItem>> nodeDict, JToken token, Tree<JsonItem> childNode, List<Tree<JsonItem>> nodes)
    {
      nodeDict.Add(token.Path, childNode);
      if (nodeDict.ContainsKey(token.Parent.Path))
      {
        var parent = nodeDict[token.Parent.Path];
        childNode.Node.Depth = parent.Node.Depth + 1;
        childNode.Node.ChildRank = parent.Children.Count ;
        parent.Children.Add(childNode);
      }
      else
       throw new Exception("parent should already be added");
    }

    private string ValueDisplayText(JValue valueNode)
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

    private JsonValueType GetValueType(JToken token)
    {
      switch (token.Type)
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
          return JsonValueType.Integer;
        case JTokenType.Float:
          return JsonValueType.Float;
        case JTokenType.String:
          return JsonValueType.String;
        case JTokenType.Boolean:
          return JsonValueType.Boolean;
        case JTokenType.Null:
          return JsonValueType.None;
        case JTokenType.Undefined:
          break;
        case JTokenType.Date:
          return JsonValueType.DateTime;
          break;
        case JTokenType.Raw:
          break;
        case JTokenType.Bytes:
          return JsonValueType.Bytes;
        case JTokenType.Guid:
          break;
        case JTokenType.Uri:
          break;
        case JTokenType.TimeSpan:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return JsonValueType.None;
    }
  }
}
