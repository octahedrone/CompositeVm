using System;

namespace Composite.Core.Tests
{
    public interface IStringEditorComponent
    {
        event EventHandler<EventArgs> ValueUpdated;

        string GetValue();

        void SetValue(string value);
    }
}