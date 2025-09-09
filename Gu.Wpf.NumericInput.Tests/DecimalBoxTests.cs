namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    public class DecimalBoxTests : FloatBaseTests<DecimalBox, decimal>
    {
        protected override decimal ExpectedUnitValue => decimal.One;

        protected override decimal Max => 10;

        protected override decimal Min => -10;

        protected override decimal Increment => 1;

        protected override Func<DecimalBox> Creator => () => new DecimalBox();

        private static IEnumerable<object[]> DecreaseCommandCanExecuteOnDecreaseData =>
            new[] { new object[] { -8M } };

        private static IEnumerable<object[]> DecreaseCommandExecuteData =>
            new[]
            {
                new object[] { "100", "99", 0M },
                new object[] { "0", "-1", -1M },
                new object[] { "-9", "-10", -10M },
                new object[] { "-10", "-10", -10M },
            };

        private static IEnumerable<object[]> DecreaseCommandExecuteSpinUpdateModePropertyChangedData =>
            new[]
            {
                new object[] { "100", "99", 0M },
                new object[] { "10", "9", 9M },
                new object[] { "0", "-1", -1M },
                new object[] { "-9", "-10", -10M },
                new object[] { "-10", "-10", -10M },
            };

        private static IEnumerable<object[]> DigitsUpdatesWhenGreaterThanMaxData =>
            new[] { new object[] { (int)2, decimal.One, "1.234", "1.23" } };

        private static IEnumerable<object[]> IncreaseCommandCanExecuteChangedOnIncreaseData =>
            new[] { new object[] { 8M } };

        private static IEnumerable<object[]> IncreaseCommandExecuteData =>
            new[]
            {
                new object[] { "-100", "-99", 0M },
                new object[] { "0", "1", 1M },
                new object[] { "9", "10", 10M },
                new object[] { "10", "10", 10M },
            };

        private static IEnumerable<object[]> IncreaseCommandExecuteSpinUpdateModePropertyChangedData =>
            new[]
            {
                new object[] { "-100", "-99", 0M },
                new object[] { "-10", "-9", -9M },
                new object[] { "0", "1", 1M },
                new object[] { "9", "10", 10M },
                new object[] { "10", "10", 10M },
            };

        private static IEnumerable<object[]> SetMaxValidatesData =>
            new[]
            {
                new object[] { 9M, false, 8M, true },
                new object[] { 10M, false, 11M, false },
                new object[] { 11M, true, 15M, false },
            };

        private static IEnumerable<object[]> SetMinValidatesData =>
            new[]
            {
                new object[] { -9M, false, -8M, true },
                new object[] { -10M, false, -11M, false },
                new object[] { -11M, true, -15M, false },
            };

        private static IEnumerable<object[]> SetTextTwiceTestData =>
            new[] { new object[] { 1M, "11", true, "1", false } };

        private static IEnumerable<object[]> SetValueValidatesData =>
            new[]
            {
                new object[] { 9M, false },
                new object[] { 10M, false },
                new object[] { 11M, true },
                new object[] { -9M, false },
                new object[] { -10M, false },
                new object[] { -11M, true },
            };

        [TestMethod]
        [DynamicData(nameof(DecreaseCommandCanExecuteOnDecreaseData))]
        public override void DecreaseCommandCanExecuteOnDecrease(decimal value) =>
            base.DecreaseCommandCanExecuteOnDecrease(value);

        [TestMethod]
        [DynamicData(nameof(DecreaseCommandExecuteData))]
        public override void DecreaseCommandExecute(string text, string expectedText, decimal expected) => base.DecreaseCommandExecute(text, expectedText, expected);

        [TestMethod]
        [DynamicData(nameof(DecreaseCommandExecuteSpinUpdateModePropertyChangedData))]
        public override void DecreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, decimal expected) => base.DecreaseCommandExecuteSpinUpdateModePropertyChanged(text, expectedText, expected);

        [TestMethod]
        [DynamicData(nameof(DigitsUpdatesWhenGreaterThanMaxData))]
        public override void DigitsUpdatesWhenGreaterThanMax(int decimals, decimal max, string text, string expectedText) => base.DigitsUpdatesWhenGreaterThanMax(decimals, max, expectedText, expectedText);

        [TestMethod]
        [DynamicData(nameof(IncreaseCommandCanExecuteChangedOnIncreaseData))]
        public override void IncreaseCommandCanExecuteChangedOnIncrease(decimal value) =>
            base.IncreaseCommandCanExecuteChangedOnIncrease(value);

        [TestMethod]
        [DynamicData(nameof(IncreaseCommandExecuteData))]
        public override void IncreaseCommandExecute(string text, string expectedText, decimal expected) => base.IncreaseCommandExecute(text, expectedText, expected);

        [TestMethod]
        [DynamicData(nameof(IncreaseCommandExecuteSpinUpdateModePropertyChangedData))]
        public override void IncreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, decimal expected) => base.IncreaseCommandExecuteSpinUpdateModePropertyChanged(text, expectedText, expected);

        [TestMethod]
        [DynamicData(nameof(SetMaxValidatesData))]
        public override void SetMaxValidates(decimal value, bool expected, decimal newMax, bool expected2) => base.SetMaxValidates(value, expected, newMax, expected2);

        [TestMethod]
        [DynamicData(nameof(SetMinValidatesData))]
        public override void SetMinValidates(decimal value, bool expected, decimal newMax, bool expected2) => base.SetMinValidates(value, expected, newMax, expected2);

        [TestMethod]
        [DynamicData(nameof(SetTextTwiceTestData))]
        public override void SetTextTwiceTest(decimal vmValue, string text1, bool expected1, string text2, bool expected2) => base.SetTextTwiceTest(vmValue, text1, expected1, text2, expected2);

        [TestMethod]
        [DynamicData(nameof(SetValueValidatesData))]
        public override void SetValueValidates(decimal value, bool expected) =>
            base.SetValueValidates(value, expected);
    }
}
