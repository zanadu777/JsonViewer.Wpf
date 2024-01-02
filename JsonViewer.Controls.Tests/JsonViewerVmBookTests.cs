using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace JsonViewer.Controls.Tests
{
  [TestClass]
  public class JsonViewerVmBookTests
  {
    private string bookJson= """
             {
               "title": "The Great Gatsby",
               "author": {
                 "name": "F. Scott Fitzgerald",
                 "birth_year": 1896,
                 "death_year": 1940
               },
               "publication_year": 1925,
               "genres": ["novel", "fiction", "classic"],
               "publisher": {
                 "name": "Charles Scribner's Sons",
                 "location": "New York"
               }
             }

             """;

    [TestMethod]
    public void DisplayTests()
    {
      var vm = new JsonViewerVm();
      vm.DefaultOpenWithDepthBelow = 2;
      vm.DefaultOpenWithDepthBelow = 2;
      vm.DisplayJson(bookJson);

      vm.OpenAllNodesCommand.CanExecute(null);
      vm.OpenAllNodesCommand.Execute(null);
      vm.CloseAllNodesCommand.CanExecute(null);
      vm.CloseAllNodesCommand.Execute(null);

      var selectedValue = vm.SelectedValue;
      vm.SelectedValue = selectedValue;

      var selectedPath = vm.SelectedPath;
      vm.SelectedPath = selectedPath;

      var selectedItem = vm.SelectedItem;
      vm.SelectedItem = selectedItem;
    }

    [TestMethod]
    public void FilterTests()
    {
      var vm = new JsonViewerVm();
      vm.DisplayJson(bookJson);
      vm.IsCaseSensitive.Should().BeFalse();
      vm.IsCaseSensitive = false;
      vm.IsCheckingKeys.Should().BeTrue();
      vm.IsCheckingKeys = true;
      vm.IsCheckingValues.Should().BeTrue();
      vm.IsCheckingValues = true;


      vm.TreeViewItems.Count.Should().Be(1);
      vm.FilterText = "ti";
      vm.FilterText = "ti";

      vm.SearchJsonCommand.CanExecute(null);
      vm.SearchJsonCommand.Execute(null);
      vm.FilteredTreeViewItems.Count.Should().Be(4);

      vm.FilterText = "so";
      vm.SearchJsonCommand.Execute(null);
      vm.FilteredTreeViewItems.Count.Should().Be(1);

      vm.IsCaseSensitive = true;
      vm.SearchJsonCommand.Execute(null);
      vm.FilteredTreeViewItems.Count.Should().Be(0);

      vm.FilterText = "So";
      vm.SearchJsonCommand.Execute(null);
      vm.FilteredTreeViewItems.Count.Should().Be(1);
    }
  }
}
