namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValidationErrorMinMaxTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static IEnumerable<object[]> TestCases => new[]
        {
            new object[] { "-2", "-1", string.Empty, "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.'" },
            new object[] { "-2.1", "-1.1", string.Empty, "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1.'" },
            new object[] { "-2", "-1", "1", "ValidationError.IsLessThanValidationResult 'Please enter a value between -1 and 1.'" },
            new object[] { "-2.1", "-1.1", "1.1",  "ValidationError.IsLessThanValidationResult 'Please enter a value between -1.1 and 1.1.'" },
            new object[] { "2", string.Empty, "1", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.'" },
            new object[] { "2.1", string.Empty, "1.1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1.'" },
            new object[] { "2", "-1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value between -1 and 1.'" },
            new object[] { "2.1", "-1.1", "1.1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value between -1.1 and 1.1.'" },
        };

        [TestInitialize]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindButton("Reset").Invoke();
        }

        [ClassInitialize]
        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void OneTimeTearDown(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(string text, string min, string max, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            var startValue = string.IsNullOrEmpty(min) ? max : min;
            doubleBox.Text = startValue;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;

            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(startValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(startValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(string text, string min, string max, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            var startValue = string.IsNullOrEmpty(min) ? max : min;
            doubleBox.Text = startValue;
            window.FindButton("lose focus").Click();
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(startValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(startValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void PropertyChangedValidateOnPropertyChanged(string text, string min, string max, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            var startValue = string.IsNullOrEmpty(min) ? max : min;
            doubleBox.Text = startValue;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(startValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestMethod]
        [DataRow("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Vänligen ange ett värde mindre än eller lika med 2,2.'")]
        [DataRow("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Vänligen ange ett värde större än eller lika med −2,1.'")]
        public void PropertyChangedSwedish(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "1";
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DataRow("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [DataRow("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenNotLocalized(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "1";
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DataRow("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [DataRow("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void LostFocusValidateOnLostFocusWhenMinAndMaxChanges(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = value;
            Assert.AreEqual(false, doubleBox.HasValidationError());

            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual(value, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DataRow("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [DataRow("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenMinAndMaxChanges(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = value;
            Assert.AreEqual(false, doubleBox.HasValidationError());

            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual(value, window.FindTextBox("ViewModelValue").Text);
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void PropertyChangedWhenNull(string text, string min, string max, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = string.Empty;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        public static string GetErrorMessage(string infoMessage)
        {
            return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
        }
    }
}
