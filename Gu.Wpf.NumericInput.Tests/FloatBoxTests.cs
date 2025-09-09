namespace Gu.Wpf.NumericInput.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    public class FloatBoxTests : FloatBaseTests<FloatBox, float>
    {
        protected override float ExpectedUnitValue => 1f;

        protected override float Max => 10;

        protected override float Min => -10;

        protected override float Increment => 1;

        protected override Func<FloatBox> Creator => () => new FloatBox();
    }
}
