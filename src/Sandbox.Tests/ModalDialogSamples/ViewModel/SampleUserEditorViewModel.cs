using Composite.Core;
using Composite.Core.PropertyEditors;
using Sandbox.Tests.Library.TextEditor;
using Sandbox.Tests.ModalDialogSamples.Model;
using Sandbox.Tests.ModalDialogSamples.ViewModel.ModelAdapters;
using Sandbox.Tests.Validation;

namespace Sandbox.Tests.ModalDialogSamples.ViewModel
{
    public class SampleUserEditorViewModel
    {
        private readonly DataEditorsManager<SampleUser, ValidationState> _editors;

        public SampleUserEditorViewModel()
        {
            _editors = new DataEditorsManager<SampleUser, ValidationState>(new SampleUserValidator());

            InitializeEditors();
        }

        private void InitializeEditors()
        {
            NameEditor = new TextEditorComponent();

            var propertyAdapter = SampleUserMetadata.NameProperty;
            var namePropertyEditor = new PropertyDataEditor<SampleUser, string, ValidationState>(propertyAdapter,
                NameEditor,
                ValidationStateAdapter.GetPropertyError);

            _editors.Add(namePropertyEditor);
        }

        public TextEditorComponent NameEditor { get; private set; }

        public SampleUser Data
        {
            get { return _editors.EditableTarget; }
            set { _editors.EditableTarget = value; }
        }
    }
}