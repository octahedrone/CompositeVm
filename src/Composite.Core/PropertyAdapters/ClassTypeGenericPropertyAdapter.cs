﻿using System;
using System.Linq.Expressions;

namespace Composite.Core.PropertyAdapters
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
    }
}