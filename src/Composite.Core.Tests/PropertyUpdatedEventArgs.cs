using System;

namespace Composite.Core.Tests
{
    public class PropertyUpdatedEventArgs : EventArgs
    {
        public PropertyUpdatedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }
    }
}