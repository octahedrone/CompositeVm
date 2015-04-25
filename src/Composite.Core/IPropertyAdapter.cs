namespace Composite.Core
{
    public interface IPropertyAdapter<TEntity, TValue>
    {
        string PropertyName { get; }

        bool IsReadOnly { get; }

        TValue GetValue(TEntity source);

        TEntity SetValue(TEntity target, TValue value);
    }
}