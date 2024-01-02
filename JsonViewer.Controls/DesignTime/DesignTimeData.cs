using JsonViewer.Controls.Db;

namespace JsonViewer.Controls.DesignTime
{
  public static  class DesignTimeData
  {
    public static JsonViewerVm JsonViewerVm => new JsonViewerVm();
    public static DbVm DbVm => new DbVm();
  }
}
