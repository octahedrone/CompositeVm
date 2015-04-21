using System;
using Composite.Core.Tests.EditrableTargets;

namespace Composite.Core.Tests
{
    public class StringPropertyDataEditor<TData> : PropertyChangedBase, IValidatedDataEditor<TData, ValidationState>
    {
        private readonly IEditorComponent<string> _component;
        private readonly IPropertyAdapter<TData, string> _propertyAdapter;

        private TData _editableTarget;

        public StringPropertyDataEditor(IPropertyAdapter<TData, string> propertyAdapter, IEditorComponent<string> component)
        {
            if (propertyAdapter == null) throw new ArgumentNullException("propertyAdapter");

            _propertyAdapter = propertyAdapter;
            _component = component;

            _component.ValueUpdated += OnComponentUpdatedValue;
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
                    _component.SetValue(newValue);
                }
            }
        }

        public object Component
        {
            get { return _component; }
        }

        public event EventHandler<PropertyUpdatedEventArgs> TargetUpdated;

        private void OnComponentUpdatedValue(object sender, EventArgs e)
        {
            var oldValue = _propertyAdapter.GetValue(_editableTarget);
            var newValue = _component.GetValue();

            if (newValue == oldValue)
            {
                return;
            }

            _editableTarget = _propertyAdapter.SetValue(_editableTarget, newValue);

            OnTargetUpdated(_propertyAdapter.PropertyName);
        }

        private void OnTargetUpdated(string targetPropertyName)
        {
            var handler = TargetUpdated;

            if (handler != null)
                handler(this, new PropertyUpdatedEventArgs(targetPropertyName));
        }

        public void UpdateValidationState(ValidationState state)
        {
            if (state == null)
            {
                _component.SetError(null);
                return;
            }

            string error;
            state.TryGetPropertyError(_propertyAdapter.PropertyName, out error);

            _component.SetError(error);
        }
    }

    public class ScalarPropertyDataEditor<TData, TComponent> : StringPropertyDataEditor<TData>
        where TComponent : IEditorComponent<string>
    {
        private readonly TComponent _component;

        public ScalarPropertyDataEditor(IPropertyAdapter<TData, string> propertyAdapter, TComponent component)
            : base(propertyAdapter, component)
        {
            _component = component;
        }

        public new TComponent Component
        {
            get { return _component; }
        }
    }
}