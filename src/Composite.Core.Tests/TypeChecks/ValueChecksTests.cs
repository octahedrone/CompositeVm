using System;
using Composite.Core.Tests.ClassTargetTests.Target;
using Composite.Core.Tests.StructTargetTests.Target;
using Composite.Core.TypeChecks;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests.TypeChecks
{
    [TestFixture]
    public class ValueChecksTests
    {
        [Test]
        public void ValueTypeIsRecognizedCorrectly()
        {
            ValueChecks.IsNullableType<EditableStruct>().Should().BeFalse();
        }

        [Test]
        public void ReferenceTypeIsRecognizedCorrectly()
        {
            ValueChecks.IsNullableType<EditableClass>().Should().BeTrue();
        }

        [Test]
        public void NullableValueTypeIsRecognizedCorrectly()
        {
            ValueChecks.IsNullableType<EditableStruct?>().Should().BeTrue();
        }

        [Test]
        public void NullableCheckIsRecognizedCorrectlyForValueType()
        {
            var check = ValueChecks.GetNullableCheck<EditableStruct>();

            check.IsNull(default(EditableStruct)).Should().BeFalse();
        }

        [Test]
        public void NullableCheckIsRecognizedCorrectlyForClassType()
        {
            var check = ValueChecks.GetNullableCheck<EditableClass>();

            check.IsNull(default(EditableClass)).Should().BeTrue();
            check.IsNull(new EditableClass()).Should().BeFalse();
        }

        [Test]
        public void NullableCheckIsRecognizedCorrectlyForNullableType()
        {
            var check = ValueChecks.GetNullableCheck<EditableStruct?>();

            check.IsNull(default(EditableStruct?)).Should().BeTrue();
            check.IsNull(default(EditableStruct)).Should().BeFalse();
        }
    }
}