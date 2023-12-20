using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataSets;
using JsonViewer.Controls;

namespace JsonViewer.Wpf
{
  public class MainWindowVm : INotifyPropertyChanged
  {
    private string json;
    public event PropertyChangedEventHandler PropertyChanged;

    public MainWindowVm()
    {
      LoadFinancialNewsArticleCommand = new DelegateCommand(OnLoadFinancialNewsArticle, _ => true);
    }

    private void OnLoadFinancialNewsArticle(object obj)
    {
      var article = new UsFinancialNewsArticle();
      Json = article.Json;
    }

    public string Json
    {
      get => json;
      set
      {
        if (value == json)
          return;
        json = value;
        OnPropertyChanged();
      }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand LoadFinancialNewsArticleCommand { get; set; }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value)) return false;
      field = value;
      OnPropertyChanged(propertyName);
      return true;
    }
  }
}
