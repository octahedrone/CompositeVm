using System;

namespace Composite.Core.Tests
{
    public interface IEditorComponent<TValue>
    {
        event EventHandler<EventArgs> ValueUpdated;

        TValue GetValue();

        void SetValue(TValue value);

        void SetError(string error);
    }
}