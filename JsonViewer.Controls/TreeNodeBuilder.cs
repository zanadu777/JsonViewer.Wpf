﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using JsonTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonViewer.Controls
{
  public class TreeNodeBuilder
  {
    public JsonNodes Build(string json)
    {
      Stopwatch swatch = Stopwatch.StartNew();
      using var stringReader = new StringReader(json);
      using var jsonReader = new JsonTextReader(stringReader);
      var root = JToken.Load(jsonReader);
      var allNodes = root.GetNodes<JToken>(t => t.Children());
      Debug.WriteLine(swatch.Elapsed.TotalSeconds);


      var itemsSource = ConvertTokens(allNodes);
      return itemsSource;
    }

    public JsonNodes ConvertTokens(IEnumerable<JToken> tokens)
    {
      var nodes = new ObservableCollection<JsonTreeViewItem>();

      Dictionary<string, List<JToken>> tokenDict = new();
      foreach (JToken token in tokens)
        tokenDict.Add(token.Path, token);


      Dictionary<string, JsonTreeViewItem> nodeDict = new Dictionary<string, JsonTreeViewItem>();
      foreach (JToken token in tokens)
      {

        if (string.IsNullOrWhiteSpace(token.Path))
        {
          var root = new JsonTreeViewItem { Header = "{", Path = token.Path, Depth = 0 , Key=string.Empty, Value = string.Empty};
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

              var valueNode = new JsonTreeViewItem { Header = header, Path = token.Path, Key= prop.Name, Value = value.Value };

              AddNode(nodeDict, token, valueNode, nodes);
            }

            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JObject)
            {
              var prop = tokenDict[token.Path][0] as JProperty;
              var header = $@"""{prop.Name}"" ";

              var valueNode = new JsonTreeViewItem { Header = header, Path = token.Path , Key=prop.Name , Value = string.Empty};
              AddNode(nodeDict, token, valueNode, nodes);
            }

            if (tokenDict[token.Path][0] is JProperty && tokenDict[token.Path][1] is JArray)
            {
              var prop = tokenDict[token.Path][0] as JProperty;

              var header = $@"""{prop.Name}"" ";

              var valueNode = new JsonTreeViewItem { Header = header, Path = token.Path, Key = prop.Name, Value = string.Empty };

              AddNode(nodeDict, token, valueNode, nodes);
            }
          }
        }

        else if (token is JValue && token.Parent is JArray)
        {
          var header = token.Path.ExtractJsonPathArrayPosition();
          var valueNode = new JsonTreeViewItem() { Header = $"{header} = {ValueText((JValue)token)}", Path = token.Path , Key =header.Trim('[', ']'),   Value=( (JValue)token).Value};

          AddNode(nodeDict, token, valueNode, nodes);
        }


        else if (token is JObject && token.Parent is JArray)
        {

          var node = new JsonTreeViewItem { Header = $"{token.Path.ExtractJsonPathArrayPosition()}", Path = token.Path , Key=string.Empty, Value = string.Empty};

          AddNode(nodeDict, token, node, nodes);
        }

        else if (token is JValue || token is JObject || token is JArray)
        {

        }
        else
        {
          var node = new JsonTreeViewItem { Header = token.Path, Path = token.Path };
          nodeDict.Add(token.Path, node);
          nodes.Add(node);
        }

      }

      return new JsonNodes() { Items = nodes, ItemsDictionary = nodeDict };
    }


    private static void AddNode(Dictionary<string, JsonTreeViewItem> nodeDict, JToken token, JsonTreeViewItem childNode, ObservableCollection<JsonTreeViewItem> nodes)
    {
      nodeDict.Add(token.Path, childNode);
      if (nodeDict.ContainsKey(token.Parent.Path))
      {
        var parent = nodeDict[token.Parent.Path];
        childNode.Depth = parent.Depth ++;
        childNode.ChildRank = parent.Items.Count + 1;
        parent.Items.Add(childNode);
      }
      else
        nodes.Add(childNode);
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
  }
}