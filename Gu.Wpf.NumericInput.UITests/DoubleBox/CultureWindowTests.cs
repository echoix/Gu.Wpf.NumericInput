namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.runtimeconfig.json")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.dll")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.exe")]
    [DeploymentItem("Gu.Wpf.NumericInput.dll")]
    public class CultureWindowTests
    {
        private const string WindowName = "CultureWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

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
        public static void OneTimeSetUp(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        [DataRow("SpinnerDoubleBox", "1,234", "1,234")]
        [DataRow("InheritingCultureDoubleBox", "1,234", "1,234")]
        [DataRow("SvSeDoubleBox", "1,234", "1,234")]
        [DataRow("EnUsDoubleBox", "1.234", "1.234")]
        [DataRow("BoundCultureDoubleBox", "1,234", "1.234")]
        public void Formats(string name, string expectedSv, string expectedEn)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var textBox = window.FindTextBox(name);
            Assert.AreEqual(expectedSv, textBox.Text);

            window.FindTextBox("CultureTextBox").Text = "en-us";
            Keyboard.Type(Key.TAB);
            Assert.AreEqual(expectedEn, textBox.Text);
        }
    }
}
