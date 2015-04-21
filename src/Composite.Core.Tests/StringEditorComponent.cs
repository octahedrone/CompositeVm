using System;

namespace Composite.Core.Tests
{
    public class StringEditorComponent : PropertyChangedBase, IStringEditorComponent
    {
        private string _value;

        public string Value
        {
            get { return _value; }

            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                OnValueUpdated();
            }
        }

        public event EventHandler<EventArgs> ValueUpdated;

        string IStringEditorComponent.GetValue()
        {
            return _value;
        }

        void IStringEditorComponent.SetValue(string value)
        {
            if (_value == value)
            {
                return;
            }

            _value = value;

            OnPropertyChanged("Value");
        }

        private void OnValueUpdated()
        {
            var handler = ValueUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}