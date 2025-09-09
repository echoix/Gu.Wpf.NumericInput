namespace Gu.Wpf.NumericInput.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    public class LongBoxTests : NumericBoxTests<LongBox, long>
    {
        protected override long ExpectedUnitValue => 1L;

        protected override long Max => 10;

        protected override long Min => -10;

        protected override long Increment => 1;

        protected override Func<LongBox> Creator => () => new LongBox();

        [TestMethod]
        [DataRow(-8L)]
        public override void DecreaseCommandCanExecuteOnDecrease(long value) =>
            base.DecreaseCommandCanExecuteOnDecrease(value);

        [TestMethod]
        [DataRow(8L)]
        public override void IncreaseCommandCanExecuteChangedOnIncrease(long value) =>
            base.IncreaseCommandCanExecuteChangedOnIncrease(value);
    }
}
