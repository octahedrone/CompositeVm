using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Composite.Core.PropertyEditors
{
    public class EditorComponent<TValue> : IEditorComponent<TValue>, IDataErrorInfo, INotifyPropertyChanged
    {
        private TValue _value;
        private string _error;

        private readonly IEqualityComparer<TValue> _valueComparer;
        private bool _isReadOnly;

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

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            private set
            {
                if (_isReadOnly == value)
                    return;

                _isReadOnly = value;

                OnPropertyChanged("IsReadOnly");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs> ValueUpdated;

        void IEditorComponent<TValue>.SetReadOnly(bool readOnly)
        {
            IsReadOnly = readOnly;
        }

        TValue IBrowserComponent<TValue>.GetValue()
        {
            return _value;
        }

        void IBrowserComponent<TValue>.SetValue(TValue value)
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

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
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