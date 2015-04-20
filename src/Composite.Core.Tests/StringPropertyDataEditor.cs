using System;

namespace Composite.Core.Tests
{
    public class StringPropertyDataEditor<TData> : PropertyChangedBase, IDataEditor<TData>
    {
        private readonly IStringPropertyAdapter<TData> _propertyAdapter;
        
        private TData _editableTarget;

        public StringPropertyDataEditor(IStringPropertyAdapter<TData> propertyAdapter)
        {
            if (propertyAdapter == null) throw new ArgumentNullException("propertyAdapter");

            _propertyAdapter = propertyAdapter;
        }

        public string Value
        {
            get { return _propertyAdapter.GetValue(_editableTarget); }

            set
            {
                var currentValue = _propertyAdapter.GetValue(_editableTarget);

                if (currentValue == value)
                    return;

                _editableTarget = _propertyAdapter.SetValue(_editableTarget, value);

                OnTargetUpdated(_propertyAdapter.PropertyName);
            }
        }

        public TData EditableTarget
        {
            get { return _editableTarget; }
            set
            {
                var oldValue = _propertyAdapter.GetValue(_editableTarget);

                _editableTarget = value;

                var newValue = _propertyAdapter.GetValue(_editableTarget);

                if (newValue != oldValue)
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