﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using JsonViewer.Controls.Db;
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
    private bool isToolBarTrayVisible;
    private bool isDbTabVisible;

    public ObservableCollection<JsonTreeViewItem> TreeViewItems { get; } = new();
    public ObservableCollection<JsonTreeViewItem> FilteredTreeViewItems { get; } = new();
    public JsonTreeViewFilterDefinition Definition { get; } = new();
    public JsonTreeViewFilterDefinition PreviousDefinition { get; set; }
    public JsonFilterResult FilterResult { get; set; }

    public ICommand SearchJsonCommand { get; }
    public ICommand OpenAllNodesCommand { get; }
    public ICommand CloseAllNodesCommand { get; }
    public ICommand AddToSelectedItemsCommand { get; }
    public ICommand RemoveFromSelectedItemsCommand { get; }
    public ICommand ClearSelectedItemsCommand { get; }

    public ICommand CheckedVisibilityCommand { get; }

    public ICommand UncheckedVisibilityCommand { get; }

    public DbVm DbVm { get; } = new();

    public JsonViewerVm()
    {
      SearchJsonCommand = new DelegateCommand(OnSearchJson, _ => true);
      OpenAllNodesCommand = new DelegateCommand(OnOpenAllNodes, _ => true);
      CloseAllNodesCommand = new DelegateCommand(OnCloseAllNodes, _ => true);
      AddToSelectedItemsCommand = new DelegateCommand(OnAddToSelectedItems, _ => true);
      RemoveFromSelectedItemsCommand = new DelegateCommand(OnRemoveFromSelectedItemsCommand, _ => true);
      ClearSelectedItemsCommand = new DelegateCommand(OnClearSelectedItemsCommand, _ => true);
      CheckedVisibilityCommand = new DelegateCommand(OnCheckedVisibilityCommand, _ => true);
      UncheckedVisibilityCommand = new DelegateCommand(OnUncheckedVisibilityCommand, _ => true);
      IsCheckingKeys = true;
      IsCheckingValues = true;
      IsCaseSensitive = false;
      IsAutoFiltering = true;

      SelectedCaseSensitivity = "Case Insensitive";
      SelectedElementToSearch = "Both";
      IsDbTabVisible = true;
    }

    private void OnUncheckedVisibilityCommand(object obj)
    {
      if (obj is string visibleObject)
      {
        switch (visibleObject)
        {
          case "Db":
            IsDbTabVisible = false;
            break;
          case "ToolBarTray":
            IsToolBarTrayVisible = false;
            break;
        }
      }
    }

    private void OnCheckedVisibilityCommand(object obj)
    {
      if (obj is string visibleObject)
      {
        switch (visibleObject)
        {
          case "Db":
            IsDbTabVisible = true;
            break;
          case "ToolBarTray":
            IsToolBarTrayVisible = true;
            break;
        }
      }
    }

    private void OnClearSelectedItemsCommand(object obj)
    {
      foreach (var node in nodeDict.Values)
        if (node.IsSelected)
        {
          node.IsSelected = false;
          node.GenerateHeader();
        }

      foreach (var node in FilteredTreeViewItems)
      {
        if (node.IsSelected)
        {
          node.IsSelected = false;
          node.GenerateHeader();
        }
      }

      SelectedPaths.Clear();
      if (IsDbTabVisible)
        DbVm.ClearSelectedJsonItems();
    }

    private void OnRemoveFromSelectedItemsCommand(object obj)
    {
      if (SelectedPath != null && nodeDict.ContainsKey(SelectedPath))
      {
        nodeDict[SelectedPath].IsSelected = false;
        nodeDict[SelectedPath].GenerateHeader();

        if (IsDbTabVisible)
          DbVm.RemoveSelectedJsonItem(nodeDict[SelectedPath].JsonItem);
      }

      foreach (var node in FilteredTreeViewItems)
      {
        if (node.Path == SelectedPath)
        {
          node.IsSelected = false;
          node.GenerateHeader();
          break;
        }
      }

      SelectedPaths.Remove(SelectedPath);
    }

    private void OnAddToSelectedItems(object obj)
    {
      if (SelectedPath != null && nodeDict.ContainsKey(SelectedPath))
      {
        nodeDict[SelectedPath].IsSelected = true;
        nodeDict[SelectedPath].GenerateHeader();

        if (IsDbTabVisible)
          DbVm.AddSelectedJsonItem(nodeDict[SelectedPath].JsonItem);
      }

      foreach (var node in FilteredTreeViewItems)
      {
        if (node.Path == SelectedPath)
        {
          node.IsSelected = true;
          node.GenerateHeader();
          break;
        }
      }


      SelectedPaths.Add(SelectedPath);
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
        if (node.Items.Count > 0 && node.Depth <= depth)
        {
          node.IsExpanded = true;
        }
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

    public bool IsToolBarTrayVisible
    {
      get => isToolBarTrayVisible;
      set
      {
        if (value == isToolBarTrayVisible) return;
        isToolBarTrayVisible = value;
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

      if (IsCheckingKeys)
        return "Keys";

      if (IsCheckingValues)
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

    public ObservableCollection<string> CaseSensitivities { get; } =
      new ObservableCollection<string>(new[] { "Case Insensitive", "Case Sensitive" });

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

    public ObservableCollection<string> ElementsToSearch { get; } =
      new ObservableCollection<string>(new[] { "Both", "Keys", "Values" });

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

    public JsonTreeViewItem SelectedItem
    {
      get => selectedItem;
      set
      {
        if (Equals(value, selectedItem))
          return;

        selectedItem = value;
        DbVm.SelectedJsonItem = selectedItem?.JsonItem;
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
      if (jsonInput == Json)
        return;

      var nodeBuilder = new TreeNodeBuilder();
      TreeViewItems.Clear();
      var nodesResult = nodeBuilder.Build(jsonInput);

      nodeDict = nodesResult.ItemsDictionary;

      foreach (var node in nodesResult.Items)
        TreeViewItems.Add(node);

      var root = TreeViewItems[0];
      root.IsExpanded = true;

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

    public bool IsDbTabVisible
    {
      get => isDbTabVisible;
      set
      {
        if (value == isDbTabVisible) return;
        isDbTabVisible = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<string> SelectedPaths { get; } = new();

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

  }
}
