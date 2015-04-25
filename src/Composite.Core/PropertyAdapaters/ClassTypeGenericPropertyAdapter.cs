using System;
using System.Linq.Expressions;

namespace Composite.Core.PropertyAdapaters
{
    public class ClassTypeGenericPropertyAdapter<TEntity, TValue> : IPropertyAdapter<TEntity, TValue>
        where TEntity : class
    {
        private readonly string _propertyName;
        private readonly Action<TEntity, TValue> _valueSetter;
        private readonly Func<TEntity, TValue> _valueGetter;

        public ClassTypeGenericPropertyAdapter(Expression<Func<TEntity, TValue>> getterExpression) 
            : this(getterExpression, null)
        {
        }

        public ClassTypeGenericPropertyAdapter(Expression<Func<TEntity, TValue>> getterExpression, Action<TEntity, TValue> valueSetter)
        {
            _valueGetter = getterExpression.Compile();
            _valueSetter = valueSetter;

            _propertyName = GetPropertyName(getterExpression);
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
            if (source == null) throw new ArgumentNullException("source");

            return _valueGetter(source);
        }

        public TEntity SetValue(TEntity target, TValue value)
        {
            if (target == null) throw new ArgumentNullException("target");

            if (IsReadOnly)
                throw new InvalidOperationException(string.Format("Property '{0}' is read only.", PropertyName));

            _valueSetter(target, value);

            return target;
        }

        private static string GetPropertyName(Expression<Func<TEntity, TValue>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("'expression' should be a property expression");
            }

            if (!(body.Expression is ParameterExpression))
            {
                const string message = "'expression' should be direct, properties nested to other objects are not supported (i.e. lambdas like v => vm.Object.Property)";
                throw new ArgumentException(message);
            }

            return body.Member.Name;
        }
    }
}