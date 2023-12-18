using FluentAssertions;
using JsonViewer.Controls.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsonViewer.Controls.Tests
{
  [TestClass]
  public class StringExtensionsTests
  {
    [TestMethod]
    public void ExtractJsonPathArrayPositionTests()
    {


      "[0]".ExtractJsonPathArrayPosition().Should().Be("[0]");

      "[133]".ExtractJsonPathArrayPosition().Should().Be("[133]");

    }
  }
}
