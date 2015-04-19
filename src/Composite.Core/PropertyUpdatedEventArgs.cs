using System;

namespace Composite.Core
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