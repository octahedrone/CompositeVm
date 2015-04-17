using System;

namespace Composite.Core.Tests
{
    public interface IDataEditor<TData>
    {
        TData EditableTarget { get; set; }

        event EventHandler<PropertyUpdatedEventArgs> TargetUpdated;
    }
}