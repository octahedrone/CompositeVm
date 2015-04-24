namespace Composite.Core
{
    public interface IValidatedDataEditor<TData, in TValidationState> : IDataEditor<TData>
    {
        void UpdateValidationState(TValidationState state);

        void ClearValidationState();
    }
}