using System;
using System.Linq.Expressions;

namespace Composite.Core.PropertyAdapaters
{
    public class ValueTypeGenericPropertyAdapter<TEntity, TValue> : IPropertyAdapter<TEntity, TValue>
        where TEntity : struct
    {
        private readonly string _propertyName;
        private readonly Func<TEntity, TValue, TEntity> _valueSetter;
        private readonly Func<TEntity, TValue> _valueGetter;

        public ValueTypeGenericPropertyAdapter(Expression<Func<TEntity, TValue>> getterExpression)
            : this(getterExpression, null)
        {
        }

        public ValueTypeGenericPropertyAdapter(Expression<Func<TEntity, TValue>> getterExpression,
            Func<TEntity, TValue, TEntity> valueSetter)
        {
            _valueGetter = getterExpression.Compile();
            _valueSetter = valueSetter;

            _propertyName = getterExpression.Body.GetPropertyName();
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public bool IsReadOnly
        {
            get { return _valueSetter == null; }
        }

        public TValue GetValue(TEntity source)
        {
            return _valueGetter(source);
        }

        public TEntity SetValue(TEntity target, TValue value)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(string.Format("Property '{0}' is read only.", PropertyName));

            return _valueSetter(target, value);
        }
    }
}