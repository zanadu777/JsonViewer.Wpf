namespace JsonViewer.Controls.Core
{
  public  class GeneratedItem
  {
    public GeneratedItem(string name, string value,int lineCount)
    {
      Name = name;
      Value = value;
      LineCount = lineCount;
    }

    public int LineCount { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
  }
}
