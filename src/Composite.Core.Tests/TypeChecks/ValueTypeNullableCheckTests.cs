using Composite.Core.Tests.StructTargetTests.Target;
using Composite.Core.TypeChecks;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests.TypeChecks
{
    [TestFixture]
    public class ValueTypeNullableCheckTests
    {
        [Test]
        public void DefaultValueIsCorrectlyDetected()
        {
            var target = default(EditableStruct);

            ValueTypeNullableCheck<EditableStruct>.Instance.IsNull(target).Should().BeFalse();
        }
    }
}