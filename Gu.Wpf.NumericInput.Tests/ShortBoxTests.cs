namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    public class ShortBoxTests : NumericBoxTests<ShortBox, short>
    {
        protected override Func<ShortBox> Creator => () => new ShortBox();

        protected override short ExpectedUnitValue => (short)1;

        protected override short Max => 10;

        protected override short Min => -10;

        protected override short Increment => 1;

        [TestMethod]
        public void IncreaseOverflow()
        {
            var box = new ShortBox
            {
                MaxValue = short.MaxValue,
                Value = short.MaxValue - 1,
                Increment = 10,
            };
            box.IncreaseCommand!.Execute(null);
            Assert.AreEqual(short.MaxValue, box.Value);
        }

        [TestMethod]
        [DataRow((short)-8)]
        public override void DecreaseCommandCanExecuteOnDecrease(short value) => base.DecreaseCommandCanExecuteOnDecrease(value);

        [TestMethod]
        [DataRow("100", "99", (short)0)]
        [DataRow("0", "-1", (short)-1)]
        [DataRow("-9", "-10", (short)-10)]
        [DataRow("-10", "-10", (short)-10)]
        public override void DecreaseCommandExecute(string text, string expectedText, short expected) => base.DecreaseCommandExecute(text, expectedText, expected);

        [TestMethod]
        [DataRow("100", "99", (short)0)]
        [DataRow("10", "9", (short)9)]
        [DataRow("0", "-1", (short)-1)]
        [DataRow("-9", "-10", (short)-10)]
        [DataRow("-10", "-10", (short)-10)]
        public override void DecreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, short expected) => base.DecreaseCommandExecuteSpinUpdateModePropertyChanged(text, expectedText, expected);

        [TestMethod]
        public void DecreaseOverflow()
        {
            var box = new ShortBox
            {
                MinValue = short.MinValue,
                Value = short.MinValue + 1,
                Increment = 10,
            };
            box.DecreaseCommand!.Execute(null);
            Assert.AreEqual(short.MinValue, box.Value);
        }

        [TestMethod]
        [DataRow((short)8)]
        public override void IncreaseCommandCanExecuteChangedOnIncrease(short value) => base.IncreaseCommandCanExecuteChangedOnIncrease(value);

        [TestMethod]
        [DataRow("-100", "-99", (short)0)]
        [DataRow("0", "1", (short)1)]
        [DataRow("9", "10", (short)10)]
        [DataRow("10", "10", (short)10)]
        public override void IncreaseCommandExecute(string text, string expectedText, short expected) => base.IncreaseCommandExecute(text, expectedText, expected);

        [TestMethod]
        [DataRow("-100", "-99", (short)0)]
        [DataRow("-10", "-9", (short)-9)]
        [DataRow("0", "1", (short)1)]
        [DataRow("9", "10", (short)10)]
        [DataRow("10", "10", (short)10)]
        public override void IncreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, short expected) => base.IncreaseCommandExecuteSpinUpdateModePropertyChanged(text, expectedText, expected);

        [TestMethod]
        [DataRow((short)9, false, (short)8, true)]
        [DataRow((short)10, false, (short)11, false)]
        [DataRow((short)11, true, (short)15, false)]
        public override void SetMaxValidates(short value, bool expected, short newMax, bool expected2) => base.SetMaxValidates(value, expected, newMax, expected2);

        [TestMethod]
        [DataRow((short)-9, false, (short)-8, true)]
        [DataRow((short)-10, false, (short)-11, false)]
        [DataRow((short)-11, true, (short)-15, false)]
        public override void SetMinValidates(short value, bool expected, short newMax, bool expected2) => base.SetMinValidates(value, expected, newMax, expected2);

        [TestMethod]
        [DataRow((short)1, "11", true, "1", false)]
        public override void SetTextTwiceTest(short vmValue, string text1, bool expected1, string text2, bool expected2) => base.SetTextTwiceTest(vmValue, text1, expected1, text2, expected2);

        [TestMethod]
        [DataRow((short)9, false)]
        [DataRow((short)10, false)]
        [DataRow((short)11, true)]
        [DataRow((short)-9, false)]
        [DataRow((short)-10, false)]
        [DataRow((short)-11, true)]
        public override void SetValueValidates(short value, bool expected) => base.SetValueValidates(value, expected);
    }
}
