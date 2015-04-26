using Composite.Core.Validation;
using Sandbox.Tests.Validation;

namespace Sandbox.Tests.ModalDialogSamples.Model
{
    public class SampleUserValidator : IValidator<SampleUser, ValidationState>
    {
        public ValidationState Validate(SampleUser target)
        {
            return ValidationState.Valid;
        }
    }
}