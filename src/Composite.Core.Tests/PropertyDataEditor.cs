using System;
using Composite.Core.Tests.EditrableTargets;

namespace Composite.Core.Tests
{
    public class PropertyDataEditor<TData, TValue> : IValidatedDataEditor<TData, ValidationState>
    {
        private readonly IEditorComponent<TValue> _component;
        private readonly IPropertyAdapter<TData, TValue> _propertyAdapter;

        private TData _editableTarget;

        public PropertyDataEditor(IPropertyAdapter<TData, TValue> propertyAdapter, IEditorComponent<TValue> component)
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
                _editableTarget = value;

                var newValue = _propertyAdapter.GetValue(_editableTarget);

                _component.SetValue(newValue);
            }
        }

        public object Component
        {
            get { return _component; }
        }

        public event EventHandler<PropertyUpdatedEventArgs> TargetUpdated;

        private void OnComponentUpdatedValue(object sender, EventArgs e)
        {
            var newValue = _component.GetValue();

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

    public class ScalarPropertyDataEditor<TData, TValue, TComponent> : PropertyDataEditor<TData, TValue>
        where TComponent : IEditorComponent<TValue>
    {
        private readonly TComponent _component;

        public ScalarPropertyDataEditor(IPropertyAdapter<TData, TValue> propertyAdapter, TComponent component)
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