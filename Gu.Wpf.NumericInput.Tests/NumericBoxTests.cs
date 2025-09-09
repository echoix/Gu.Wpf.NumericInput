#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class NumericBoxTests<TBox, T>
        : BaseBoxTests
        where TBox : NumericBox<T>
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        protected new TBox Box => (TBox)base.Box!;

        protected abstract T ExpectedUnitValue { get; }

        protected abstract Func<TBox> Creator { get; }

        protected abstract T Max { get; }

        protected abstract T Min { get; }

        protected abstract T Increment { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected DummyVm<T> Vm { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void SetUp()
        {
            var enUs = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = enUs;
            Thread.CurrentThread.CurrentCulture = enUs;
            base.Box = this.Creator();
            this.Box.IsReadOnly = false;
            this.Box.MinValue = this.Min;
            this.Box.MaxValue = this.Max;
            this.Box.Increment = this.Increment;
            this.Box.Culture = Thread.CurrentThread.CurrentUICulture;
            this.Vm = new DummyVm<T>();
            var binding = new Binding("Value")
            {
                Source = this.Vm,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                Mode = BindingMode.TwoWay,
            };
            _ = BindingOperations.SetBinding(this.Box, NumericBox<T>.ValueProperty, binding);
            this.Box.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
        }

        [TestMethod]
        public void Defaults()
        {
            var box = this.Creator();
            Assert.AreEqual(this.ExpectedUnitValue, box.Increment);
            var typeMin = (T)typeof(T).GetField("MinValue")!.GetValue(null)!;
            Assert.AreEqual(typeMin, box.MinLimit);
            Assert.IsNull(box.MinValue);

            var typeMax = (T)typeof(T).GetField("MaxValue")!.GetValue(null)!;
            Assert.AreEqual(typeMax, box.MaxLimit);
            Assert.IsNull(box.MaxValue);
        }

        [TestMethod]
        [DataRow(9, false)]
        [DataRow(10, false)]
        [DataRow(11, true)]
        [DataRow(-9, false)]
        [DataRow(-10, false)]
        [DataRow(-11, true)]
        public virtual void SetValueValidates(T value, bool expected)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
            Assert.AreEqual(value.ToString(this.Box.StringFormat, this.Box.Culture), this.Box.Text);
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
        }

        [TestMethod]
        [DataRow(9, false, 8, true)]
        [DataRow(10, false, 11, false)]
        [DataRow(11, true, 15, false)]
        public virtual void SetMaxValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
            this.Box.MaxValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
        }

        [TestMethod]
        [DataRow(-9, false, -8, true)]
        [DataRow(-10, false, -11, false)]
        [DataRow(-11, true, -15, false)]
        public virtual void SetMinValidates(T value, bool expected, T newMax, bool expected2)
        {
            this.Vm.Value = value;
            Assert.AreEqual(expected, Validation.GetHasError(this.Box));
            this.Box.MinValue = newMax;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.ValueBinding, this.Box.TextSource);
        }

        [TestMethod]
        [DataRow(1, "11", true, "1", false)]
        public virtual void SetTextTwiceTest(T vmValue, string text1, bool expected1, string text2, bool expected2)
        {
            this.Vm.Value = vmValue;
            this.Box.Text = text1;
            Assert.AreEqual(expected1, Validation.GetHasError(this.Box));

            this.Box.Text = text2;
            Assert.AreEqual(expected2, Validation.GetHasError(this.Box));
            ////Assert.Fail("11 -> 1");
            Assert.AreEqual(Status.Idle, this.Box.Status);
            Assert.AreEqual(TextSource.UserInput, this.Box.TextSource);
        }

        [TestMethod]
        public virtual void ValueUpdatesWhenTextIsSet()
        {
            this.Box.Text = "1";
            Assert.AreEqual(this.ExpectedUnitValue, this.Box.GetValue(NumericBox<T>.ValueProperty));
        }

        [TestMethod]
        public void TextUpdatesWhenValueChanges()
        {
#pragma warning disable WPF0014 // SetValue must use registered type.
            this.Box.SetValue(NumericBox<T>.ValueProperty, this.ExpectedUnitValue);
#pragma warning restore WPF0014 // SetValue must use registered type.
            Assert.AreEqual("1", this.Box.Text);
        }

        [TestMethod]
        public void ValidationErrorResetsValue()
        {
            this.Box.Text = "1";
            Assert.AreEqual(false, Validation.GetHasError(base.Box));
            Assert.AreEqual(this.ExpectedUnitValue, this.Box.Value);
            Assert.AreEqual(null, this.Vm.Value);

            this.Box.Text = "1e";
            Assert.AreEqual(true, Validation.GetHasError(base.Box));
            Assert.AreEqual("1e", this.Box.Text);
            Assert.AreEqual(this.Vm.Value, this.Box.Value);
        }

        [TestMethod]
        [DataRow("-100", "-99", 0)]
        [DataRow("0", "1", 1)]
        [DataRow("9", "10", 10)]
        [DataRow("10", "10", 10)]
        public virtual void IncreaseCommandExecute(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.IncreaseCommand!.Execute(null);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expected, this.Box.Value);
            Assert.AreEqual(this.Box.Parse("0"), this.Vm.Value);
        }

        [TestMethod]
        [DataRow("-100", "-99", 0)]
        [DataRow("-10", "-9", -9)]
        [DataRow("0", "1", 1)]
        [DataRow("9", "10", 10)]
        [DataRow("10", "10", 10)]
        public virtual void IncreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.SpinUpdateMode = SpinUpdateMode.PropertyChanged;
            this.Box.IncreaseCommand!.Execute(null);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expected, this.Box.Value);
            Assert.AreEqual(this.Box.Parse(expected.ToString(CultureInfo.InvariantCulture)), this.Vm.Value);
        }

        [TestMethod]
        [DataRow("9", true)]
        [DataRow("10", false)]
        [DataRow("11", false)]
        [DataRow("1e", false)]
        public void IncreaseCommandCanExecuteOnUserInput(string text, bool expected)
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            this.Box.Text = text;
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Box.Text = string.Empty;
            Assert.AreEqual(false, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void IncreaseCommandCanExecuteRaiseExplicit()
        {
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            ((ManualRelayCommand)this.Box.IncreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [DataRow(8)]
        public virtual void IncreaseCommandCanExecuteChangedOnIncrease(T value)
        {
            this.Box.AllowSpinners = true;
            this.Box.Value = value;
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            Assert.IsTrue(this.Box.IncreaseCommand.CanExecute(null));

            this.Box.IncreaseCommand.Execute(null);
            Assert.AreEqual("9", this.Box.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Box.IncreaseCommand.CanExecute(null));

            this.Box.IncreaseCommand.Execute(null);
            Assert.AreEqual("10", this.Box.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(this.Box.IncreaseCommand.CanExecute(null));
        }

        [TestMethod]
        public void IncreaseCommandCanExecuteChangedOnValueChanged()
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (sender, args) => count++;
            this.Vm.Value = this.Box.Parse("1");
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Vm.Value = this.Box.Parse("2");
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [DataRow(true, false)]
        [DataRow(false, true)]
        public void IncreaseCommandCanExecuteIsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            base.Box!.Text = "0";
            var count = 0;
            this.Box.IncreaseCommand!.CanExecuteChanged += (_, __) => count++;
            base.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.IncreaseCommand.CanExecute(null));
            Assert.AreEqual(@readonly ? 1 : 0, count);
        }

        [TestMethod]
        [DataRow("100", "99", 0)]
        [DataRow("0", "-1", -1)]
        [DataRow("-9", "-10", -10)]
        [DataRow("-10", "-10", -10)]
        public virtual void DecreaseCommandExecute(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.DecreaseCommand!.Execute(null);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expected, this.Box.Value);
        }

        [TestMethod]
        [DataRow("100", "99", 0)]
        [DataRow("10", "9", 9)]
        [DataRow("0", "-1", -1)]
        [DataRow("-9", "-10", -10)]
        [DataRow("-10", "-10", -10)]
        public virtual void DecreaseCommandExecuteSpinUpdateModePropertyChanged(string text, string expectedText, T expected)
        {
            this.Vm.Value = this.Box.Parse("0");
            this.Box.Text = text;
            this.Box.SpinUpdateMode = SpinUpdateMode.PropertyChanged;
            this.Box.DecreaseCommand!.Execute(null);
            Assert.AreEqual(expectedText, this.Box.Text);
            Assert.AreEqual(expected, this.Box.Value);
            Assert.AreEqual(this.Box.Parse(expected.ToString(CultureInfo.InvariantCulture)), this.Vm.Value);
        }

        [TestMethod]
        public void DecreaseCommandCanExecuteRaiseExplicit()
        {
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (_, __) => count++;
            ((ManualRelayCommand)this.Box.DecreaseCommand).RaiseCanExecuteChanged();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [DataRow("-9", true)]
        [DataRow("-10", false)]
        [DataRow("-11", false)]
        [DataRow("-1e", false)]
        public void DecreaseCommandCanExecuteOnUserInput(string text, bool expected)
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (_, __) => count++;
            this.Box.Text = text;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Box.Text = string.Empty;
            Assert.AreEqual(false, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [DataRow(-8)]
        public virtual void DecreaseCommandCanExecuteOnDecrease(T value)
        {
            this.Box.AllowSpinners = true;
            this.Box.Value = value;
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (sender, args) => count++;
            Assert.IsTrue(this.Box.DecreaseCommand.CanExecute(null));

            this.Box.DecreaseCommand.Execute(null);
            Assert.AreEqual("-9", this.Box.Text);
            Assert.AreEqual(1, count);
            Assert.IsTrue(this.Box.DecreaseCommand.CanExecute(null));

            this.Box.DecreaseCommand.Execute(null);
            Assert.AreEqual("-10", this.Box.Text);
            Assert.AreEqual(2, count);
            Assert.IsFalse(this.Box.DecreaseCommand.CanExecute(null));
        }

        [TestMethod]
        public void DecreaseCommandCanExecuteChangedOnValueChanged()
        {
            this.Box.AllowSpinners = true;
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (sender, args) => count++;
            this.Vm.Value = this.Box.Parse("1");
            Assert.AreEqual(1, count);

            this.Box.AllowSpinners = false;
            Assert.AreEqual(2, count);

            this.Vm.Value = this.Box.Parse("2");
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [DataRow(true, false)]
        [DataRow(false, true)]
        public void DecreaseCommandCanExecuteIsReadonly(bool @readonly, bool expected)
        {
            this.Box.AllowSpinners = true;
            this.Box.Text = "0";
            var count = 0;
            this.Box.DecreaseCommand!.CanExecuteChanged += (_, __) => count++;
            this.Box.IsReadOnly = @readonly;
            Assert.AreEqual(expected, this.Box.DecreaseCommand.CanExecute(null));
            Assert.AreEqual(@readonly ? 1 : 0, count);
        }
    }
}
