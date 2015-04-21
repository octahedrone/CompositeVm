namespace Composite.Core
{
    public interface IPropertyAdapter<TEntity, TValue>
    {
        string PropertyName { get; }

        TValue GetValue(TEntity source);

        TEntity SetValue(TEntity target, TValue value);
    }
}