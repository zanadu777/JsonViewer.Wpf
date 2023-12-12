using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JsonViewer.Controls
{
  public class JsonViewerVm : INotifyPropertyChanged
  {
    private JsonTreeViewItem selectedItem;
    private string selectedPath;
    private object selectedValue;
    public event PropertyChangedEventHandler PropertyChanged;
    private Dictionary<string, JsonTreeViewItem> nodeDict = new();
    private string filterText;
    private int jsonTabSelectedIndex;
    private int defaultOpenWithDepthBelow = 1;
    private JsonTreeViewFilter filter = new();



    public JsonViewerVm()
    {
      SearchJsonCommand = new DelegateCommand(OnSearchJson, x => true);
      OpenAllNodesCommand = new DelegateCommand(OnOpenAllNodes, x => true);
      CloseAllNodesCommand = new DelegateCommand(OnCloseAllNodes, x => true);
      IsCheckingKeys = true;
      IsCheckingValues = true;
      IsCaseSensitive = false;
    }

    private void OnCloseAllNodes(object obj)
    {
      foreach (var node in nodeDict.Values)
      {
        if (node.Items.Count > 0)
          node.IsExpanded = false;
      }
    }

    private void OnOpenAllNodes(object obj)
    {
      foreach (var node in nodeDict.Values)
      {
        if (node.Items.Count > 0)
          node.IsExpanded = true;
      }
    }

    private void OpenWithDepthBelow(int depth)
    {
      foreach (var node in nodeDict.Values)
      {
        if (node.Items.Count > 0 && node.Depth > depth)
          node.IsExpanded = true;
      }
    }

    public int DefaultOpenWithDepthBelow
    {
      get => defaultOpenWithDepthBelow;
      set
      {
        if (value == defaultOpenWithDepthBelow)
          return;

        defaultOpenWithDepthBelow = value;
        OnPropertyChanged();
      }
    }


    public bool IsCaseSensitive
    {
      get => filter.Definition.IsCaseSensitive;
      set
      {
        if (value == filter.Definition.IsCaseSensitive)
          return;

        filter.Definition.IsCaseSensitive = value;
        OnPropertyChanged();
      }
    }

    public bool IsCheckingValues
    {
      get => filter.Definition.IsCheckingValues;
      set
      {
        if (value == filter.Definition.IsCheckingValues)
          return;
        filter.Definition.IsCheckingValues = value;
        OnPropertyChanged();
      }
    }

    public bool IsCheckingKeys
    {
      get => filter.Definition.IsCheckingKeys;
      set
      {
        if (value == filter.Definition.IsCheckingKeys)
          return;

        filter.Definition.IsCheckingKeys = value;
        OnPropertyChanged();
      }
    }

    private void OnSearchJson(object obj)
    {
      filter.Definition.FilterText = FilterText;
      filter.FullNodes = nodeDict;


      FilteredTreeViewItems.Clear();
      foreach (var node in filter.Filter().Values)
        FilteredTreeViewItems.Add(node.ShallowClone());


      JsonTabSelectedIndex = 1;
    }

    public ObservableCollection<JsonTreeViewItem> TreeViewItems { get; } = new();
    public ObservableCollection<JsonTreeViewItem> FilteredTreeViewItems { get; } = new();

    public JsonTreeViewItem SelectedItem
    {
      get => selectedItem;
      set
      {
        if (Equals(value, selectedItem)) 
          return;

        selectedItem = value;
        SelectedPath = selectedItem.Path;
        OnPropertyChanged();
      }
    }

    public string SelectedPath
    {
      get => selectedPath;
      set
      {
        if (value == selectedPath) 
          return;

        selectedPath = value;
        OnPropertyChanged();
      }
    }

    public object SelectedValue
    {
      get => selectedValue;
      set
      {
        if (Equals(value, selectedValue))
          return;

        selectedValue = value;
        OnPropertyChanged();
      }
    }

    public int JsonTabSelectedIndex
    {
      get => jsonTabSelectedIndex;
      set
      {
        if (value == jsonTabSelectedIndex) return;
        jsonTabSelectedIndex = value;
        OnPropertyChanged();
      }
    }

    public ICommand SearchJsonCommand { get; }

    public ICommand OpenAllNodesCommand { get; }

    public ICommand CloseAllNodesCommand { get; }


    public string FilterText

    {
      get => filterText;
      set
      {
        if (value == filterText)
          return;

        filterText = value;
        Debug.WriteLine(FilterText);
        OnPropertyChanged();
      }
    }


    public void DisplayJson(string json)
    {
      var nodeBuilder = new TreeNodeBuilder();
      TreeViewItems.Clear();
      var nodesResult = nodeBuilder.Build(json);

      nodeDict = nodesResult.ItemsDictionary;

      foreach (var node in nodesResult.Items)
        TreeViewItems.Add(node);

      var summary = nodeDict.Summary();

      OpenWithDepthBelow(DefaultOpenWithDepthBelow);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
