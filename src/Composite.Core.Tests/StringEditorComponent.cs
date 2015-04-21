using System;
using System.ComponentModel;

namespace Composite.Core.Tests
{
    public class StringEditorComponent : PropertyChangedBase, IStringEditorComponent, IDataErrorInfo
    {
        private string _value;
        private string _error;

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

        public void SetError(string error)
        {
            if (_error == error)
                return;

            _error = string.IsNullOrEmpty(error) ? null : error;

            // this is how IDataErrorInfo 'notification' works
            OnPropertyChanged("Value");
        }

        private void OnValueUpdated()
        {
            var handler = ValueUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (columnName != "Value")
                    return null;

                return _error;
            }
        }

        string IDataErrorInfo.Error
        {
            get { return _error; }
        }
    }
}