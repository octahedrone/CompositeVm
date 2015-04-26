using System;
using System.Linq.Expressions;

namespace Composite.Core.PropertyAdapters
{
    public static class ExpressionTools
    {
        public static string GetPropertyName(this Expression expression)
        {
            var body = expression as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("'expression' should be a property expression");
            }

            if (!(body.Expression is ParameterExpression))
            {
                const string message =
                    "'expression' should be direct, properties nested to other objects are not supported (i.e. lambdas like v => vm.Object.Property)";
                throw new ArgumentException(message);
            }

            return body.Member.Name;
        }
    }
}