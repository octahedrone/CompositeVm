using System;

namespace Composite.Core.TypeChecks
{
    public static class ValueChecks
    {
        public static INullableCheck<TValue> GetNullableCheck<TValue>()
        {
            return IsReferenceType<TValue>()
                ? ReferenceTypeNullableCheck<TValue>.Instance
                : ValueTypeNullableCheck<TValue>.Instance;
        }

        private static bool IsReferenceType<TValue>()
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