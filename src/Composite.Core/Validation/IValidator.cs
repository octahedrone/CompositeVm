namespace Composite.Core.Validation
{
    public interface IValidator<in TTarget, out TResult>
    {
        TResult Validate(TTarget target);
    }
}