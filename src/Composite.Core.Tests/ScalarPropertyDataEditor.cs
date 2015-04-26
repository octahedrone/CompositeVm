using Composite.Core.PropertyEditors;

namespace Composite.Core.Tests
{
    public class ScalarPropertyDataEditor<TData, TValue, TComponent> : PropertyDataEditor<TData, TValue, ValidationState>
        where TComponent : IEditorComponent<TValue>
    {
        private readonly TComponent _component;

        public ScalarPropertyDataEditor(IPropertyAdapter<TData, TValue> propertyAdapter, TComponent component)
            : base(propertyAdapter, component, ValidationStateAdapter.GetPropertyError)
        {
            _component = component;
        }



        public new TComponent Component
        {
            get { return _component; }
        }
    }
}