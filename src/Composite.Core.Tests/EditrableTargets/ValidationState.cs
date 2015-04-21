using System;
using System.Collections.Generic;

namespace Composite.Core.Tests.EditrableTargets
{
    public class ValidationState
    {
        public static readonly ValidationState Valid = new ValidationState(new Dictionary<string, string>(0));

        private readonly IDictionary<string, string> _errors;

        public ValidationState(IDictionary<string, string> errors)
        {
            if (errors == null) throw new ArgumentNullException("errors");

            _errors = errors;
        }

        public bool IsValid
        {
            get
            {
                return _errors.Count == 0;
            }
        }

        public bool TryGetPropertyError(string propertyName, out string error)
        {
            return _errors.TryGetValue(propertyName, out error);
        }
    }
}