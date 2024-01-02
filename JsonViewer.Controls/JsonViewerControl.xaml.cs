using JsonViewer.Controls.Db;
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
    private ObservableCollection<JsonTreeViewItem> items = new();

    public JsonViewerControl()
    {
      InitializeComponent();
      FilteredItemsSource= new ObservableCollection<JsonTreeViewItem>();
      JsonTabSelectedIndex = 0;
    }

    public TreeView TreeView => TreeViewControl;

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

    #region AddToSelectedItemsCommand
    public static readonly DependencyProperty addToSelectedItemsCommandProperty = DependencyProperty.Register(
      nameof(AddToSelectedItemsCommand), typeof(ICommand), typeof(JsonViewerControl), new PropertyMetadata(default(ICommand)));

    public ICommand AddToSelectedItemsCommand
    {
      get { return (ICommand)GetValue(addToSelectedItemsCommandProperty); }
      set { SetValue(addToSelectedItemsCommandProperty, value); }
    }
    #endregion

    #region RemoveFromSelectedItemsCommand
    public static readonly DependencyProperty removeFromSelectedItemsCommandProperty = DependencyProperty.Register(
      nameof(RemoveFromSelectedItemsCommand), typeof(ICommand), typeof(JsonViewerControl), new PropertyMetadata(default(ICommand)));

    public ICommand RemoveFromSelectedItemsCommand
    {
      get { return (ICommand)GetValue(removeFromSelectedItemsCommandProperty); }
      set { SetValue(removeFromSelectedItemsCommandProperty, value); }
    }
    #endregion

    #region ClearSelectedItemsCommand
    public static readonly DependencyProperty clearSelectedItemsCommandProperty = DependencyProperty.Register(
      nameof(ClearSelectedItemsCommand), typeof(ICommand), typeof(JsonViewerControl), new PropertyMetadata(default(ICommand)));

    public ICommand ClearSelectedItemsCommand
    {
      get { return (ICommand)GetValue(clearSelectedItemsCommandProperty); }
      set { SetValue(clearSelectedItemsCommandProperty, value); }
    }
    #endregion

    #region CheckedVisibilityCommand
    public static readonly DependencyProperty checkedVisibilityCommandProperty = DependencyProperty.Register(
      nameof(CheckedVisibilityCommand), typeof(ICommand), typeof(JsonViewerControl), new PropertyMetadata(default(ICommand)));

    public ICommand CheckedVisibilityCommand
    {
      get { return (ICommand)GetValue(checkedVisibilityCommandProperty); }
      set { SetValue(checkedVisibilityCommandProperty, value); }
    }
    #endregion

    #region UncheckedVisibilityCommand
    public static readonly DependencyProperty uncheckedVisibilityCommandProperty = DependencyProperty.Register(
      nameof(UncheckedVisibilityCommand), typeof(ICommand), typeof(JsonViewerControl), new PropertyMetadata(default(ICommand)));

    public ICommand UncheckedVisibilityCommand
    {
      get { return (ICommand)GetValue(uncheckedVisibilityCommandProperty); }
      set { SetValue(uncheckedVisibilityCommandProperty, value); }
    }
    #endregion

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

    #region Json
    public static readonly DependencyProperty jsonProperty = DependencyProperty.Register(
      nameof(Json), typeof(string), typeof(JsonViewerControl), new PropertyMetadata(default(string)));

    public string Json
    {
      get { return (string)GetValue(jsonProperty); }
      set { SetValue(jsonProperty, value); }
    }
    #endregion

    #region IsDbTabVisible
    public static readonly DependencyProperty isDbTabVisibleProperty = DependencyProperty.Register(
      nameof(IsDbTabVisible), typeof(bool), typeof(JsonViewerControl), new PropertyMetadata(default(bool)));

    public bool IsDbTabVisible
    {
      get { return (bool)GetValue(isDbTabVisibleProperty); }
      set { SetValue(isDbTabVisibleProperty, value); }
    }
    #endregion

    #region IsToolBarTrayVisible
    public static readonly DependencyProperty isToolBarTrayVisibleProperty = DependencyProperty.Register(
      nameof(IsToolBarTrayVisible), typeof(bool), typeof(JsonViewerControl), new PropertyMetadata(default(bool)));

    public bool IsToolBarTrayVisible
    {
      get { return (bool)GetValue(isToolBarTrayVisibleProperty); }
      set { SetValue(isToolBarTrayVisibleProperty, value); }
    }
    #endregion

    #region DbVm
    public static readonly DependencyProperty dbVmProperty = DependencyProperty.Register(
      nameof(DbVm), typeof(DbVm), typeof(JsonViewerControl), new PropertyMetadata(default(DbVm)));

    public DbVm DbVm
    {
      get { return (DbVm)GetValue(dbVmProperty); }
      set { SetValue(dbVmProperty, value); }
    }
    #endregion

    private void TreeViewControl_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      SelectedItem = (JsonTreeViewItem)e.NewValue;
    }

    private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
    {
      e.Handled = true;
    }

    private void CheckedVisibilityRoutedCommand(object sender, RoutedEventArgs e)
    {
      if (DataContext is JsonViewerVm vm && sender is MenuItem menuItem)
        vm.CheckedVisibilityCommand.Execute(menuItem.CommandParameter);
    }

    private void UncheckedVisibilityRoutedCommand(object sender, RoutedEventArgs e)
    {
      if (DataContext is JsonViewerVm vm && sender is MenuItem menuItem)
        vm.UncheckedVisibilityCommand.Execute(menuItem.CommandParameter);
    }
  }
}
