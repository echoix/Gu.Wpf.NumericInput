namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Globalization;

    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.runtimeconfig.json")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.dll")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.exe")]
    [DeploymentItem("Gu.Wpf.NumericInput.dll")]
    public class FormatTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly CultureInfo EnUs = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo SvSe = CultureInfo.GetCultureInfo("sv-SE");

        private static IEnumerable<object[]> TestCases => new[]
        {
            new object[] { "1", "F1", EnUs, "1.0", "1" },
            new object[] { "1", "F1", SvSe, "1,0", "1" },
            new object[] { "1.23456", "F3", EnUs, "1.235", "1.23456" },
            new object[] { "1.23456", "F4", EnUs, "1.2346", "1.23456" },
            new object[] { "1", "0.#", EnUs, "1", "1" },
            new object[] { "1.23456", "0.###", EnUs, "1.235", "1.23456" },
        };

        [TestInitialize]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindButton("Reset").Invoke();
            window.WaitUntilResponsive();
        }

        [ClassInitialize]
        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void OneTimeTearDown(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void WithStringFormat(string text, string stringFormat, CultureInfo culture, string formatted, string viewModelValue)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("StringFormat").Text = stringFormat;
            _ = window.FindComboBox("Culture").Select(culture.Name);

            doubleBox.Enter(text);
            window.FindButton("lose focus").Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(formatted, doubleBox.FormattedView().Text);
            Assert.AreEqual(viewModelValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        public void WhenStringFormatChangesBindingLostFocusValidateOnPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("StringFormat").Text = "F1";

            doubleBox.Text = "1.23456";
            window.FindButton("lose focus").Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindTextBox("StringFormat").Text = "F4";
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2346", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindTextBox("StringFormat").Text = "F1";
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        public void WhenStringFormatChangesBindingPropertyChangedValidateOnPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("StringFormat").Text = "F1";

            doubleBox.Text = "1.23456";
            window.FindButton("lose focus").Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindTextBox("StringFormat").Text = "F4";
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2346", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindTextBox("StringFormat").Text = "F1";
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }
    }
}
