using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace JsonViewer.Controls
{
  /// <summary>
  /// Interaction logic for JsonViewerControlWrapper.xaml
  /// </summary>
  public partial class JsonViewerControlWrapper : UserControl
  {
    public JsonViewerControlWrapper()
    {
      InitializeComponent();
      var vm = new JsonViewerVm();
      DataContext = vm;
    }

    #region Json
    public static readonly DependencyProperty jsonProperty = DependencyProperty.Register(
      nameof(Json), typeof(string), typeof(JsonViewerControlWrapper), new PropertyMetadata(default(string), OnJsonChanged));

    private static void OnJsonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var wrapper = (JsonViewerControlWrapper)d;
      var vm = (JsonViewerVm) wrapper.DataContext;
      vm.DisplayJson((string)e.NewValue);
    }

    public string Json
    {
      get { return (string)GetValue(jsonProperty); }
      set { SetValue(jsonProperty, value); }
    }
    #endregion
  }
}
