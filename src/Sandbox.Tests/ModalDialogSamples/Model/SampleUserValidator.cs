using System;
using System.Collections.Generic;
using Composite.Core.Validation;
using Sandbox.Tests.Validation;

namespace Sandbox.Tests.ModalDialogSamples.Model
{
    public class SampleUserValidator : IValidator<SampleUser, ValidationState>
    {
        public ValidationState Validate(SampleUser target)
        {
            if (String.IsNullOrEmpty(target.Name) || string.IsNullOrWhiteSpace(target.Name))
            {
                return new ValidationState(new Dictionary<string, string>
                {
                    {"Name", "Please enter non-empty or whitespace only name"}
                });
            }

            return ValidationState.Valid;
        }
    }
}