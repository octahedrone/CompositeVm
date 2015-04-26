using Composite.Core;
using Composite.Core.PropertyAdapters;
using Sandbox.Tests.ModalDialogSamples.Model;

namespace Sandbox.Tests.ModalDialogSamples.ViewModel.ModelAdapters
{
    public static class SampleUserMetadata
    {
        public static readonly IPropertyAdapter<SampleUser, string> NameProperty;

        static SampleUserMetadata()
        {
            NameProperty = new ClassTypeGenericPropertyAdapter<SampleUser, string>(_ => _.Name, (t, v) => t.Name = v);
        }
    }
}