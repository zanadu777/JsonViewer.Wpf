using System.Windows;

namespace JsonViewer.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow2.xaml
  /// </summary>
  public partial class MainWindow2 : Window
  {
    public MainWindow2()
    {
      InitializeComponent();
      var vm = new MainWindowVm();
      DataContext = vm;
      Wrapper.DataContext= vm;
    }
  }
}
