using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JsonViewer.Controls.Core;
using JsonViewer.Controls.Db.DbGenerators;

namespace JsonViewer.Controls.Db
{
  public class DbVm : INotifyPropertyChanged
  {
    private JsonItem selectedJsonItem;
    private int selectedIndex;
    private DbInfo dbInfo;

    public DbVm()
    {
      dbInfo = new();
      TableName = IsolatedStorageHelper.ReadFromIsolatedStorage("Db.TableName");
      if (string.IsNullOrWhiteSpace(TableName))
        TableName = "JsonTable";

      ColumnName = IsolatedStorageHelper.ReadFromIsolatedStorage("Db.ColumnName");
      if (string.IsNullOrWhiteSpace(ColumnName))
        ColumnName = "Json";

      SingleGenerators.Add(new SelectSingleGenerator(dbInfo));
      SingleGenerators.Add(new SingleToColumnGenerator(dbInfo));
      SingleGenerators.Add(new UpdateSingleGenerator(dbInfo));

      MultiGenerators.Add(new SelectMultiGenerator(dbInfo));
      MultiGenerators.Add((new SelectMultiUsingOpenJsonGenerator(dbInfo)));
    }

    private List<DbGenerator> SingleGenerators { get; } = new ();
    private List<DbGenerator> MultiGenerators { get;  } = new();
    public JsonItem SelectedJsonItem
    {
      get => selectedJsonItem;
      set
      {
        if (value == selectedJsonItem)
          return;

        selectedJsonItem = value;
        SelectedJsonItemAsCollection.Clear();
        SelectedJsonItemAsCollection.Add(selectedJsonItem);
        OnPropertyChanged();
        UpdateGeneratedItemsFromSelectedItem();
      }
    }

    private void UpdateGeneratedItemsFromSelectedItem()
    {
      GeneratedItemsFromSelectedItem.Clear();
      foreach (var generator in SingleGenerators)
      {
        var generatedItems = generator.Generate(SelectedJsonItem);
        if (! generatedItems.Any())
          continue;

        foreach (var generatedItem in generatedItems)
          GeneratedItemsFromSelectedItem.Add(generatedItem);
      } 
    }

    private void UpdateGeneratedMultiItemsFromSelectedItem()
    { 
      GeneratedMultiItemsFromSelectedItems.Clear();
      foreach (var generator in MultiGenerators)
      {
        var generatedItems = generator.Generate(SelectedJsonItems.ToList());
        if (! generatedItems.Any())
          continue;

        foreach (var generatedItem in generatedItems)
          GeneratedMultiItemsFromSelectedItems.Add(generatedItem);
      }
    }

    public ObservableCollection<JsonItem> SelectedJsonItemAsCollection { get; set; } = new();

    public ObservableCollection<JsonItem> SelectedJsonItems { get; set; } = new();

    public ObservableCollection<GeneratedItem> GeneratedItemsFromSelectedItem { get; } = new();
    public ObservableCollection<GeneratedItem> GeneratedMultiItemsFromSelectedItems { get; } = new();

    public void AddSelectedJsonItem(JsonItem jsonItem)
    {
      SelectedJsonItems.Add(jsonItem);
      UpdateGeneratedMultiItemsFromSelectedItem();
      OnPropertyChanged(nameof(IsSelectedJsonItems));
    }

    public void RemoveSelectedJsonItem(JsonItem jsonItem)
    {
      SelectedJsonItems.Remove(jsonItem);
      UpdateGeneratedMultiItemsFromSelectedItem();
      OnPropertyChanged(nameof(IsSelectedJsonItems));
    }

    public void ClearSelectedJsonItems()
    {
      SelectedJsonItems.Clear();
      UpdateGeneratedMultiItemsFromSelectedItem();
      OnPropertyChanged(nameof(IsSelectedJsonItems));
    }

    public int SelectedIndex
    {
      get => selectedIndex;
      set
      {
        if (value == selectedIndex) return;
        selectedIndex = value;
        OnPropertyChanged();
      }
    }

    public bool IsSelectedJsonItems => SelectedJsonItems.Any();

    public string TableName
    {
      get => dbInfo.TableName;
      set
      { 
        if ( dbInfo.TableName == value) 
          return;

        dbInfo.TableName = value;
        OnPropertyChanged();
        IsolatedStorageHelper.WriteToIsolatedStorage("Db.TableName", TableName);
        UpdateGeneratedItemsFromSelectedItem();
      }
    }

    public string ColumnName
    {
      get => dbInfo.ColumnName;
      set
      {
        if (dbInfo.ColumnName == value)
          return;

        dbInfo.ColumnName= value;
        OnPropertyChanged();
        IsolatedStorageHelper.WriteToIsolatedStorage("Db.ColumnName", dbInfo.ColumnName);
        UpdateGeneratedItemsFromSelectedItem();
      }
    }

    public string MultiSelect
    {
      get
      {
        if (selectedJsonItem == null)
          return string.Empty;

        var alias = TableName.FirstOrDefault().ToString().ToUpper();

        return $"""
                select JSON_VALUE({alias}.{ColumnName}, '$.{SelectedJsonItem.Path}') as {SelectedJsonItem.Key}
                from {TableName} {alias}
                """;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return false;
      field = value;
      OnPropertyChanged(propertyName);
      return true;
    }
  }
}
