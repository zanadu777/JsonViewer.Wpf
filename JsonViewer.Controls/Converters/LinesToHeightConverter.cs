﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace JsonViewer.Controls.Converters
{
  internal class LinesToHeightConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is int lines)
      {
        return lines * 18;
      }
      return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
