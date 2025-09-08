namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;

    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    [TestClass]
    public class ValidationHappyPathTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static IEnumerable<object[]> EnglishCases => new[]
        {
            new object[] { "1", "1" },
            new object[] { " 1", "1" },
            new object[] { "1 ", "1" },
            new object[] { " 1 ", "1" },
            new object[] { "1.2", "1.2" },
            new object[] { "-1.2", "-1.2" },
            new object[] { "+1.2", "1.2" },
            new object[] { ".1", "0.1" },
            new object[] { "-.1", "-0.1" },
            new object[] { "0.1", "0.1" },
            new object[] { "1e1", "10" },
            new object[] { "1e0", "1" },
            new object[] { "1e-1", "0.1" },
            new object[] { "1E1", "10" },
            new object[] { "1E0", "1" },
            new object[] { "1E-1", "0.1" },
            new object[] { "-1e1", "-10" },
            new object[] { "-1e0", "-1" },
            new object[] { "-1e-1", "-0.1" },
            new object[] { "-1E1", "-10" },
            new object[] { "-1E0", "-1" },
            new object[] { "-1E-1", "-0.1" },
        };

        private static IEnumerable<object[]> SwedishCases => new[]
        {
            new object[] { "1", "1" },
            new object[] { " 1", "1" },
            new object[] { "1 ", "1" },
            new object[] { " 1 ", "1" },
            new object[] { "1,2", "1.2" },
            new object[] { "-1,2", "-1.2" },
            new object[] { "+1,2", "1.2" },
            new object[] { ",1", "0.1" },
            new object[] { "-,1", "-0.1" },
            new object[] { "0,1", "0.1" },
            new object[] { "1e1", "10" },
            new object[] { "1e0", "1" },
            new object[] { "1e-1", "0.1" },
            new object[] { "1E1", "10" },
            new object[] { "1E0", "1" },
            new object[] { "1E-1", "0.1" },
            new object[] { "-1e1", "-10" },
            new object[] { "-1e0", "-1" },
            new object[] { "-1e-1", "-0.1" },
            new object[] { "-1E1", "-10" },
            new object[] { "-1E0", "-1" },
            new object[] { "-1E-1", "-0.1" },
        };

        private static IEnumerable<object[]> MinMaxSource => new[]
        {
            new object[] { "1", string.Empty, string.Empty, "1" },
            new object[] { "-1", "-1", string.Empty, "-1" },
            new object[] { "-1", "-10", string.Empty, "-1" },
            new object[] { "1", string.Empty, "1", "1" },
            new object[] { "1", string.Empty, "10", "1" },
            new object[] { "-2", "-2", "2", "-2" },
            new object[] { "-1", "-2", "2", "-1" },
            new object[] { "1", "-2", "2", "1" },
            new object[] { "2", "-2", "2", "2" },
        };

        [TestInitialize]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("ViewModelValue").Text = "0";
            window.FindButton("Reset").Invoke();
        }

        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void OneTimeTearDown(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        [DynamicData(nameof(EnglishCases))]
        public void LostFocusValidateOnLostFocus(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DynamicData(nameof(EnglishCases))]
        public void LostFocusValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DynamicData(nameof(EnglishCases))]
        public void PropertyChangedValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DynamicData(nameof(SwedishCases))]
        public void SwedishLostFocusValidateOnLostFocus(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DynamicData(nameof(SwedishCases))]
        public void SwedishLostFocusValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DynamicData(nameof(SwedishCases))]
        public void SwedishPropertyChangedValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        public void WhenNullLostFocusValidateOnLostFocus()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        public void WhenNullLostFocusValidateOnPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        public void WheNullPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DynamicData(nameof(MinMaxSource))]
        public void MinMaxLostFocus(string text, string min, string max, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DynamicData(nameof(MinMaxSource))]
        public void MinMaxLostFocusValidateOnPropertyChanged(string text, string min, string max, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DynamicData(nameof(MinMaxSource))]
        public void MinMaxPropertyChanged(string text, string min, string max, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }
    }
}
