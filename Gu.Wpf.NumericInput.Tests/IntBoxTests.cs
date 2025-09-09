namespace Gu.Wpf.NumericInput.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    public class IntBoxTests : NumericBoxTests<IntBox, int>
    {
        protected override int ExpectedUnitValue => 1;

        protected override int Max => 10;

        protected override int Min => -10;

        protected override int Increment => 1;

        protected override Func<IntBox> Creator => () => new IntBox();
    }
}
