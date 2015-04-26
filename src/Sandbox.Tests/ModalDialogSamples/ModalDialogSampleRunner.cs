using System.Windows.Controls;
using NUnit.Framework;

namespace Sandbox.Tests.ModalDialogSamples
{
    [TestFixture]
    [RequiresSTA]
    [Ignore("UI blocking test")]
    public class ModalDialogSampleRunner
    {
        [Test]
        public void RunSampleEditor()
        {
            var testData = new TextBlock {Text = "Wa-ha!"};

            testData.ShowModal();
        }
    }
}