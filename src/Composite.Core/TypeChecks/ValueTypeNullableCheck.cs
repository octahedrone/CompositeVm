namespace Composite.Core.TypeChecks
{
    public class ValueTypeNullableCheck<TValue> : INullableCheck<TValue>
    {
        public static readonly INullableCheck<TValue> Instance = new ValueTypeNullableCheck<TValue>();

        private ValueTypeNullableCheck()
        {
        }

        public bool IsNull(TValue value)
        {
            return false;
        }
    }
}