using System;
using Composite.Core.TypeChecks;

namespace Composite.Core.PropertyEditors
{
    public class PropertyDataEditor<TData, TValue, TValidationState> : IValidatedDataEditor<TData, TValidationState>
    {
        private static readonly INullableCheck<TData> TargetNullCheck = ValueChecks.GetNullableCheck<TData>();

        private readonly IEditorComponent<TValue> _component;
        private readonly IPropertyAdapter<TData, TValue> _propertyAdapter;
        private readonly Func<string, TValidationState, string> _propertyErrorProvider;

        private TData _editableTarget;

        public PropertyDataEditor(IPropertyAdapter<TData, TValue> propertyAdapter, IEditorComponent<TValue> component, Func<string, TValidationState, string> propertyErrorProvider)
        {
            if (propertyAdapter == null) throw new ArgumentNullException("propertyAdapter");
            if (component == null) throw new ArgumentNullException("component");
            if (propertyErrorProvider == null) throw new ArgumentNullException("propertyErrorProvider");

            _propertyAdapter = propertyAdapter;
            _component = component;
            _propertyErrorProvider = propertyErrorProvider;

            _component.ValueUpdated += OnComponentUpdatedValue;

            _component.SetReadOnly(_propertyAdapter.IsReadOnly);
        }

        public TData EditableTarget
        {
            get { return _editableTarget; }
            set
            {
                _editableTarget = value;

                var newValue = TargetNullCheck.IsNull(_editableTarget)
                    ? default(TValue) 
                    : _propertyAdapter.GetValue(_editableTarget);

                _component.SetValue(newValue);
            }
        }

        public object Component
        {
            get { return _component; }
        }

        public event EventHandler<EventArgs> TargetUpdated;

        private void OnComponentUpdatedValue(object sender, EventArgs e)
        {
            if (TargetNullCheck.IsNull(_editableTarget))
            {
                return;
            }

            var newValue = _component.GetValue();

            _editableTarget = _propertyAdapter.SetValue(_editableTarget, newValue);

            OnTargetUpdated();
        }

        private void OnTargetUpdated()
        {
            var handler = TargetUpdated;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void UpdateValidationState(TValidationState state)
        {
            var error = _propertyErrorProvider(_propertyAdapter.PropertyName, state);

            _component.SetError(error);
        }

        public void ClearValidationState()
        {
            _component.SetError(null);
        }
    }
}