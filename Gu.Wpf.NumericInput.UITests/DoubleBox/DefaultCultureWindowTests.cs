namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.runtimeconfig.json")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.dll")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.exe")]
    [DeploymentItem("Gu.Wpf.NumericInput.dll")]
    public class DefaultCultureWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [ClassInitialize]
        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void OneTimeSetUp(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        public void OnLoad()
        {
            using var application = Application.Launch(ExeFileName, "DefaultCultureWindow", OnDispose.KillProcess);
            var window = application.MainWindow;
            var valueTextBox = window.FindTextBox("ValueTextBox");
            var spinnerDoubleBox = window.FindTextBox("SpinnerDoubleBox");
            var doubleBox = window.FindTextBox("DoubleBox");
            valueTextBox.Enter("1.234");
            Keyboard.Type(Key.TAB);
            Assert.AreEqual("1.234", spinnerDoubleBox.Text);
            Assert.AreEqual("1.234", doubleBox.Text);
        }
    }
}
