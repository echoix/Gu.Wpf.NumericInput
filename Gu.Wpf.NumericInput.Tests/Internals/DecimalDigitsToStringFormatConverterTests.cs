namespace Gu.Wpf.NumericInput.Tests.Internals
{
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DecimalDigitsToStringFormatConverterTests
    {
        [TestMethod]
        [DataRow(null, 12345.678901, "12345.678901")]
        [DataRow(0, 12345.678901, "12346")]
        [DataRow(1, 12345.678901, "12345.7")]
        [DataRow(3, 12345.678901, "12345.679")]
        [DataRow(9, 12345.678901, "12345.678901000")]
        [DataRow(-3, 12345.678901, "12345.679")]
        [DataRow(-3, 12345.6, "12345.6")]
        [DataRow(3, 12345.6, "12345.600")]
        public void Convert(int? digits, double value, string expected)
        {
            var converter = DecimalDigitsToStringFormatConverter.Default;
            var format = (string?)converter.Convert(digits, null, null, null);
            var actual = value.ToString(format, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, actual);
        }
    }
}
