using System;

namespace Composite.Core
{
    public interface IDataEditor<TData>
    {
        TData EditableTarget { get; set; }

        event EventHandler<PropertyUpdatedEventArgs> TargetUpdated;
    }

    public interface IValidatedDataEditor<TData, in TValidationState> : IDataEditor<TData>
    {
        void UpdateValidationState(TValidationState state);
    }
}