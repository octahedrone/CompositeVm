using System;
using Composite.Core.Tests.EditrableTargets;

namespace Composite.Core.Tests
{
    public class StringPropertyDataEditor : PropertyChangedBase, IDataEditor<EditableStruct>
    {
        private EditableStruct _editableTarget;

        public string Value
        {
            get { return _editableTarget.Text; }

            set
            {
                if (_editableTarget.Text == value)
                    return;

                _editableTarget.Text = value;

                OnTargetUpdated("Text");
            }
        }

        public EditableStruct EditableTarget
        {
            get { return _editableTarget; }
            set
            {
                var oldValue = _editableTarget;

                _editableTarget = value;

                if (oldValue.Text != _editableTarget.Text)
                {
                    OnPropertyChanged("Value");
                }
            }
        }

        public event EventHandler<PropertyUpdatedEventArgs> TargetUpdated;

        private void OnTargetUpdated(string targetPropertyName)
        {
            var handler = TargetUpdated;

            if (handler != null)
                handler(this, new PropertyUpdatedEventArgs(targetPropertyName));
        }
    }
}