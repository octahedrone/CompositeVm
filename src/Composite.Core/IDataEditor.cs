using System;

namespace Composite.Core
{
    public interface IDataComponent<TData>
    {
        TData EditableTarget { get; set; }
    }

    public interface IDataEditor<TData> : IDataComponent<TData>
    {
        event EventHandler<EventArgs> TargetUpdated;
    }
}