namespace Composite.Core.Tests
{
    public interface IStringPropertyAdapter<TEntity>
    {
        string PropertyName { get; }

        string GetValue(TEntity source);

        TEntity SetValue(TEntity target, string value);
    }
}