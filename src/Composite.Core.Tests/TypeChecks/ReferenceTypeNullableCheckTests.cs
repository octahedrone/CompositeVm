using Composite.Core.Tests.ClassTargetTests.Target;
using Composite.Core.Tests.StructTargetTests.Target;
using Composite.Core.TypeChecks;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests.TypeChecks
{
    [TestFixture]
    public class ReferenceTypeNullableCheckTests
    {
        [Test]
        public void NotNullValueIsCorrectlyDetected()
        {
            var target = new EditableClass();

            ReferenceTypeNullableCheck<EditableClass>.Instance.IsNull(target).Should().BeFalse();
        }

        [Test]
        public void NullValueIsCorrectlyDetected()
        {
            EditableClass target = null;

            ReferenceTypeNullableCheck<EditableClass>.Instance.IsNull(target).Should().BeTrue();
        }

        [Test]
        public void NullableNullValueIsCorrectlyDetected()
        {
            var target = default(EditableStruct?);

            ReferenceTypeNullableCheck<EditableStruct?>.Instance.IsNull(target).Should().BeTrue();
        }

        [Test]
        public void NullableNotNullValueIsCorrectlyDetected()
        {
            EditableStruct? target = default(EditableStruct);

            ReferenceTypeNullableCheck<EditableStruct?>.Instance.IsNull(target).Should().BeFalse();
        }
    }
}