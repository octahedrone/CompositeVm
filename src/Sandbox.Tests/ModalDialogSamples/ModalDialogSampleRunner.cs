using NUnit.Framework;
using Sandbox.Tests.ModalDialogSamples.Model;
using Sandbox.Tests.ModalDialogSamples.View;
using Sandbox.Tests.ModalDialogSamples.ViewModel;

namespace Sandbox.Tests.ModalDialogSamples
{
    [TestFixture]
    [RequiresSTA]
    [Ignore("UI blocking test")]
    public class SampleUserEditorRunner
    {
        [Test]
        public void RunSampleEditor()
        {
            var testData = new SampleUser {Name = "Mr. Who"};
            var editorViewModel = new SampleUserEditorViewModel {Data = testData};

            var resources = new SampleUserEditorResources();
            editorViewModel.ShowModal(resources);
        }
    }
}