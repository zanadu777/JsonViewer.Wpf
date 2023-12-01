using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace JsonTools.Tests
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
