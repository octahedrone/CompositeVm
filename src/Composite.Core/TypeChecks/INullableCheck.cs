namespace Composite.Core.TypeChecks
{
    public interface INullableCheck<in TValue>
    {
        bool IsNull(TValue value);
    }
}