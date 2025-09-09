namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [STATestClass]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.runtimeconfig.json")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.dll")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.exe")]
    [DeploymentItem("Gu.Wpf.NumericInput.dll")]
    public class TouchTests
    {
        private const string WindowName = "TouchWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [ClassInitialize]
        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void OneTimeSetUp(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        public void Tap()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;
            Touch.Tap(window.FindTextBox("TextBox1").GetClickablePoint());
            Wait.UntilInputIsProcessed();

            Touch.Tap(window.FindTextBox("TextBox2").GetClickablePoint());
            Wait.UntilInputIsProcessed();
        }
    }
}
