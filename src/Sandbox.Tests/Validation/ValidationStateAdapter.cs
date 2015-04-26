namespace Sandbox.Tests.Validation
{
    public static class ValidationStateAdapter
    {
        public static string GetPropertyError(string property, ValidationState state)
        {
            string error;
            return state.TryGetPropertyError(property, out error) ? error : null;
        }
    }
}