namespace Composite.Core
{
    public interface IValidatedDataEditor<in TValidationState>
    {
        void UpdateValidationState(TValidationState state);

        void ClearValidationState();
    }
}