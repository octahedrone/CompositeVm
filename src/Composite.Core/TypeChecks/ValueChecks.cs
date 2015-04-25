using System;

namespace Composite.Core.TypeChecks
{
    public static class ValueChecks
    {
        public static INullableCheck<TValue> GetNullableCheck<TValue>()
        {
            return IsNullableType<TValue>()
                ? ReferenceTypeNullableCheck<TValue>.Instance
                : ValueTypeNullableCheck<TValue>.Instance;
        }

        public static bool IsNullableType<TValue>()
        {
            var type = typeof (TValue);
            if (!type.IsValueType)
            {
                return true;
            }

            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}