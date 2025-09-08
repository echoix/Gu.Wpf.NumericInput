namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    //[STATestClass]
    [TestClass]
    public class ValidationErrorRegexTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static IEnumerable<object[]> TestCases => new[]
        {
            new object[] { "1.2", @"^\d\.\d$", "1.2", null },
            new object[] { "12.34", @"^\d\.\d$", "0", "ValidationError.RegexValidationResult 'Please provide valid input.'" },
        };

        private static IEnumerable<object[]> SwedishCases => new[]
        {
            new object[] { "1,2",  @"^\d,\d$", "1.2", null },
            new object[] { "12,34",  @"^\d,\d$", "0", "ValidationError.RegexValidationResult 'Vänligen ange ett giltigt värde.'" },
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
        [DynamicData(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("RegexPattern").Text = pattern;

            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
#pragma warning disable CA1801, IDE0060 // Review unused parameters
        public void LostFocusValidateOnLostFocusWhenPatternChanges(string text, string pattern, string expected, string expectedInfoMessage)
#pragma warning restore CA1801, IDE0060 // Review unused parameters
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindTextBox("RegexPattern").Text = pattern;
            window.FindButton("lose focus").Click();
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void PropertyChanged(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestMethod]
        [DynamicData(nameof(SwedishCases))]
        public void PropertyChangedSwedish(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestMethod]
        [DynamicData(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");

            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;
            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        private static string GetErrorMessage(string infoMessage)
        {
            return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
        }
    }
}
