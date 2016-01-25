using System;

namespace Composite.Core.PropertyEditors
{
    public interface IEditorComponent<TValue> : IBrowserComponent<TValue>
    {
        event EventHandler<EventArgs> ValueUpdated;
        
        void SetReadOnly(bool readOnly);
    }
}