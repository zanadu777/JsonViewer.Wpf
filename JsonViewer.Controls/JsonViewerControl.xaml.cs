﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonViewer.Controls
{
  /// <summary>
  /// Interaction logic for JsonViewerControl.xaml
  /// </summary>
  public partial class JsonViewerControl : UserControl
  {
    public JsonViewerControl()
    {
      InitializeComponent();
    }

    public TreeView TreeView => this.TreeViewControl;
  }
}
