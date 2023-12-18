namespace JsonViewer.Controls.Extensions
{
  public static  class StringExtensions
  {
    public static string ExtractJsonPathArrayPosition(this string jsonPath)
    {
      for (var i = jsonPath.Length - 1; i >= 0; i--)
        if (jsonPath[i] == '[' )
          return i == 0 ? jsonPath : jsonPath.Substring(i);

      return string.Empty;
    }
  }
}
