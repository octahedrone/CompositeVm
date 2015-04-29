using System;

namespace Composite.Core
{
    public interface IDataEditor<TData>
    {
        TData EditableTarget { get; set; }

        event EventHandler<EventArgs> TargetUpdated;
    }
}