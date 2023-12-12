using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JsonViewer.Controls
{
  /// <summary>
  /// Interaction logic for JsonViewerControl.xaml
  /// </summary>
  public partial class JsonViewerControl : UserControl
  {
    private ObservableCollection<JsonTreeViewItem> items = new ObservableCollection<JsonTreeViewItem>();

    public JsonViewerControl()
    {
      InitializeComponent();
      FilteredItemsSource= new ObservableCollection<JsonTreeViewItem>();
      JsonTabSelectedIndex = 0;
    }

    public TreeView TreeView => this.TreeViewControl;

    #region ItemsSource
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
      nameof(ItemsSource), typeof(ObservableCollection<JsonTreeViewItem>), typeof(JsonViewerControl), new PropertyMetadata(default(ObservableCollection<JsonTreeViewItem>)));

    public ObservableCollection<JsonTreeViewItem> ItemsSource
    {
      get { return (ObservableCollection<JsonTreeViewItem>)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }
    #endregion

    #region FilteredItemsSource
    public static readonly DependencyProperty filteredItemsSourceProperty = DependencyProperty.Register(
      nameof(FilteredItemsSource), typeof(ObservableCollection<JsonTreeViewItem>), typeof(JsonViewerControl), new PropertyMetadata(default(ObservableCollection<JsonTreeViewItem>)));

    public ObservableCollection<JsonTreeViewItem> FilteredItemsSource
    {
      get { return (ObservableCollection<JsonTreeViewItem>)GetValue(filteredItemsSourceProperty); }
      set { SetValue(filteredItemsSourceProperty, value); }
    }
    #endregion

    #region SelectedItem
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
      nameof(SelectedItem), typeof(JsonTreeViewItem), typeof(JsonViewerControl), new PropertyMetadata(default(JsonTreeViewItem)));

    public JsonTreeViewItem SelectedItem
    {
      get
      {
        return (JsonTreeViewItem)GetValue(SelectedItemProperty);
      }
      set { SetValue(SelectedItemProperty, value); }
    }
    #endregion

    #region SelectedPath
    public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register(
      nameof(SelectedPath), typeof(string), typeof(JsonViewerControl), new PropertyMetadata(default(string)));

    public string SelectedPath
    {
      get
      {
        return (string)GetValue(SelectedPathProperty);
      }
      set { SetValue(SelectedPathProperty, value); }
    }
    #endregion

    #region SearchJsonCommand 
    public static readonly DependencyProperty SearchJsonCommandProperty = DependencyProperty.Register(
      nameof(SearchJsonCommand), typeof(ICommand), typeof(JsonViewerControl), new PropertyMetadata(default(ICommand)));

    public ICommand SearchJsonCommand
    {
      get { return (ICommand)GetValue(SearchJsonCommandProperty); }
      set { SetValue(SearchJsonCommandProperty, value); }
    }
    #endregion region

    #region SelectedValue 
    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(
      nameof(SelectedValue), typeof(object), typeof(JsonViewerControl), new PropertyMetadata(default(object)));

    public object SelectedValue
    {
      get { return (object)GetValue(SelectedValueProperty); }
      set { SetValue(SelectedValueProperty, value); }
    }
    #endregion region

    #region FilterText 

    public static readonly DependencyProperty filterTextProperty = DependencyProperty.Register(
      nameof(FilterText), typeof(string), typeof(JsonViewerControl), new PropertyMetadata(default(string)));


    public string FilterText
    {
      get { return (string)GetValue(filterTextProperty); }
      set { SetValue(filterTextProperty, value); }
    }
    #endregion

    #region JsonTabSelectedIndex
    public static readonly DependencyProperty jsonTabSelectedIndexProperty = DependencyProperty.Register(
      nameof(JsonTabSelectedIndex), typeof(int), typeof(JsonViewerControl), new PropertyMetadata(default(int)));

    public int JsonTabSelectedIndex
    {
      get { return (int)GetValue(jsonTabSelectedIndexProperty); }
      set { SetValue(jsonTabSelectedIndexProperty, value); }
    }
    #endregion

    private void TreeViewControl_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      SelectedItem = (JsonTreeViewItem)e.NewValue;
    }
  }
}
