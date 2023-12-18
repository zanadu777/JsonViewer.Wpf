using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonViewer.Controls.NewtonsoftDependent
{
  public class JsonTools
  {
    public bool IsFistLevelAnArray(string json)
    {
      var rootToken = ToToken(json);
      return rootToken.Type == JTokenType.Array;
    }

    public JToken ToToken(string json)
    {
      using var stringReader = new StringReader(json);
      using var jsonReader = new JsonTextReader(stringReader);
      var root = JToken.Load(jsonReader);
      return root;
    }

    public List<string> FistLevelAsIndividualJson(string json)
    {
      var rootToken = ToToken(json);
      return ChildrenAsIndividualJsons(rootToken);
    }

    public List<string> ChildrenAsIndividualJsons(JToken token)
    {
      var jsons = new List<string>();

      if (token.Type != JTokenType.Array)
        return jsons;

      foreach (var arrayItem in token.Children())
        jsons.Add(arrayItem.ToString());

      return jsons;
    }

    public string GetStringValue(string json, string path)
    {
      JObject obj =  JObject.Parse(json);

      var value = (string)obj.SelectToken(path);
      return value;
    }
  }
}
