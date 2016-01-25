namespace Composite.Core.PropertyEditors
{
    public interface IBrowserComponent<TValue>
    {
        TValue GetValue();

        void SetValue(TValue value);

        void SetError(string error);
    }
}