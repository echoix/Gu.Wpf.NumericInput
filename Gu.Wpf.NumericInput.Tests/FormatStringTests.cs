namespace Gu.Wpf.NumericInput.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FormatStringTests
    {
        [TestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("First", false)]
        [DataRow("First: {0}", true)]
        [DataRow("First: {0:N}", true)]
        [DataRow("First: {0} ", true)]
        [DataRow("First: {0}, Second: {0:N}", true)]
        [DataRow("First: {0:F2 }, Second: {0:N}", true)]
        [DataRow("First: {{{0}}}", true)]
        [DataRow("First: {{{0:F3}}}", true)]
        [DataRow("First: {0", false)]
        [DataRow("First: 0}", false)]
        [DataRow("First: {{0}}", false)]
        [DataRow("First: {{0}", false)]
        [DataRow("First: {0}}", false)]
        [DataRow("First: {1}", false)]
        [DataRow("First: {0}, Second: {1}", true)]
        [DataRow("First: {1}, Second: {0}", true)]
        [DataRow("First: {0}, First: {0:N} Second: {1}", true)]
        [DataRow("First: {0}, Second: {1:N}", true)]
        [DataRow("First: {0}, Second: {2}", false)]
        [DataRow("First: {1}, Second: {2}", false)]
        [DataRow("First: {1}, Second: {1}", false)]
        [DataRow("First: {0N}", false)]
        public void IsFormatString(string text, bool expected)
        {
            Assert.AreEqual(expected, FormatString.IsFormatString(text));
        }

        [TestMethod]
        [DataRow(null, 0, true)]
        [DataRow(null, 1, false)]
        [DataRow("", 0, true)]
        [DataRow("", 1, false)]
        [DataRow("First", 0, true)]
        [DataRow("First", 1, false)]
        [DataRow("First: {0}", 1, true)]
        [DataRow("First: {0} ", 1, true)]
        [DataRow("First: {0}, Second: {0:N}", 1, true)]
        [DataRow("First: {0:F2 }, Second: {0:N}", 1, true)]
        [DataRow("First: {{{0}}}", 1, true)]
        [DataRow("First: {{{0:F3}}}", 1, true)]
        [DataRow("First: {0", 1, false)]
        [DataRow("First: 0}", 1, false)]
        [DataRow("First: {{0}}", 1, false)]
        [DataRow("First: {{0}", 1, false)]
        [DataRow("First: {0}}", 1, false)]
        [DataRow("First: {1}", 1, false)]
        [DataRow("First: {0}, Second: {1}", 2, true)]
        [DataRow("First: {1}, Second: {0}", 2, true)]
        [DataRow("First: {0}, First: {0:N} Second: {1}", 2, true)]
        [DataRow("First: {0}, Second: {1:N}", 2, true)]
        [DataRow("First: {0}, Second: {2}", 2, false)]
        [DataRow("First: {1}, Second: {2}", 2, false)]
        [DataRow("First: {1}, Second: {2}", 1, false)]
        [DataRow("First: {1}, Second: {2}", 3, false)]
        [DataRow("First: {0:N}", 1, true)]
        [DataRow("First: {0N}", 1, false)]
        public void IsValidFormatString(string text, int numberOfArguments, bool expected)
        {
            Assert.AreEqual(expected, FormatString.IsValidFormatString(text, numberOfArguments));
        }

        [TestMethod]
        [DataRow("", true, 0, false)]
        [DataRow("First", true, 0, false)]
        [DataRow("First: {{0}}", true, 0, false)]
        [DataRow("First: {{0}", false, -1, false)]
        [DataRow("First: {0}}", false, -1, false)]
        [DataRow("First: {0}", true, 1, false)]
        [DataRow("First: {{{0}}}", true, 1, false)]
        [DataRow("First: {{{0:F3}}}", true, 1, true)]
        [DataRow("First: {0:F2 }, Second: {0:N}", true, 1, true)]
        [DataRow("First: {0}, Second: {0:N}", true, 1, true)]
        [DataRow("First: {0}, Second: {1}", true, 2, false)]
        [DataRow("First: {1}, Second: {0}", true, 2, false)]
        [DataRow("First: {1}, Second: {2}", false, -1, false)]
        [DataRow("First: {0}, First: {0:N} Second: {1}", true, 2, true)]
        [DataRow("First: {0}, Second: {1:N}", true, 2, true)]
        [DataRow("First: {0:N}", true, 1, true)]
        [DataRow("First: {0} ", true, 1, false)]
        [DataRow("First: {1}", false, -1, false)]
        [DataRow("First: {0N}", false, -1, null)]
        public void IsValidFormatWithOutParams(string text, bool expected, int expectedIndex, bool? expectedFormat)
        {
            Assert.AreEqual(expected, FormatString.IsValidFormat(text, out int count, out bool? anyItemHasFormat));
            Assert.AreEqual(expectedIndex, count);
            Assert.AreEqual(expectedFormat, anyItemHasFormat);
        }
    }
}
