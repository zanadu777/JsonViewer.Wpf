using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace JsonViewer.Controls.Converters
{
  public class BoolToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool isReversed = false;
      if (parameter != null)
        isReversed = (parameter.ToString().Trim().ToLower() == "true");

      if (value is bool boolValue)
      {
        if (isReversed)
          return boolValue ? Visibility.Collapsed : Visibility.Visible;

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
      }
      return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}