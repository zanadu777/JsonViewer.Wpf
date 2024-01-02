using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace JsonViewer.Controls.Tests
{
  [TestClass]
  public class JsonViewerControlTests
  {
    [TestMethod]
    public void CreationTests()
    {
      var control = new JsonViewerControl();
      control.Should().NotBeNull();

      var treeviewControl = control.TreeView;

      var filteredItemsSource = control.FilteredItemsSource;

      var selectedItem = control.SelectedItem;
      control.SelectedItem = selectedItem;

      var itemsSource = control.ItemsSource;
      control.ItemsSource = itemsSource;


      var selectedPath = control.SelectedPath;
      control.SelectedPath = selectedPath;

      var searchJsonCommand = control.SearchJsonCommand;
      control.SearchJsonCommand= searchJsonCommand;

      var selectedValue = control.SelectedValue;
      control.SelectedValue= selectedValue;

      var filterText = control.FilterText;
      control.FilterText= filterText;

      var jsonTabSelectedIndex = control.JsonTabSelectedIndex;

    }
  }
}
