using Composite.Core.PropertyAdapaters;
using Composite.Core.Tests.StructTargetTests.Target;
using FluentAssertions;
using NUnit.Framework;

namespace Composite.Core.Tests
{
    [TestFixture]
    public class ValueTypeGenericPropertyAdapterTests
    {
        [Test]
        public void PropertyNameIsRetrievedCorrectly()
        {
            var sut = new ValueTypeGenericPropertyAdapter<EditableStruct, string>(_ => _.Text);

            // assert
            sut.PropertyName.Should().Be("Text");
            sut.IsReadOnly.Should().BeTrue();
        }

        [Test]
        public void PropertyValueIsRetrievedCorrectly()
        {
            var sut = new ValueTypeGenericPropertyAdapter<EditableStruct, string>(_ => _.Text);

            // act
            const string text = "Value";
            var value = sut.GetValue(new EditableStruct { Text = text });

            // assert
            value.Should().Be(text);
        }

        [Test]
        public void PropertyValueIsUpdatedCorrectly()
        {
            var sut = new ValueTypeGenericPropertyAdapter<EditableStruct, string>(_ => _.Text, (t, v) =>
            {
                t.Text = v;
                return t;
            });

            const string initialText = "Initial";
            const string updatedText = "Updated";

            // act
            var target = new EditableStruct { Text = initialText };
            target = sut.SetValue(target, updatedText);

            // assert
            sut.IsReadOnly.Should().BeFalse();
            target.Text.Should().Be(updatedText);
        }
    }
}