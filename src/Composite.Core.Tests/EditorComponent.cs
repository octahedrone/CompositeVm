using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Composite.Core.Tests
{
    public class EditorComponent<TValue> : PropertyChangedBase, IEditorComponent<TValue>, IDataErrorInfo
    {
        private TValue _value;
        private string _error;

        private readonly IEqualityComparer<TValue> _valueComparer;

        public EditorComponent()
            : this(EqualityComparer<TValue>.Default)
        {
        }

        public EditorComponent(IEqualityComparer<TValue> valueComparer)
        {
            if (valueComparer == null) throw new ArgumentNullException("valueComparer");

            _valueComparer = valueComparer;
        }

        public TValue Value
        {
            get { return _value; }

            set
            {
                if (_valueComparer.Equals(_value, value))
                {
                    return;
                }

                _value = value;

                OnValueUpdated();
            }
        }

        public event EventHandler<EventArgs> ValueUpdated;

        TValue IEditorComponent<TValue>.GetValue()
        {
            return _value;
        }

        void IEditorComponent<TValue>.SetValue(TValue value)
        {
            if (_valueComparer.Equals(_value, value))
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