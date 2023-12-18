using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using JsonViewer.Controls.NewtonsoftDependent;

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
    private string json;
    private string selectedCaseSensitivity;
    private string selectedElementToSearch;
    private bool isAutoFiltering;

    public JsonTreeViewFilterDefinition Definition { get; } = new();
    public JsonTreeViewFilterDefinition PreviousDefinition { get; set; }
    public JsonFilterResult FilterResult { get; set; }


    public JsonViewerVm()
    {
      SearchJsonCommand = new DelegateCommand(OnSearchJson, _ => true);
      OpenAllNodesCommand = new DelegateCommand(OnOpenAllNodes, _ => true);
      CloseAllNodesCommand = new DelegateCommand(OnCloseAllNodes, _ => true);
      IsCheckingKeys = true;
      IsCheckingValues = true;
      IsCaseSensitive = false;
      IsAutoFiltering = true;

      SelectedCaseSensitivity = "Case Insensitive";
      SelectedElementToSearch = "Both";
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
      get => Definition.IsCaseSensitive;
      set
      {
        if (value == Definition.IsCaseSensitive)
          return;

        Definition.IsCaseSensitive = value;
        OnPropertyChanged();
      }
    }

    public bool IsCheckingValues
    {
      get => Definition.IsCheckingValues;
      set
      {
        if (value == Definition.IsCheckingValues)
          return;

        Definition.IsCheckingValues = value;
        selectedElementToSearch = GetSelectedElementToSearch();
        OnPropertyChanged();
        OnPropertyChanged(nameof(SelectedElementToSearch));
        OnSearchJson(null);
      }
    }

    public bool IsCheckingKeys
    {
      get => Definition.IsCheckingKeys;
      set
      {
        if (value == Definition.IsCheckingKeys)
          return;

        Definition.IsCheckingKeys = value;
        selectedElementToSearch = GetSelectedElementToSearch();
        OnPropertyChanged();
        OnPropertyChanged(nameof(SelectedElementToSearch));
        OnSearchJson(null);
      }
    }

    private string GetSelectedElementToSearch()
    {
      if (IsCheckingKeys && IsCheckingValues)
        return "Both";

      if (IsCheckingKeys  )
        return "Keys";

      if (  IsCheckingValues)
        return "Values";

      throw new Exception("need to search for something");
    }

    private void OnSearchJson(object obj)
    {
      if (string.IsNullOrWhiteSpace(FilterText))
        return;

      Definition.FilterText = FilterText;
      FilterResult = Definition.Filter(nodeDict);

      FilteredTreeViewItems.Clear();
      foreach (var node in FilterResult.Full.Values)
      {
        var clone = node.ShallowClone();
        if (FilterResult.Key.ContainsKey(clone.Path))
          clone.IsKeyHilighted = true;

        if (FilterResult.Value.ContainsKey(clone.Path))
          clone.IsValueHilighted = true;

        clone.GenerateHeader();
        FilteredTreeViewItems.Add(clone);
      }

      JsonTabSelectedIndex = 1;
    }

    public bool IsAutoFiltering
    {
      get => isAutoFiltering;
      set
      {
        if (value == isAutoFiltering) return;
        isAutoFiltering = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<string> CaseSensitivities { get; } = new ObservableCollection<string>(new[] { "Case Insensitive", "Case Sensitive" });

    public string SelectedCaseSensitivity
    {
      get => selectedCaseSensitivity;
      set
      {
        if (value == selectedCaseSensitivity) 
          return;

        selectedCaseSensitivity = value;
        if (selectedCaseSensitivity == "Case Sensitive") 
         Definition.IsCaseSensitive = true;
        if (selectedCaseSensitivity == "Case Insensitive")
          Definition.IsCaseSensitive = false;

        OnPropertyChanged(nameof(IsCaseSensitive));
        OnPropertyChanged();
        OnSearchJson(null);
      }
    }

    public ObservableCollection<string> ElementsToSearch { get; }= new ObservableCollection<string>(new[] { "Both", "Keys" , "Values"});

    public string SelectedElementToSearch
    {
      get => selectedElementToSearch;
      set
      {
        if (value == selectedElementToSearch) 
          return;

        selectedElementToSearch = value;

        if (selectedElementToSearch == "Both")
        {
          Definition.IsCheckingKeys = true;
          Definition.IsCheckingValues = true;
        }

        if (selectedElementToSearch == "Keys")
        {
          Definition.IsCheckingKeys = true;
          Definition.IsCheckingValues = false;
        }
          

        if (selectedElementToSearch == "Values")
        {
          Definition.IsCheckingKeys = false;
          Definition.IsCheckingValues = true;
        }

        OnPropertyChanged();
        OnPropertyChanged(nameof(IsCheckingKeys));
        OnPropertyChanged(nameof(IsCheckingValues));
        OnSearchJson(null);
      }
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

        SelectedPath = (value == null) ? null : selectedItem.Path;
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
        if (IsAutoFiltering)
        {
          OnSearchJson(null);
        }
        OnPropertyChanged();
      }
    }

    public void DisplayJson(string jsonInput)
    {
      DisplayJson(jsonInput, false);
    }

    private void DisplayJson(string jsonInput, bool fromPropertyChange)
    {
      var nodeBuilder = new TreeNodeBuilder();
      TreeViewItems.Clear();
      var nodesResult = nodeBuilder.Build(jsonInput);

      nodeDict = nodesResult.ItemsDictionary;

      foreach (var node in nodesResult.Items)
        TreeViewItems.Add(node);

      OpenWithDepthBelow(DefaultOpenWithDepthBelow);

      if (!fromPropertyChange)
        Json = jsonInput;
    }

    public string Json
    {
      get => json;
      set
      {
        if (value == json) return;
        json = value;
        DisplayJson(json, true);
        OnPropertyChanged();
      }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
