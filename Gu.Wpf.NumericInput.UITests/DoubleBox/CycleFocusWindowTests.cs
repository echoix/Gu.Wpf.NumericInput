namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.runtimeconfig.json")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.dll")]
    [DeploymentItem("Gu.Wpf.NumericInput.Demo.exe")]
    [DeploymentItem("Gu.Wpf.NumericInput.dll")]
    public class CycleFocusWindowTests
    {
        private const string WindowName = "CycleFocusWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [ClassInitialize]
        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void OneTimeTearDown(TestContext testContext)
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void WithSpinners(bool withSpinners)
        {
            using var application = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = application.MainWindow;
            var doubleBoxes = window.FindGroupBox("DoubleBoxes");
            var textBox = doubleBoxes.FindTextBox("TextBox1");
            textBox.Click();
            window.FindGroupBox("Settings").FindCheckBox("AllowSpinners").IsChecked = withSpinners;
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            var doubleBox3 = doubleBoxes.FindTextBox("DoubleBox3");

            textBox.Click();
            Assert.AreEqual(true, textBox.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(false, textBox.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(false, textBox.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(false, textBox.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox3.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            window.WaitUntilResponsive();
            Assert.AreEqual(true, textBox.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.HasKeyboardFocus);
        }
    }
}
