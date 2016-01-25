using System;

namespace Composite.Core
{
    public interface IDataBrowser<TData>
    {
        TData EditableTarget { get; set; }
    }

    public interface IDataEditor<TData> : IDataBrowser<TData>
    {
        event EventHandler<EventArgs> TargetUpdated;
    }
}