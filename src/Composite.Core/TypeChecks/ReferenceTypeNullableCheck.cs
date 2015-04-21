namespace Composite.Core.TypeChecks
{
    public class ReferenceTypeNullableCheck<TValue> : INullableCheck<TValue>
    {
        public static readonly INullableCheck<TValue> Instance = new ReferenceTypeNullableCheck<TValue>();

        private ReferenceTypeNullableCheck()
        {
        }

        public bool IsNull(TValue value)
        {
            return Equals(default(TValue), value);
        }
    }
}