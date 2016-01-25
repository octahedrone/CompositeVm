namespace Composite.Core
{
    public interface IValidatedComponent<in TValidationState>
    {
        void UpdateValidationState(TValidationState state);

        void ClearValidationState();
    }
}